using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameSparks.Core;
using WEngine;


/// <summary>
/// <para>This module handles connection to a backend service for saving and loading of cloud data.</para>
/// <para>It also handles saving and loading of some local data such as AppData.</para>
/// 
/// TODO:
/// - Cloud data is only for saving player persistent data, make it modular that you can save other types too.
/// </summary>
[CreateAssetMenu(fileName = "GamesparksBackend", menuName = "WEngine/Modules/ABackend/GamesparksBackend")]
public class GamesparksBackend : ABackend, IInitializable, IDeinitializable {
    public int Priority { get { return 1000; } }

    public override bool IsAvailable { get { return isAvailable = GS.Available; } }
    public override bool IsRegistered { get { return isRegistered = GS.Authenticated; } }
    public override bool IsAuthenticated { get { return isAuthenticated = GS.Authenticated; } }
    public override bool IsRegistering { get { return isRegistering; } }
    public override bool IsAuthenticating { get { return isAuthenticating; } }
    public override bool IsLoadingCloudData { get { return isLoadingCloudData; } }
    public override bool IsSavingCloudData { get { return isSavingCloudData; } }
    public override bool IsLoadingLocalData { get { return isLoadingLocalData; } }
    public override bool IsSavingLocalData { get { return isSavingLocalData; } }

    [Inject] public AGameData GameData;

    [Header("Status")]
#pragma warning disable 0414
    [SerializeField] private bool isAvailable = true;
    [SerializeField] private bool isRegistered;
    [SerializeField] private bool isAuthenticated;
    [SerializeField] private bool isRegistering;
    [SerializeField] private bool isAuthenticating;
    [SerializeField] private bool isLoadingCloudData;
    [SerializeField] private bool isSavingCloudData;
    [SerializeField] private bool isLoadingLocalData;
    [SerializeField] private bool isSavingLocalData;
#pragma warning restore 0414

    [Header("Debug")]
#pragma warning disable 0414
    [SerializeField] private Dictionary<string, string> savedCloudData = new Dictionary<string, string>();
    [SerializeField] private Dictionary<string, string> savedLocalData = new Dictionary<string, string>();
#pragma warning restore 0414


    public override void Register() {
        isRegistering = true;
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetUserName(GameData.Transient.Player.UserName)
            .SetPassword(GameData.Transient.Player.Password)
            .SetDisplayName(GameData.Transient.Player.DisplayName)
            .Send(
            // Success response
            (GameSparks.Api.Responses.RegistrationResponse response) => {
                Debug.Log("Player successfully registered: " + response.UserId);
                GameData.Transient.Player.Password = "********";
                isRegistering = false;
            },
            // Error response
            (GameSparks.Api.Responses.RegistrationResponse response) => {
                Debug.LogWarning("Error registering player: " + response.UserId + " | Error Code: " + response.Errors.GetString("USERNAME"));
                isRegistering = false;
            });
    }

    public override void Authenticate() {
        isAuthenticating = true;
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(GameData.Transient.Player.UserName)
            .SetPassword(GameData.Transient.Player.Password)
            .Send(
            // Success response
            (GameSparks.Api.Responses.AuthenticationResponse response) => {
                Debug.Log("Player successfully authenticated: " + response.UserId);
                GameData.Transient.Player.DisplayName = response.DisplayName;
                GameData.Transient.Player.Password = "********";
                isAuthenticating = false;
            },
            // Error response
            (GameSparks.Api.Responses.AuthenticationResponse response) => {
                Debug.LogWarning("Error authenticating player: " + response.UserId + " | Error Code: " + response.Errors.GetString("DETAILS"));
                isAuthenticating = false;
            });
    }

    public override void LoadCloudData(string key, Type type, Action<object> onSuccess, Action onFail) {
        isLoadingCloudData = true;
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("LoadJSONPlayerData")
            .Send(
            // Success response
            (GameSparks.Api.Responses.LogEventResponse response) => {
                if(response.ScriptData.GetGSData("Data") == null) {
                    onSuccess(null);
                    Debug.Log("Cloud data successfully loaded but NULL");
                    isLoadingCloudData = false;
                } else {
                    if(onSuccess != null) onSuccess(JsonUtility.FromJson(response
                                                    .ScriptData
                                                    .GetGSData("Data")
                                                    .GetGSData(key)
                                                    .JSON, type));
                    Debug.Log("Cloud data successfully loaded: " + response.ScriptData.GetGSData("Data").GetGSData(key).JSON);
                    isLoadingCloudData = false;
                }

                Debug.Log("Success");
            },
            // Error response
            (GameSparks.Api.Responses.LogEventResponse response) => {
                if(onFail != null) onFail();
                Debug.LogWarning("Error loading cloud data: " + response.Errors.JSON);
                isLoadingCloudData = false;

                Debug.Log("Fail");
            });
    }

    public override void SaveCloudData(string key, object data, Action onSuccess, Action onFail) {
        isSavingCloudData = true;
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("SaveJSONPlayerData")
            .SetEventAttribute("Data", new GSRequestData().AddJSONStringAsObject(key, JsonUtility.ToJson(data)))
            .Send(
            // Success response
            (GameSparks.Api.Responses.LogEventResponse response) => {
                if(onSuccess != null) onSuccess();
                Debug.Log("Cloud data successfully saved: " + JsonUtility.ToJson(data, true));
                isSavingCloudData = false;
            },
            // Error response
            (GameSparks.Api.Responses.LogEventResponse response) => {
                if(onFail != null) onFail();
                Debug.LogWarning("Error saving cloud data: " + response.Errors.JSON);
            });
    }

    public override void LoadLocalData(string key, Type type, Action<object> onSuccess, Action onFail) {
        isLoadingLocalData = true;
        // Load from a text file
        string localDataPath = Path.Combine(Application.persistentDataPath, string.Format("Local{0}UserData.json", key));
        if(File.Exists(localDataPath)) {
            savedLocalData[key] = File.ReadAllText(localDataPath);
        }

        // Construct type from JSON
        string jsonData = "";
        if(savedLocalData.TryGetValue(key, out jsonData)) {
            if(onSuccess != null) onSuccess(JsonUtility.FromJson(jsonData, type));
        } else {
            if(onFail != null) onFail();
        }
        Debug.Log("Local data successfully loaded: \n" + jsonData);
        isLoadingLocalData = false;
    }

    public override void SaveLocalData(string key, object data, Action onSuccess, Action onFail) {
        isSavingLocalData = true;
        // Serialize data to JSON
        savedLocalData[key] = JsonUtility.ToJson(data, true);
        Debug.Log("Local data successfully saved: \n" + savedLocalData[key]);
        isSavingLocalData = false;

        // Save to a text file
        string localDataPath = Path.Combine(Application.persistentDataPath, string.Format("Local{0}UserData.json", key));
        File.WriteAllText(localDataPath, savedLocalData[key]);
        isSavingLocalData = false;

        if(onSuccess != null) onSuccess();
    }

    public override void Logout() {
        GS.Reset();
    }

    private void ShutdownGamesparks() {
        GS.Disconnect();
        GS.ShutDown();
    }

    public void Initialize() {
        // Logging out on game load prevents gamesparks
        // from auto-signing in, if you want auto-login
        // then comment this line
        Logout();
        Reset();
    }

    public void Deinitialize() {
        ShutdownGamesparks();
        Reset();
    }

    private void Reset() {
        isAvailable = true;
        isRegistered = false;
        isAuthenticated = false;
        isRegistering = false;
        isAuthenticating = false;
    }
}
