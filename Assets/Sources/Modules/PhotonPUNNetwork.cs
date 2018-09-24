using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEngine;
using Photon;


/// <summary>
/// Photon integration for DefaultNetwork module.
/// </summary>
[CreateAssetMenu(fileName = "PhotonPUNNetwork", menuName = "WEngine/Modules/ANetwork/PhotonPUNNetwork")]
public class PhotonPUNNetwork : ANetwork, IInitializable {
    public int Priority { get { return 1000; } }

    public override bool IsConnected { get { return isConnected = PhotonNetwork.connected; } }
    public override bool IsConnectedAndReady { get { return isConnectedAndReady = PhotonNetwork.connectedAndReady; } }
    public override bool IsInRoom { get { return isInRoom = PhotonNetwork.inRoom; } }
    public override bool IsInLobby { get { return isInLobby = PhotonNetwork.insideLobby; } }
    public override List<GameObject> InstantiateablePrefabs { get { return instantiateablePrefabs; } }
    public override List<FriendInfo> Friends { get { return friends = PhotonNetwork.Friends; } }
    public override ExitGames.Client.Photon.Hashtable PlayerCustomProperties { get { return playerCustomProperties = PhotonNetwork.player.CustomProperties; } }
    public override string RoomName { get { return roomName = PhotonNetwork.inRoom ? PhotonNetwork.room.Name : ""; } }

    [Header("Data")]
#pragma warning disable 0414
    [SerializeField] private string userId;
    [SerializeField] private string gameVersion;
#pragma warning restore 0414
    [SerializeField] private List<GameObject> instantiateablePrefabs;

    [Header("Status")]
#pragma warning disable 0414
    [SerializeField] private bool isConnected;
    [SerializeField] private bool isConnectedAndReady;
    [SerializeField] private bool isInRoom;
    [SerializeField] private bool isInLobby;
    [SerializeField] private string roomName;

    private List<FriendInfo> friends = new List<FriendInfo>();
    private ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
#pragma warning restore 0414

    [Header("References")]
    [Inject] public AGameData GameData;

    private Coroutine createRoomRoutine;
    private Coroutine joinOrCreateRoomRoutine;
    private Coroutine joinUserRoomRoutine;
    private PhotonPUNNetworkMono photonPUNNetworkMono;


    public override void SetUserId(string userId) {
        PhotonNetwork.playerName = this.userId = userId;
    }

    public override void SetGameVersion(string gameVersion) {
        PhotonNetwork.gameVersion = this.gameVersion = gameVersion;
    }

    public override void SetPlayerCustomProperties(DictionaryEntry[] customProperties) {
        ExitGames.Client.Photon.Hashtable hashTable = customProperties != null ? new ExitGames.Client.Photon.Hashtable() : null;
        if(customProperties != null) {
            foreach(var entry in customProperties) {
                hashTable[entry.Key] = entry.Value;
            }
        }
        
        PhotonNetwork.SetPlayerCustomProperties(hashTable);
    }

    public override void ConnectToServer() {
        if(!PhotonNetwork.connected) {
            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.AuthValues = new AuthenticationValues(PhotonNetwork.playerName);
            PhotonNetwork.ConnectUsingSettings(PhotonNetwork.gameVersion);
            Debug.Log("Connected to the server: " + userId + " | " + gameVersion);
        }
    }

    public override void DisconnectToServer() {
        PhotonNetwork.Disconnect();
    }

    public override void JoinLobby(TypedLobby typedLobby = null) {
        if(typedLobby != null) PhotonNetwork.JoinLobby(typedLobby);
        else PhotonNetwork.JoinLobby();
    }

    public override void LeaveLobby() {
        PhotonNetwork.LeaveLobby();
    }

    public override RoomInfo[] GetRoomList(DictionaryEntry[] expectedRoomProperties) {
        ExitGames.Client.Photon.Hashtable hashTable = expectedRoomProperties != null ? new ExitGames.Client.Photon.Hashtable() : null;
        if(expectedRoomProperties != null) {
            foreach(var entry in expectedRoomProperties) {
                hashTable[entry.Key] = entry.Value;
            }
        }

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        
        if(hashTable != null) {
            rooms = rooms.Where(r => {
                bool isEqual = false;
                // Compare if the expected room properties match a
                // room's properties
                if(hashTable.Count == r.CustomProperties.Count) {
                    isEqual = true;
                    foreach(var entry in hashTable) {
                        object value = null;
                        if(r.CustomProperties.TryGetValue(entry.Key, out value)) {
                            // Using .Equals() here because our value is boxed, using the == or !=
                            // operator will result to reference equality comparison which
                            // is not what we want
                            if(!entry.Value.Equals(value)) {
                                isEqual = false;
                                break;
                            }
                        } else {
                            isEqual = false;
                            break;
                        }
                    }
                }
                return isEqual;
            }).ToArray();
        }

        return rooms;
    }

    public override void CreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null) {
        // Prevents from running multiple create room calls
        if(createRoomRoutine != null) return;

        ExitGames.Client.Photon.Hashtable hashTable = customRoomProperties != null ? new ExitGames.Client.Photon.Hashtable() : null;
        if(customRoomProperties != null) {
            foreach(var entry in customRoomProperties) {
                hashTable[entry.Key] = entry.Value;
            }
        }

        createRoomRoutine = photonPUNNetworkMono.StartCoroutine(_LeaveRoom(() => {
            // If there's no custom room properties for lobby, then
            // include all instead
            if(hashTable != null && customRoomPropertiesForLobby == null) {
                customRoomPropertiesForLobby = hashTable.Keys.Cast<string>().ToArray();
            }

            // Load default options if nothing is provided
            if(options == null) {
                options = new RoomOptions() {
                    MaxPlayers = 5,
                    IsVisible = true,
                    IsOpen = true,
                    CleanupCacheOnLeave = true,
                    PublishUserId = true,

                    PlayerTtl = 5000,
                    CustomRoomProperties = hashTable,
                    CustomRoomPropertiesForLobby = customRoomPropertiesForLobby
                };
            }

            // We change scenes when joining a room
            ASceneLoader SceneLoader = Main.GetGameModule<ASceneLoader>();
            SceneLoader.LoadSceneAsync(sceneName,
                LoadSceneMode.Single,
                null,
                () => {
                    PhotonNetwork.JoinOrCreateRoom(roomName, options, typedLobby, expectedUsers);
                    createRoomRoutine = null;
                });
        }));
    }

    public override void JoinOrCreateRoom(string roomName, string sceneName, RoomOptions options = null, TypedLobby typedLobby = null, string[] expectedUsers = null, DictionaryEntry[] customRoomProperties = null, string[] customRoomPropertiesForLobby = null) {
        // Prevents from running multiple join or create room calls
        if(joinOrCreateRoomRoutine != null) return;

        joinOrCreateRoomRoutine = photonPUNNetworkMono.StartCoroutine(_LeaveRoom(() => {
            // Check if a room with a room name exists
            RoomInfo room = GetRoomList(null).Where(r => r.Name == roomName).FirstOrDefault();

            if(room == null) Debug.Log("No room with name");

            // If there's no room with a name like roomname exists
            // then check if there's a room with the customRoomProperties
            // exists
            if(room == null) {
                RoomInfo[] rooms = GetRoomList(customRoomProperties);
                room = rooms.Length > 0 ? rooms[UnityEngine.Random.Range(0, rooms.Length)] : null;
            }

            if(room == null) Debug.Log("No room with same properties.");

            // If there's a room that exists
            if(room != null) {
                roomName = room.Name;
                sceneName = (string)room.CustomProperties[GameData.Key.Room.CustomPropertySceneNameKey];
                Debug.Log("Found a room!");
            }

            CreateRoom(roomName, sceneName, options, typedLobby, expectedUsers, customRoomProperties, customRoomPropertiesForLobby);
            joinOrCreateRoomRoutine = null;
        }));
    }

    public override void JoinUserRoom(string userName) {
        // Prevents from running multiple join user room calls
        if(joinUserRoomRoutine != null) return;

        joinUserRoomRoutine = photonPUNNetworkMono.StartCoroutine(_LeaveRoom(() => {
            photonPUNNetworkMono.StartCoroutine(_JoinUserRoom(userName));
        }));
    }

    public override void LeaveRoom(bool becomeInactive = true) {
        PhotonNetwork.LeaveRoom(becomeInactive);
    }

    public override void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, byte group, object[] data) {
        // Check if not in room
        if(!IsInRoom) {
            Debug.LogError("Can't use Photon.Instantiate outside of rooms!");
            return;
        }

        // Check if in spawnable prefabs
        GameObject instantiateablePrefab = InstantiateablePrefabs.FirstOrDefault(p => p.name == prefab.name);

        if(!instantiateablePrefab) {
            Debug.LogError("There is no spawnable prefab of name: " + prefab.name);
            return;
        }

        // Call RPC
        int id = PhotonNetwork.AllocateViewID();
        PhotonView view = photonPUNNetworkMono.GetComponent<PhotonView>();
        view.RPC("RPC_InstantiateOnNetwork", PhotonTargets.AllBuffered, prefab.name, position, rotation, id);
    }

    public override void Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data) {
        // Check if not in room
        if(!IsInRoom) {
            Debug.LogError("Can't use Photon.Instantiate outside of rooms!");
            return;
        }

        // Instantiate
        PhotonNetwork.Instantiate(prefabName, position, rotation, group, data);
    }

    #region Wait methods
    private IEnumerator _JoinUserRoom(string userName) {
        if(PhotonNetwork.FindFriends(new string[] { userName })) {
            yield return new WaitUntil(() => PhotonNetwork.Friends != null);
            FriendInfo friend = PhotonNetwork.Friends.FirstOrDefault(f => f.UserId == userName);
            if(friend != null) {
                if(friend.IsInRoom) {
                    Debug.Log("Found friend, Joining!: " + friend.Room);
                    JoinOrCreateRoom(friend.Room, "");
                    yield break;
                } else {
                    Debug.Log("Found friend, but is not in room: " + friend.Room);
                }
            } else {
                Debug.Log("Friend not found.");
            }
        }

        // Join user's homespace if friend is non-existent
        this.CreateRoomBySceneName(GameData.Key.Scene.LandingRoom, true);
        joinUserRoomRoutine = null;
    }
    
    private IEnumerator _LeaveRoom(Action onLeftRoom) {
        if(IsInRoom) LeaveRoom(false);
        yield return new WaitWhile(() => IsInRoom);
        yield return new WaitWhile(() => !IsInLobby);
        if(onLeftRoom != null) onLeftRoom();
    }
    #endregion

    public void Initialize() {
        // Spawn the mono
        photonPUNNetworkMono = new GameObject("PhotonPUNNetworkMono").ExtensionAddComponent<PhotonPUNNetworkMono>();
        DontDestroyOnLoad(photonPUNNetworkMono.gameObject);
        photonPUNNetworkMono.Initialize();

        // Register events
        photonPUNNetworkMono.PhotonOnConnectedToPhoton += _OnConnected;
        photonPUNNetworkMono.PhotonOnDisconnectedFromPhoton += _OnDisconnected;
        photonPUNNetworkMono.PhotonOnJoinedLobby += _OnJoinLobby;
        photonPUNNetworkMono.PhotonOnLeftLobby += _OnLeaveLobby;
        photonPUNNetworkMono.PhotonOnCreatedRoom += _OnCreateRoom;
        photonPUNNetworkMono.PhotonOnJoinRoom += _OnJoinRoom;
        photonPUNNetworkMono.PhotonOnJoinRoomFailed += _OnJoinRoomFailed;
        photonPUNNetworkMono.PhotonOnLeftRoom += _OnLeaveRoom;
        photonPUNNetworkMono.PhotonOnSetPlayerCustomProperties += _OnSetPlayerCustomProperties;
        photonPUNNetworkMono.PhotonOnUpdatedFriendList += _OnUpdatedFriendList;
    }
}


/// <summary>
/// Handles PUN callbacks
/// </summary>
public class PhotonPUNNetworkMono : PunBehaviour {
    public event Action PhotonOnConnectedToPhoton = delegate { };
    public event Action PhotonOnDisconnectedFromPhoton = delegate { };
    public event Action PhotonOnJoinedLobby = delegate { };
    public event Action PhotonOnLeftLobby = delegate { };
    public event Action PhotonOnCreatedRoom = delegate { };
    public event Action PhotonOnJoinRoom = delegate { };
    public event Action PhotonOnJoinRoomFailed = delegate { };
    public event Action PhotonOnLeftRoom = delegate { };
    public event Action PhotonOnUpdatedFriendList = delegate { };
    public event Action<object, object> PhotonOnSetPlayerCustomProperties = delegate { };

    // Custom events
    public event Action PhotonOnReconnectAndRejoinFailed = delegate { };

    [Inject] public ANetwork Network;
    [Inject] public AGameData GameData;

    private bool isTryRejoin;

    private PhotonView thisPhotonView;


    public override void OnConnectedToPhoton() {
        PhotonOnConnectedToPhoton();
        Debug.Log("PhotonNetwork: OnConnectedToPhoton");
    }

    public override void OnConnectionFail(DisconnectCause cause) {
        Debug.Log("PhotonNetwork: OnConnectionFail: " + cause.ToString());

        switch(cause) {
            case DisconnectCause.DisconnectByClientTimeout:
                isTryRejoin = true;
                break;
            case DisconnectCause.DisconnectByServerTimeout:
                isTryRejoin = true;
                break;
        }
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause) {
        Debug.Log("PhotonNetwork: OnFailedToConnectToPhoton - " + cause.ToString());
    }

    public override void OnConnectedToMaster() {
        Debug.Log("PhotonNetwork: OnConnectedToMaster");
    }

    public override void OnDisconnectedFromPhoton() {
        PhotonOnDisconnectedFromPhoton();
        Debug.Log("PhotonNetwork: OnDisconnectedFromPhoton");

        // Try to rejoin if we got disconnected from a timeout
        if(isTryRejoin) {
            if(!PhotonNetwork.ReconnectAndRejoin()) {
                PhotonOnReconnectAndRejoinFailed();
            }

            isTryRejoin = false;
        }
    }

    public override void OnJoinedLobby() {
        PhotonOnJoinedLobby();
        Debug.Log("PhotonNetwork: OnJoinedLobby");
    }

    public override void OnLeftLobby() {
        PhotonOnLeftLobby();
        Debug.Log("PhotonNetwork: OnLeftLobby");
    }

    public override void OnCreatedRoom() {
        PhotonOnCreatedRoom();
        Debug.Log("PhotonNetwork: OnCreatedRoom");
    }

    public override void OnJoinedRoom() {
        // Instead of assigning an AllocateSceneViewId() or AllocateViewId(),
        // we instead asign a static view id so that it is synced across builds.
        // It will not work with the Allocate() methods because they will 
        // always generate non-equal viewIds. SceneViewId() will not work because
        // MasterClient is the only one who can do it, and ViewId() will just generate
        // local ids which is not what we wanted
        if(thisPhotonView == null) {
            thisPhotonView = gameObject.AddComponent<PhotonView>();
            thisPhotonView.viewID = 999;
        }

        PhotonOnJoinRoom();
        Debug.Log("PhotonNetwork: OnJoinedRoom");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        PhotonOnJoinRoomFailed();
        Debug.Log("PhotonNetwork: OnJoinedRoomFailed");
    }

    public override void OnLeftRoom() {
        // We do it in a coroutine as it is perfect for code that
        // needs to wait
        StartCoroutine(CleanupPhotonView());

        PhotonOnLeftRoom();
        Debug.Log("PhotonNetwork: OnLeftRoom");
    }

    public override void OnUpdatedFriendList() {
        PhotonOnUpdatedFriendList();
    }

    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
        PhotonOnSetPlayerCustomProperties(playerAndUpdatedProps[0], playerAndUpdatedProps[1]);
    }

    private IEnumerator CleanupPhotonView() {
        int viewId = thisPhotonView.viewID;
        Destroy(photonView);
        
        // Waits one frame, we do this because Unity's Destroy doesn't
        // destroy and removes references right-away. Also we do this
        // because Photons needs the PV destroyed first before letting you
        // unallocate a viewId
        yield return null;

        PhotonNetwork.UnAllocateViewID(viewId);
    }

    internal void Initialize() {
        // Instead of connecting to Photon on start of the game
        // We connect when the player signs-in. This way we can
        // properly set the userId so that users can join other 
        // users in room.

        // Check now the ConnectToNetworkSystem.cs for the new
        // implementation

        // Register handlers
        //Network.SetUserId(GameData.Transient.Player.UserName);
        //Network.SetGameVersion(GameData.Transient.App.AppVersion);

        //Network.ConnectToServer();
    }

    [PunRPC]
    private void RPC_InstantiateOnNetwork(string prefabName, Vector3 pos, Quaternion rot, int viewId) {
        Debug.Log("Instantiating " + prefabName + " for: " + viewId);

        // Instantiate object
        GameObject go = UnityExtension.ExtensionInstantiate(Network.InstantiateablePrefabs.FirstOrDefault(p => p.name == prefabName), (GameObject goInstance) => {
            // Change name
            goInstance.name = prefabName;
            
            // Change position
            Transform t = goInstance.GetComponent<Transform>();
            t.position = pos;
            t.rotation = rot;
        });

        Debug.Log("Successfully instantiated " + prefabName + " for: " + viewId);

        // Set Photon view id if has one
        PhotonView view = go.GetComponent<PhotonView>();
        if(view) view.viewID = viewId;

        // Attach a PUNMonitor
        go.ExtensionAddComponent<PhotonPUNMonitor>();

        Debug.Log("Successfully assigned id" + prefabName + " for: " + viewId);
    }
}


/// <summary>
/// Monitors networked objects and gives them callbacks that gets invoked
/// </summary>
public class PhotonPUNMonitor : PunBehaviour {
    private PhotonView thisPhotonView;


    private void Awake() {
        thisPhotonView = GetComponent<PhotonView>();
    }

    private void OnDestroy() {
        PhotonNetwork.UnAllocateViewID(thisPhotonView.viewID);
        Debug.Log("Unallocated view ID: " + thisPhotonView.viewID);
    }
}