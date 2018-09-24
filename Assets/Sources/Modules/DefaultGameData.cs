using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;


/// <summary>
/// This module holds all the data that the game creates. It acts like the big data
/// of the game.
/// </summary>
[CreateAssetMenu(fileName = "DefaultGameData", menuName = "WEngine/Modules/AGameData/DefaultGameData")]
public class DefaultGameData : AGameData, IInitializable, IDeinitializable {
    public int Priority { get { return 0; } }
    public override bool IsLoaded { get { return isLoaded; } }

    public override TransientData Transient { get { return transient; } }
    public override PersistentData Persistent { get { return persistent; } }
    public override ContainerData Container { get { return container; } }
    public override KeyData Key { get { return key; } }

    [Inject, SerializeField] private TransientData transient = new TransientData();
    [Inject, SerializeField] private PersistentData persistent = new PersistentData();
    [Inject, SerializeField] private ContainerData container = new ContainerData();
    [Inject, SerializeField] private KeyData key;

    [Header("Debug")]
    [SerializeField] private bool isLoaded = false;

    private ContainerData oldContainer = new ContainerData();


    public void Initialize() {
        CacheValues();
        Reset();

        // Instantiate mono
        var defaultGameDataMono = new GameObject("DefaultGameDataMono").ExtensionAddComponent<DefaultGameDataMono>();
        DontDestroyOnLoad(defaultGameDataMono.gameObject);
        defaultGameDataMono.Initialize();

        // Hook callbacks
        defaultGameDataMono.OnLoadedData += () => {
            isLoaded = true;
        };
    }

    public void Deinitialize() {
        Reset();   
    }

    private void CacheValues() {
        // Copy current container values first so we can reset
        oldContainer.Ui = container.Ui;
    }

    private void Reset() {
        // Instead of doing a new PlayerTransientData() to load up
        // default values, we instead load it up by hand so that we
        // can do things such as randomization
        string randomKey = UnityEngine.Random.Range(0, 1000000).ToString();
        transient.Player.UserName = "User" + randomKey;
        transient.Player.Password = "Pass";
        transient.Player.DisplayName = "User" + randomKey;
        transient.Player.IsNewPlayer = true;
        transient.Player.IsAdmin = false;

        // Here it is safe to just do new()
        persistent.App = new AppPersistentData();
        persistent.Player = new PlayerPersistentData();

        container.Asset.Avatars = new Dictionary<string, AvatarData>();
        container.Asset.Worlds = new Dictionary<string, WorldData>();
        container.Ui = oldContainer.Ui;

        isLoaded = false;
    }
}


/// <summary>
/// Class that houses four types of data:
/// * Transient - Short lived data, not meant to be saved on the backend: [username, password, etc]
/// * Persistent - Data that are meant to be saved on the backend: [user avatar id, app settings]
/// * Container - Data that has ids for references: [asset bank]
///             - Data that are generated at runtime: [ui instances, enemy trackers]
/// </summary>
public abstract class AGameData : ScriptableObject {
    abstract public bool IsLoaded { get; }

    /// <summary>
    /// Short lived data, not meant to be saved on the backend: [player username, player password, etc]
    /// </summary>
    abstract public TransientData Transient { get; }

    /// <summary>
    /// Data that are meant to be saved on the backend: [player avatar id, app settings, player coins]
    /// </summary>
    abstract public PersistentData Persistent { get; }

    /// <summary>
    /// Data that has key for references: [asset bank]
    /// May also hold data generated at runtime: [ui instances, enemy trackers]
    /// </summary>
    abstract public ContainerData Container { get; }

    /// <summary>
    /// Data that has keys for use with ContainerData, 
    /// or hard string referecenes: dictionary keys
    /// </summary>
    abstract public KeyData Key { get; }
}


/// <summary>
/// Handles the mono side of GameData.
/// 
/// Handles game data, it is responsible for things like loading data at start, 
/// loading assets, assigning temporary room player custom properties.
/// 
/// Implementation of saving is that:
/// This saves local and backend data when the game loses focus, or the game
/// is quit.
/// </summary>
public class DefaultGameDataMono : MonoBehaviour {
    public event Action OnLoadedData = delegate { };

    [Inject] public AGameData GameData;
    [Inject] public ANetwork Network;
    [Inject] public ABackend Backend;
    [Inject] public AConnectionChecker ConnectionChecker;

    private AvatarData[] loadedAvatars;
    private WorldData[] loadedWorlds;
    private SignInBackgroundData[] loadedSignInBackgrounds;

    private Coroutine delayedQuit;
    private string avatarId;
    private float moveVerticalInput;
    private bool jumpInput;
    private bool emoteInput;


    private IEnumerator LoadData() {
        yield return StartCoroutine(LoadAssets());
        yield return StartCoroutine(LoadLocalData());
        yield return StartCoroutine(LoadBackendData());
        OnLoadedData();
    }

    private IEnumerator LoadAssets() {
        // Load asset resources
        loadedAvatars = _LoadAssetResources<AvatarData>(GameData.Container.Asset.AvatarPaths);
        foreach(var avatar in loadedAvatars) {
            GameData.Container.Asset.Avatars[avatar.Id] = avatar;
        }

        loadedWorlds = _LoadAssetResources<WorldData>(GameData.Container.Asset.WorldPaths);
        foreach(var world in loadedWorlds) {
            GameData.Container.Asset.Worlds[world.Id] = world;
        }

        loadedSignInBackgrounds = _LoadAssetResources<SignInBackgroundData>(GameData.Container.Asset.SignInBackgroundPaths);
        foreach(var signInBackground in loadedSignInBackgrounds) {
            GameData.Container.Asset.SignInBackgrounds.Add(signInBackground);
        }

        // Load Ui
        _LoadUiContainerElements(ref GameData.Container.Ui, null);
        DontDestroyOnLoad(GameData.Container.Ui);

        _LoadUiContainerElements(ref GameData.Container.Ui.Loading, GameData.Container.Ui.OverlayMasterCanvas);
        _LoadUiContainerElements(ref GameData.Container.Ui.SignIn, GameData.Container.Ui.OverlayMasterCanvas);
        _LoadUiContainerElements(ref GameData.Container.Ui.UserOnBoarding, GameData.Container.Ui.OverlayMasterCanvas);
        _LoadUiContainerElements(ref GameData.Container.Ui.Input, GameData.Container.Ui.OverlayMasterCanvas);
        _LoadUiContainerElements(ref GameData.Container.Ui.Interface, GameData.Container.Ui.OverlayMasterCanvas);

        _LoadUiContainerElements(ref GameData.Container.Ui.Debug, GameData.Container.Ui.OverlayNonScaledMasterCanvas);
        _LoadUiContainerElements(ref GameData.Container.Ui.Joystick, GameData.Container.Ui.OverlayNonScaledMasterCanvas);

        // Load avatar prefabs to network module's instantiateable prefabs
        // Don't load already loaded prefabs
        foreach(AvatarData avatar in GameData.Container.Asset.Avatars.Values) {
            if(Network.InstantiateablePrefabs.Contains(avatar.gameObject)) continue;
            Network.InstantiateablePrefabs.Add(avatar.gameObject);
        }
        yield return null;
    }

    private T[] _LoadAssetResources<T>(params string[] paths) where T : UnityEngine.Object {
        List<T> assetsList = new List<T>();
        foreach(var path in paths) {
            var assets = Resources.LoadAll<T>(path);
            if(assets.Length > 0) {
                assetsList.AddRange(assets);
            }
        }
        return assetsList.ToArray();
    }

    private void _LoadUiContainerElements<T>(ref T UiData, Transform parentUi) where T : MonoBehaviour {
        string UiDataName = UiData.name;
        UiData = UnityExtension.ExtensionInstantiate(UiData, (uiData) => {
            uiData.name = UiDataName;

            if(parentUi != null) {
                RectTransform rt = uiData.GetComponent<RectTransform>();
                rt.SetParent(parentUi);

                // Doing this offset adjustment because apparently ui will not
                // be in right position if this is not done
                rt.offsetMax = new Vector2(0f, 0f);
                rt.offsetMin = new Vector2(0f, 0f);
            }
        });
    }

    private IEnumerator LoadLocalData() {
        Backend.LoadLocalData("App",
                              typeof(AppPersistentData),
                              (data) => {
                                  GameData.Persistent.App = (AppPersistentData)data;
                              },
                              () => {
                                  GameData.Persistent.App = new AppPersistentData();
                              });
        yield return null;
    }

    private IEnumerator LoadBackendData() {
        yield return new WaitUntil(() => ConnectionChecker.IsInternetOk);
        yield return new WaitUntil(() => Backend.IsAvailable);
        yield return new WaitUntil(() => Backend.IsAuthenticated);

        // Load player data
        Backend.LoadCloudData("Player",
                              typeof(PlayerPersistentData),
                              (data) => {
                                  if(data == null) {
                                      GameData.Persistent.Player = new PlayerPersistentData();
                                  } else {
                                      GameData.Persistent.Player = (PlayerPersistentData)data;
                                  }
                              },
                              () => {
                                  GameData.Persistent.Player = new PlayerPersistentData();
                              });

        yield return new WaitWhile(() => Backend.IsLoadingCloudData);
    }

    private IEnumerator UpdateLocalData() {
        Backend.SaveLocalData("App", GameData.Persistent.App, null, null);
        yield return null;
    }

    private IEnumerator UpdateBackendData() {
        yield return new WaitUntil(() => ConnectionChecker.IsInternetOk);
        yield return new WaitUntil(() => Backend.IsAvailable);
        yield return new WaitUntil(() => Backend.IsAuthenticated);

        Backend.SaveCloudData("Player", GameData.Persistent.Player, null, null);
    }

    private void UpdatePlayerCustomProperties() {
        // Player custom properties are for Photon syncing
        // We need this to sync things like avatar changes 
        // through the network
        var properties = new List<DictionaryEntry>();

        var avatarIdProp = _UpdatePlayerCustomProperties(GameData.Key.Player.CustomPropertyAvatarIdKey, ref avatarId, GameData.Persistent.Player.AvatarId);
        if(!avatarIdProp.Equals(default(DictionaryEntry))) {
            properties.Add(avatarIdProp);
            Debug.Log("AvatarPropId Update");
        }

        var moveVerticalInputProp = _UpdatePlayerCustomProperties(GameData.Key.Player.CustomPropertyMoveVerticalInputKey, ref moveVerticalInput, GameData.Transient.Player.MoveVerticalInput);
        if(!moveVerticalInputProp.Equals(default(DictionaryEntry))) properties.Add(moveVerticalInputProp);

        var jumpInputProp = _UpdatePlayerCustomProperties(GameData.Key.Player.CustomPropertyJumpInputKey, ref jumpInput, GameData.Transient.Player.JumpInput);
        if(!jumpInputProp.Equals(default(DictionaryEntry))) properties.Add(jumpInputProp);

        var emoteInputProp = _UpdatePlayerCustomProperties(GameData.Key.Player.CustomPropertyEmoteInputKey, ref emoteInput, GameData.Transient.Player.EmoteInput);
        if(!emoteInputProp.Equals(default(DictionaryEntry))) properties.Add(emoteInputProp);

        if(properties.Count > 0) {
            Network.SetPlayerCustomProperties(properties.ToArray());
        }
    }

    private DictionaryEntry _UpdatePlayerCustomProperties<T>(string propertyId, ref T data, T newData) {
        if(data == null) {
            data = newData;
            return new DictionaryEntry(propertyId, data);
        }

        if(!data.Equals(newData)) {
            data = newData;
            return new DictionaryEntry(propertyId, data);
        }

        return default(DictionaryEntry);
    }

    private void UnloadAssets() {
        // Making sure things are reset when the play mode is done
        // so the loaded avatar data doesn't stack up.
        // In a standalone game you really don't need to do this.
        // You just do this for the editor.
        // Also done as a defensive programming approach
        foreach(var avatar in GameData.Container.Asset.Avatars.Values) {
            Network.InstantiateablePrefabs.Remove(avatar.gameObject);
        }

        // Unload assets
        foreach(var avatar in loadedAvatars) {
            GameData.Container.Asset.Avatars.Remove(avatar.Id);
        }
        
        foreach(var world in loadedWorlds) {
            GameData.Container.Asset.Worlds.Remove(world.Id);
        }

        foreach(var signInBackground in loadedSignInBackgrounds) {
            GameData.Container.Asset.SignInBackgrounds.Remove(signInBackground);
        }
    }

    #region Network Events
    private void OnJoinRoom() {
        // We do temporary player custom properties here such as animation values,
        // kill scores, etc.
        GameData.Transient.Player.MoveVerticalInput = 0f;
        GameData.Transient.Player.EmoteInput = false;
        GameData.Transient.Player.JumpInput = false;
    }

    private void OnLeaveRoom() {
        // We always reset the temporary player custom properties here so that it
        // doesn't sync when going to another room
        GameData.Transient.Player.MoveVerticalInput = 0f;
        GameData.Transient.Player.EmoteInput = false;
        GameData.Transient.Player.JumpInput = false;
    }
    #endregion

    public void Initialize() {
        // Set initial player custom properties so we avoid null references
        Network.SetPlayerCustomProperties(new DictionaryEntry[] {
            new DictionaryEntry(GameData.Key.Player.CustomPropertyAvatarIdKey, ""),
            new DictionaryEntry(GameData.Key.Player.CustomPropertyMoveHorizontalInputKey, 0f),
            new DictionaryEntry(GameData.Key.Player.CustomPropertyMoveVerticalInputKey, 0f),
            new DictionaryEntry(GameData.Key.Player.CustomPropertyJumpInputKey, false),
            new DictionaryEntry(GameData.Key.Player.CustomPropertyEmoteInputKey, false)
        });

        // Load data
        StartCoroutine(LoadData()); 

        // Network callbacks
        Network.OnJoinRoom += OnJoinRoom;
        Network.OnLeaveRoom += OnLeaveRoom;
    }

    private void Update() {
        // Update player custom properties only if we're connected to the network
        if(Network.IsConnected) {
            UpdatePlayerCustomProperties();
        }
    }

    // We exchanged OnApplicationFocus for OnApplicationPause because apparently
    // OnApplicationFocus is called say when the keyboard is brought up by the user,
    // but not when say the "Home" button is pressed. OnApplicationPause is the
    // one responsible for that.
    //private void OnApplicationFocus(bool focus) {
    //    if(!focus) {
    //        // Save data
    //        StartCoroutine(UpdateLocalData());

    //        // Only update the backend if these
    //        // service connections are met
    //        if(Backend.IsAvailable &&
    //           Backend.IsAuthenticated) {
    //            StartCoroutine(UpdateBackendData());
    //        }
    //    }
    //}

    private void OnApplicationPause(bool pause) {
        if(!pause) {
            // Save data
            StartCoroutine(UpdateLocalData());

            // Only update the backend if these
            // service connections are met
            if(Backend.IsAvailable &&
               Backend.IsAuthenticated) {
                StartCoroutine(UpdateBackendData());
            }
        }
    }

    private void OnApplicationQuit() {
        if(delayedQuit == null) {
            delayedQuit = StartCoroutine(DelayedQuit());
        }
    }

    private IEnumerator DelayedQuit() {
        // Delay the quit so we can save some data first
        Application.CancelQuit();

        // Save data
        yield return StartCoroutine(UpdateLocalData());

        if(Backend.IsAvailable &&
           Backend.IsAuthenticated) {
            yield return StartCoroutine(UpdateBackendData());
        }

        // Unload assets, this is mainly for editor use
        UnloadAssets();

        yield return new WaitForEndOfFrame();
        Debug.Log("Quitting application");
        Application.Quit();
    }
}