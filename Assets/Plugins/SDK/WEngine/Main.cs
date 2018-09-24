using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using RSG.Scene.Query;


namespace WEngine {
    /// <summary>
    /// Main entry point.
    /// 
    /// <para>
    /// This class is the main back-bone of the dependency injection nature
    /// of this framework. This is responsible for:
    /// </para>
    /// 
    /// <list type="bullet">
    /// <item><description>Auto-injection: Inject dependencies via the [Inject] attribute.</description></item>
    /// <item><description>Manual injection: Inject dependencies via Main.Inject();</description></item>
    /// <item><description>Cached modules: Access modules referenced in your GameConfig.</description></item>
    /// </list>
    /// 
    /// <para>
    /// NOTES:
    /// Auto-injection doesn't work for Awake(). But if you wanna do something
    /// on Awake() and you need that dependency you can instead use Manual 
    /// injection.
    /// </para>
    /// </summary>
    /// 
    /// TODO: Make the main be able to run in coroutine mode so we prevent the
    ///       black screen at startup. But make sure to warn the user that
    ///       when using coroutine mode the first scene shall not have any
    ///       dependecies or else we're fucked.
    public static class Main {
        private static List<object> gameModules = new List<object>();
        private static Dictionary<Type, List<FieldInfo>> injectableTypeCache = new Dictionary<Type, List<FieldInfo>>();


        /// <summary>
        /// <para>
        /// Checks if a module is of some type. Useful if you want to have module-dependent code.
        /// </para>
        /// 
        /// <example>
        /// <code>
        /// if(IsModuleAssignableFrom(MyModule, typeof(ModuleA)) {
        ///     // Do something if MyModule is of type ModuleA
        /// } else if(IsModuleAssignableFrom(MyModule, typeof(ModuleB)) {
        ///     // Do something if MyModule is of type ModuleB
        /// }
        /// </code>
        /// </example>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="moduleToCompare"></param>
        /// <returns></returns>
        public static bool IsModuleAssignableFrom(object module, Type moduleToCompare) {
            object gameModule = GetGameModule(module.GetType());
            return gameModule != null ? gameModule.GetType().IsAssignableFrom(moduleToCompare) : false;
        }

        public static T GetGameModule<T>() {
            object gameModule = GetGameModule(typeof(T));
            return gameModule != null ? (T)gameModule : default(T);
        }

        public static object GetGameModule(Type type) {
            object gameModule = gameModules.FirstOrDefault(module => type.IsAssignableFrom(module.GetType()));
            //if(gameModule == null) Debug.Log(string.Format("Couldn't find module of type: [{0}] please make one.", type.Name));
            return gameModule;
        }

        public static void Inject(params object[] objs) {
            foreach(object obj in objs) {
                InjectGameModules(new KeyValuePair<Type, object>(obj.GetType(), obj));
            }
        }

        public static void Inject(KeyValuePair<Type, object>[] objs) {
            InjectGameModules(objs);
        }

        [RunBeforeGameLoad()]
        private static void RunMain() {
            // Auto inject events
            // These events make auto-injection work. So say when we instantiate an object
            // a callback gets called that injects dependencies on that instantiated object.
            SceneManager.sceneLoaded += OnSceneLoaded;
            UnityExtension.OnInstantiate += OnInstantiate;
            UnityExtension.OnAddComponent += OnAddComponent;
            UnityExtension.OnCreateInstance += OnCreateInstance;

            // Run through every config inside the GameConfigs in Resources
            // folders
            var gameConfigs = Resources.LoadAll<GameConfig>("")
                .Where(gc => gc.IsEnabled);
            foreach(var gameConfig in gameConfigs) {
                var configs = gameConfig.Configs.Where(c => c.IsIncluded);

                foreach(var config in configs) {
                    var modules = config.Modules.Where(m => m.IsIncluded);

                    foreach(var module in modules) {
                        foreach(ScriptableObject so in module.ScriptableObjects) {
                            gameModules.Add(so);
                        }
                        foreach(GameObject go in module.GameObjects) {
                            var goInstance = UnityExtension.ExtensionInstantiate(go);
                            goInstance.name = go.name;
                            UnityEngine.Object.DontDestroyOnLoad(goInstance);
                            
                            foreach(MonoBehaviour m in goInstance.GetComponents<MonoBehaviour>()) {
                                gameModules.Add(m);
                            }
                        }
                    }
                }
            }

            // Inject game modules on themselves
            foreach(object module in gameModules) {
                InjectGameModules(new KeyValuePair<Type, object>(module.GetType(), module));
            }

            // Initialize game modules base on priority to somehow prevent race
            // conditions
            List<object> initializables = new List<object>(gameModules);
            initializables.Sort((object a, object b) => {
                var aInit = a as IInitializable;
                var bInit = b as IInitializable;

                if(aInit != null && bInit == null) return 1;
                else if(aInit == null && bInit != null) return -1;
                else if (aInit == null && bInit == null) return 0;
                else return aInit.Priority < bInit.Priority ? -1 : aInit.Priority > bInit.Priority ? 1 : 0;
            });
            foreach(object gameModule in initializables) {
                var module = gameModule as IInitializable;
                //Debug.Log(gameModule.GetType().ToString() + ": " + (module != null ? module.Priority.ToString() : "-1"));
                if(module != null) module.Initialize();
            }

            // Instantiate mono dummy
            var mainMono = new GameObject("MainMono").ExtensionAddComponent<MainMono>();
            UnityEngine.Object.DontDestroyOnLoad(mainMono.gameObject);
            mainMono.UnityOnApplicationQuit += () => {
                foreach(object gameModule in gameModules) {
                    var module = gameModule as IDeinitializable;
                    if(module != null) module.Deinitialize();
                }
            };
        }

        #region Auto-injection Events
        private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            IEnumerable<GameObject> gos = new SceneTraversal().RootNodes();
            foreach(var go in gos) {
                var monos = go.GetComponentsInChildren<MonoBehaviour>(true);
                foreach(var mono in monos) {
                    //Debug.Log(go.name + " | " + mono.name + " | " + mono.GetType().Name);
                    InjectGameModules(new KeyValuePair<Type, object>(mono.GetType(), mono));
                }
            }
        }

        private static void OnInstantiate(object obj) {
            // Check its type
            var go = obj as GameObject;
            var mono = obj as MonoBehaviour;

            // If it's a mono then we'll get the gameobject
            if(mono) { go = mono.gameObject; }

            if(go) {
                var monos = go.GetComponentsInChildren<MonoBehaviour>(true);
                foreach(var m in monos) {
                    InjectGameModules(new KeyValuePair<Type, object>(m.GetType(), m));
                }
                return;
            }

            // If the instantiated is a scriptable object
            var so = obj as ScriptableObject;
            if(so) {
                InjectGameModules(new KeyValuePair<Type, object>(so.GetType(), so));
                return;
            }
        }

        private static void OnAddComponent(GameObject arg1, Component arg2) {
            var mono = arg2 as MonoBehaviour;
            InjectGameModules(new KeyValuePair<Type, object>(mono.GetType(), mono));
        }

        private static void OnCreateInstance(object obj) {
            InjectGameModules(new KeyValuePair<Type, object>(obj.GetType(), obj));
        }
        #endregion

        private static void InjectGameModules(params KeyValuePair<Type, object>[] objs) {
            // Some hooks
            NullFill.InjectNullFillable(objs);

            foreach(KeyValuePair<Type, object> kvp in objs) {
                IEnumerable<FieldInfo> injectables = null;
                if(injectableTypeCache.ContainsKey(kvp.Key)) {
                    injectables = injectableTypeCache[kvp.Key];
                } else {
                    // We cache the injectables so that the next time we
                    // inject, reflection wouldn't be so slow.
                    injectables = kvp.Key.GetFields(BindingFlags.Instance |
                                                    BindingFlags.Public |
                                                    BindingFlags.NonPublic)
                                         .Where(fi => fi.GetCustomAttributes(typeof(InjectAttribute), false).Length != 0);
                    injectableTypeCache[kvp.Key] = new List<FieldInfo>(injectables);
                }
                foreach(var injectable in injectables) {
                    // Inject the module
                    object gameModule = GetGameModule(injectable.FieldType);
                    if(gameModule != null) injectable.SetValue(kvp.Value, gameModule);
                    
                    // Disabling because it seems unclear how this thing works
                    //var injectAttr = (InjectAttribute)injectable.GetCustomAttributes(typeof(InjectAttribute), false)[0];
                    //if(injectAttr.IsInjectInstance) {
                    //    // Check if we should inject an instance instead, if yes then we
                    //    // just instantiate a copy of the module we've just injected
                    //    // This is helpful for things that are unique per object such as
                    //    // HP module perhaps
                    //    object injectableValue = injectable.GetValue(kvp.Value);
                    //    if(injectableValue as UnityEngine.Object) {
                    //        injectable.SetValue(kvp.Value, injectableValue);
                    //    }
                    //}

                    // Recursive inject to the type's field
                    InjectGameModules(new KeyValuePair<Type, object>(injectable.FieldType, injectable.GetValue(kvp.Value)));
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RunBeforeGameLoad() {
            // Get all methods in the assembly that has the [RunBeforeGameLoad] attribute
            // and invoke it
            var assembly = Assembly.GetAssembly(typeof(Main));
            var assemblyTypes = assembly.GetTypes();

            var beforeGameLoadMethods = new List<MethodInfo>();
            foreach(Type type in assemblyTypes) {
                beforeGameLoadMethods
                    .AddRange(type.GetMethods(BindingFlags.Static | 
                                              BindingFlags.Public | 
                                              BindingFlags.NonPublic | 
                                              BindingFlags.Instance)
                    .Where(
                        m => Attribute.GetCustomAttribute(m, typeof(RunBeforeGameLoadAttribute)) != null
                    ));
            }

            // Sort the invocation list by priority indicated in the attribute
            beforeGameLoadMethods.Sort((MethodInfo a, MethodInfo b) => {
                var aAttr = (RunBeforeGameLoadAttribute)Attribute.GetCustomAttribute(a, typeof(RunBeforeGameLoadAttribute));
                var bAttr = (RunBeforeGameLoadAttribute)Attribute.GetCustomAttribute(b, typeof(RunBeforeGameLoadAttribute));

                return aAttr.Priority < bAttr.Priority ? -1 : aAttr.Priority > bAttr.Priority ? 1 : 0;
            });

            foreach(MethodInfo method in beforeGameLoadMethods) {
                method.Invoke(null, null);
            }
        }
    }


    /// <summary>
    /// Intended use is to attach a callback when the application is quitted so we can
    /// do things like de-initializing game modules.
    /// </summary>
    public class MainMono : MonoBehaviour {
        public event Action UnityOnApplicationQuit = delegate { };


        private void OnApplicationQuit() {
            UnityOnApplicationQuit();
        }
    }


    public static class NullFill {
        private static Dictionary<Type, List<FieldInfo>> nullFillableTypeCache = new Dictionary<Type, List<FieldInfo>>();


        public static void InjectNullFillable(params KeyValuePair<Type, object>[] objs) {
            foreach(KeyValuePair<Type, object> kvp in objs) {
                IEnumerable<FieldInfo> nullFillables = null;
                if(nullFillableTypeCache.ContainsKey(kvp.Key)) {
                    nullFillables = nullFillableTypeCache[kvp.Key];
                } else {
                    // We cache the null fillable so that the next time we
                    // fill, reflection wouldn't be so slow.
                    nullFillables = kvp.Key.GetFields(BindingFlags.Instance |
                                                    BindingFlags.Public |
                                                    BindingFlags.NonPublic)
                                         .Where(fi => fi.GetCustomAttributes(typeof(NullFillableAttribute), false).Length != 0);
                    nullFillableTypeCache[kvp.Key] = new List<FieldInfo>(nullFillables);

                }
                foreach(var nullFillable in nullFillables) {
                    // THIS IMPLEMENTATION CAN BE REPLACED BY THE ONE IN THE 
                    // UTILITIES CLASS
                    // Make sure that the field is null
                    object value = nullFillable.GetValue(kvp.Value);
                    if(value == null) {
                        // Fill the null value
                        if(value == null && typeof(MonoBehaviour).IsAssignableFrom(value.GetType())) {
                            value = new GameObject().AddComponent(value.GetType());
                        }

                        if(value == null && typeof(ScriptableObject).IsAssignableFrom(value.GetType())) {
                            value = ScriptableObject.CreateInstance(value.GetType());
                        }

                        if(value == null && typeof(object).IsAssignableFrom(value.GetType())) {
                            value = Activator.CreateInstance(value.GetType());
                        } 

                        nullFillable.SetValue(kvp.Value, value);
                    }

                    // Recursive null fill to the type's field
                    InjectNullFillable(new KeyValuePair<Type, object>(nullFillable.FieldType, nullFillable.GetValue(kvp.Value)));
                }
            }
        }
    }
}