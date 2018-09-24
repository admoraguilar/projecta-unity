using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEngine;


/// <summary>
/// This module handles the network connectivity of the game. Useful for
/// integrating services like UNET or Photon.
/// </summary>
[CreateAssetMenu(fileName = "DefaultNetwork", menuName = "WEngine/Modules/ANetwork/DefaultNetwork")]
public class DefaultNetwork : ANetwork, IInitializable {
    public int Priority { get { return 1000; } }

    public override bool IsConnected { get { return isConnected; } }
    public override bool IsConnectedAndReady { get { return isConnectedAndReady; } }
    public override bool IsInRoom { get { return isInRoom; } }
    public override bool IsInLobby { get { return isInLobby; } }
    public override List<GameObject> InstantiateablePrefabs { get { return instantiateablePrefabs; } }
    public override List<FriendInfo> Friends { get { return friends; } }
    public override ExitGames.Client.Photon.Hashtable PlayerCustomProperties { get { return playerCustomProperties; } }
    public override string RoomName { get { return roomName; } }

    [Header("Data")]
    [SerializeField] private List<GameObject> instantiateablePrefabs;

    [Header("Status")]
    [SerializeField] private bool isConnected;
    [SerializeField] private bool isConnectedAndReady;
    [SerializeField] private bool isInRoom;
    [SerializeField] private bool isInLobby;
    [SerializeField] private string roomName;
    [SerializeField] private string userId;
    [SerializeField] private string gameVersion;

    private List<FriendInfo> friends = new List<FriendInfo>();
    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();


    public override void SetUserId(string userId) {
        this.userId = userId;
    }

    public override void SetGameVersion(string gameVersion) {
        this.gameVersion = gameVersion;
    }

    public override void SetPlayerCustomProperties(DictionaryEntry[] customProperties) {
        foreach(var entry in customProperties) {
            playerCustomProperties[entry.Key] = entry.Value;
        }
        _OnSetPlayerCustomProperties(null, playerCustomProperties);
    }

    public override void ConnectToServer() {
        isConnected = true;
        isConnectedAndReady = true;
        if(isConnected) {
            Debug.Log("[DefaultNetwork] Connected to the server: " + userId + " | " + gameVersion);
            _OnConnected();
        }
    }

    public override void DisconnectToServer() {
        isConnected = false;
        isConnectedAndReady = false;
        if(!isConnected) _OnDisconnected();
    }

    public override void JoinLobby(TypedLobby typedLobby) {
        isInLobby = true;
        isInRoom = false;
        if(isInLobby) _OnJoinLobby();
    }

    public override void LeaveLobby() {
        isInLobby = false;
        if(!isInLobby) _OnLeaveLobby();
    }

    public override RoomInfo[] GetRoomList(DictionaryEntry[] expectedRoomProperties) {
        return null;
    }

    public override void CreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null) {
        ASceneLoader SceneLoader = Main.GetGameModule<ASceneLoader>();

        // We change scenes when joining a room
        SceneLoader.LoadSceneAsync(sceneName,
            LoadSceneMode.Single,
            null,
            () => {
                isInRoom = true;
                isInLobby = false;
                if(isInRoom) {
                    _OnCreateRoom();
                    _OnJoinRoom();
                }
                this.roomName = roomName;
            });
    }

    public override void JoinOrCreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null) {
        CreateRoom(roomName, sceneName, options, typedLobby, expectedUsers, customRoomProperties, customRoomPropertiesForLobby);
    }

    public override void JoinUserRoom(string userName) {
        CreateRoom(userName, "", null, null, null, null, null);
    }

    public override void LeaveRoom(bool becomeInactive = true) {
        JoinLobby(null);
        if(!isInRoom) _OnLeaveRoom();
        this.roomName = "";
    }

    public override void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, byte group, object[] data) {
        GameObject instantiateablePrefab = InstantiateablePrefabs.FirstOrDefault(p => p.name == prefab.name);

        if(!instantiateablePrefab) {
            Debug.LogError("There is no spawnable prefab of name: " + prefab.name);
            return;
        }

        UnityExtension.ExtensionInstantiate(prefab, (GameObject goInstance) => {
            // Remove (clone) in name
            goInstance.name = prefab.name;

            // Change position
            Transform t = goInstance.GetComponent<Transform>();
            t.position = position;
            t.rotation = rotation;
        });
    }

    public override void Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data) {
        GameObject go = Resources.Load<GameObject>(prefabName);
        Instantiate(go, position, rotation, group, data);
    }

    public void Initialize() {
        //ConnectToServer();
    }
}


public abstract class ANetwork : ScriptableObject {
    public event Action OnConnected = delegate { };
    public event Action OnDisconnected = delegate { };
    public event Action OnJoinLobby = delegate { };
    public event Action OnLeaveLobby = delegate { };
    public event Action OnCreateRoom = delegate { };
    public event Action OnJoinRoom = delegate { };
    public event Action OnJoinRoomFailed = delegate { };
    public event Action OnLeaveRoom = delegate { };
    public event Action OnUpdatedFriendList = delegate { };
    public event Action<object, object> OnSetPlayerCustomProperties = delegate { };
    
    protected void _OnConnected() { OnConnected(); }
    protected void _OnDisconnected() { OnDisconnected(); }
    protected void _OnJoinLobby() { OnJoinLobby(); }
    protected void _OnLeaveLobby() { OnLeaveLobby(); }
    protected void _OnCreateRoom() { OnCreateRoom(); }
    protected void _OnJoinRoom() { OnJoinRoom(); }
    protected void _OnJoinRoomFailed() { OnJoinRoomFailed(); }
    protected void _OnLeaveRoom() { OnLeaveRoom(); }
    protected void _OnUpdatedFriendList() { OnUpdatedFriendList(); }
    protected void _OnSetPlayerCustomProperties(object player, object changedProperties) { OnSetPlayerCustomProperties(player, changedProperties); }

    abstract public bool IsConnected { get;}
    abstract public bool IsConnectedAndReady { get; }
    abstract public bool IsInRoom { get; }
    abstract public bool IsInLobby { get; }
    abstract public List<GameObject> InstantiateablePrefabs { get; }
    abstract public List<FriendInfo> Friends { get; }
    abstract public ExitGames.Client.Photon.Hashtable PlayerCustomProperties { get; }
    abstract public string RoomName { get; }

    abstract public void SetUserId(string userId);
    abstract public void SetGameVersion(string gameVersion);
    abstract public void SetPlayerCustomProperties(DictionaryEntry[] customProperties);
    abstract public void ConnectToServer();
    abstract public void DisconnectToServer();
    abstract public void JoinLobby(TypedLobby typedLobby);
    abstract public void LeaveLobby();
    abstract public RoomInfo[] GetRoomList(DictionaryEntry[] expectedRoomProperties);
    abstract public void CreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null);
    abstract public void JoinOrCreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null);
    abstract public void JoinUserRoom(string userName);
    abstract public void LeaveRoom(bool becomeInactive = true);
    abstract public void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, byte group, object[] data);
    abstract public void Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data);
}


/// <summary>
/// Extension class for network: Room creation templates, etc.
/// </summary>
public static class NetworkExtensions {
    public static void CreateRoomBySceneName(this ANetwork network, string sceneName, bool isPrivate) {
        AGameData GameData = Main.GetGameModule<AGameData>();
        ATime Time = Main.GetGameModule<ATime>();

        network.CreateRoom(sceneName + Time.GetNumberedUtcNow().ToString(),
                           sceneName,
                           null, null, null,
                           new DictionaryEntry[] {
                               new DictionaryEntry(GameData.Key.Room.CustomPropertySceneNameKey, sceneName),
                               new DictionaryEntry(GameData.Key.Room.CustomPropertyIsWorldPrivateKey, isPrivate)
                           },
                           null);
    }

    public static void JoinOrCreateRoomBySceneName(this ANetwork network, string sceneName) {
        AGameData GameData = Main.GetGameModule<AGameData>();
        ATime Time = Main.GetGameModule<ATime>();

        network.JoinOrCreateRoom(sceneName + Time.GetNumberedUtcNow().ToString(),
                                 sceneName,
                                 null, null, null,
                                 new DictionaryEntry[] {
                                     new DictionaryEntry(GameData.Key.Room.CustomPropertySceneNameKey, sceneName),
                                     new DictionaryEntry(GameData.Key.Room.CustomPropertyIsWorldPrivateKey, false)
                                 },
                                 null);
    }
}