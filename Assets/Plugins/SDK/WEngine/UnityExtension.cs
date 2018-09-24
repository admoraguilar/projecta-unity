using UnityEngine;
using System;


namespace WEngine {
    /// <summary>
    /// Extension methods for Unity.
    /// Use these instead of the original Unity methods to make the dependency injection
    /// work:
    /// - ExtensionInstantiate();  -> UnityEngine.Object.Instantiate();
    /// - ExtensionAddComponent(); -> GameObject.AddComponent();
    /// - ExtensionCreateInstance(); -> new Object(); -> ScriptableObject.CreateInstance();
    /// </summary>
    public static class UnityExtension {
        public static event Action<object> OnInstantiate = delegate { };
        public static event Action<GameObject, Component> OnAddComponent = delegate { };
        public static event Action<object> OnCreateInstance = delegate { };


        /// <summary>
        /// Instantiates an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="onInstantiated">Intiialize the object through this callback.</param>
        /// <returns></returns>
        public static T ExtensionInstantiate<T>(T original, Action<T> onInstantiated = null) where T : UnityEngine.Object {
            T objInstance = UnityEngine.Object.Instantiate(original);
            if(onInstantiated != null) onInstantiated(objInstance);
            OnInstantiate(objInstance);
            return objInstance;
        }


        /// <summary>
        /// Adds a component to a gameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="onAddedComponent">Intiialize the component through this callback.</param>
        /// <returns></returns>
        public static T ExtensionAddComponent<T>(this GameObject go, Action<T> onAddedComponent = null) where T : Component {
            T component = go.AddComponent<T>();
            if(onAddedComponent != null) onAddedComponent(component);
            OnAddComponent(go, component);
            return component;
        }


        /// <summary>
        /// Creates an instance of an type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onCreateInstance">Intiialize the object through this callback.</param>
        /// <returns></returns>
        public static T CreateInstance<T>(Action<T> onCreateInstance = null) {
            return (T)CustomCreateInstance(typeof(T), (obj) => {
                if(onCreateInstance != null) onCreateInstance((T)obj);
            });
        }


        /// <summary>
        /// Creates an instance of an type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="onCreateInstance">Intiialize the object through this callback.</param>
        /// <returns></returns>
        public static object CustomCreateInstance(Type type, Action<object> onCreateInstance = null) {
            object obj = null;

            // For special cases
            if(type.IsSubclassOf(typeof(ScriptableObject))) obj = ScriptableObject.CreateInstance(type);
            else obj = Activator.CreateInstance(type);

            if(onCreateInstance != null) onCreateInstance(obj);
            OnCreateInstance(obj);
            return obj;
        }

        public static void RunOnChildrenRecursive(this GameObject go, Action<GameObject> action) {
            if(go == null) return;
            foreach(var trans in go.GetComponentsInChildren<Transform>(true)) {
                action(trans.gameObject);
            }
        }
    }
}