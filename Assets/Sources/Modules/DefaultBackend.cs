using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// <para>This module handles connection to a backend service for saving and loading of cloud data.</para>
/// <para>It also handles saving and loading of some local data such as AppData.</para>
/// </summary>
[CreateAssetMenu(fileName = "DefaultBackend", menuName = "WEngine/Modules/ABackend/DefaultBackend")]
public class DefaultBackend : ABackend, IInitializable, IDeinitializable {
    public int Priority { get { return 1000; } }

    public override bool IsAvailable { get { return isAvailable; } }
    public override bool IsRegistered { get { return isRegistered; } }
    public override bool IsAuthenticated { get { return isAuthenticated; } }
    public override bool IsRegistering { get { return isRegistering; } }
    public override bool IsAuthenticating { get { return isAuthenticating; } }
    public override bool IsLoadingCloudData { get { return isLoadingCloudData; } }
    public override bool IsSavingCloudData { get { return isSavingCloudData; } }
    public override bool IsLoadingLocalData { get { return isLoadingLocalData; } }
    public override bool IsSavingLocalData { get { return isSavingLocalData; } }

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
        isRegistered = true;
        isRegistering = false;
    }

    public override void Authenticate() {
        isAuthenticated = true;
        isAuthenticating = false;
    }

    public override void LoadCloudData(string key, Type type, Action<object> onSuccess, Action onFail) {
        //string jsonData = "";
        //if(savedCloudData.TryGetValue(key, out jsonData)) {
        //    data = JsonUtility.FromJson(jsonData, type);
        //}
        //Debug.Log("Loaded cloud data: \n" + jsonData);
        //isLoadingCloudData = false;

        // We do local storage because this module doesn't
        // really connect to an actual backend service
        LoadLocalData(key, type, onSuccess, onFail);
        isLoadingCloudData = false;
    }

    public override void SaveCloudData(string key, object data, Action onSuccess, Action onFail) {
        //savedCloudData[key] = JsonUtility.ToJson(data, true);
        //Debug.Log("Saved cloud data: \n" + savedCloudData[key]);
        //isSavingCloudData = false;

        // We do local storage because this module doesn't
        // really connect to an actual backend service
        SaveLocalData(key, data, onSuccess, onFail);
        isSavingCloudData = false;
    }

    public override void LoadLocalData(string key, Type type, Action<object> onSuccess, Action onFail) {
        // Load from a text file
        string localDataPath = Path.Combine(Application.persistentDataPath, string.Format("Local{0}UserData.json", key));
        if(File.Exists(localDataPath)) {
            savedLocalData[key] = File.ReadAllText(localDataPath);
            Debug.Log(string.Format("File loaded: {0}", localDataPath));

            // Construct type from JSON
            string jsonData = "";
            if(savedLocalData.TryGetValue(key, out jsonData)) {
                if(onSuccess != null) onSuccess(JsonUtility.FromJson(jsonData, type));
                Debug.Log("Local data successfully loaded: " + jsonData);
            } else {
                if(onFail != null) onFail();
            }
        }

        isLoadingLocalData = false;
    }

    public override void SaveLocalData(string key, object data, Action onSuccess, Action onFail) {
        // Serialize data to JSON
        savedLocalData[key] = JsonUtility.ToJson(data, true);
        Debug.Log("Local data successfully saved: \n" + savedLocalData[key]);

        // Save to a text file
        string localDataPath = Path.Combine(Application.persistentDataPath, string.Format("Local{0}UserData.json", key));
        File.WriteAllText(localDataPath, savedLocalData[key]);
        Debug.Log(string.Format("File saved: {0}", localDataPath));

        if(onSuccess != null) onSuccess();
        isSavingLocalData = false;
    }

    public override void Logout() {
        isRegistered = false;
        isAuthenticated = false;
    }

    public void Initialize() {
        Reset();
    }

    public void Deinitialize() {
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


public abstract class ABackend : SerializedScriptableObject {
    abstract public bool IsAvailable { get; }
    abstract public bool IsRegistered { get; }
    abstract public bool IsAuthenticated { get; }
    abstract public bool IsRegistering { get; }
    abstract public bool IsAuthenticating { get; }
    abstract public bool IsLoadingCloudData { get; }
    abstract public bool IsSavingCloudData { get; }
    abstract public bool IsLoadingLocalData { get; }
    abstract public bool IsSavingLocalData { get; }


    abstract public void Register();
    abstract public void Authenticate();
    abstract public void LoadCloudData(string key, Type type, Action<object> onSuccess, Action onFail);
    abstract public void SaveCloudData(string key, object data, Action onSuccess, Action onFail);
    abstract public void LoadLocalData(string key, Type type, Action<object> onSuccess, Action onFail);
    abstract public void SaveLocalData(string key, object data, Action onSuccess, Action onFail);
    abstract public void Logout();
}