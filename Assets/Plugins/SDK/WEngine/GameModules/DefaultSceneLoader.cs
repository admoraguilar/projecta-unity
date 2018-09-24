using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// This module is used as a wrapper for Unity's scene loading mechanism.
/// Specifically this makes some things like attaching a callback once a scene has finished loading an easier task.
/// </summary>
[CreateAssetMenu(fileName = "DefaultSceneLoader", menuName = "WEngine/Modules/ASceneLoader/DefaultSceneLoader")]
public class DefaultSceneLoader : ASceneLoader {
    /// <summary>
    /// Calls SceneManager.LoadSceneAsync();
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="loadSceneMode"></param>
    /// <param name="onSceneLoad">Instantly gets called. Useful for doing something before the scene loads.</param>
    /// <param name="onSceneLoaded">Gets called when the scene finishes loading. Useful for doing something after the scene loads.</param>
    public override void LoadSceneAsync(string sceneId, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action onSceneLoad = null, Action onSceneLoaded = null) {
        SceneManager.LoadSceneAsync(sceneId, loadSceneMode);

        _OnSceneLoad();
        if(onSceneLoad != null) onSceneLoad();

        // Attach a callback once the scene finishes loading
        UnityAction<Scene, LoadSceneMode> action = null;
        action = (Scene s, LoadSceneMode lsm) => {
            if(onSceneLoaded != null) onSceneLoaded();
            SceneManager.sceneLoaded -= action;
        };

        SceneManager.sceneLoaded += action;
    }


    /// <summary>
    /// Calls SceneManager.UnloadSceneAsync();
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name="onSceneUnload">Instantly gets called. Useful for doing something before the scene unloads.</param>
    /// <param name="onSceneUnloaded">Gets called when the scene finishes unloading. Useful for doing something after the scene unloads.</param>
    public override void UnloadSceneAsync(string sceneId, Action onSceneUnload = null, Action onSceneUnloaded = null) {
        SceneManager.UnloadSceneAsync(sceneId);

        _OnSceneUnload();
        if(onSceneUnload != null) onSceneUnload();

        // Attach a callback once the scene finishes unloading
        UnityAction<Scene, LoadSceneMode> action = null;
        action = (Scene s, LoadSceneMode lsm) => {
            if(onSceneUnloaded != null) onSceneUnloaded();
            SceneManager.sceneLoaded -= action;
        };

        SceneManager.sceneLoaded += action;
    }
}


public abstract class ASceneLoader : ScriptableObject {
    /// <summary>
    /// Gets called whenever a call to LoadSceneAsync() is made.
    /// </summary>
    public event Action OnSceneLoad = delegate { };


    /// <summary>
    /// Gets called whenever a call to UnloadSceneAsync() is made.
    /// </summary>
    public event Action OnSceneUnload = delegate { };

    protected void _OnSceneLoad() { OnSceneLoad(); }
    protected void _OnSceneUnload() { OnSceneUnload(); }

    abstract public void LoadSceneAsync(string sceneId, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Action onSceneLoad = null, Action onSceneLoaded = null);
    abstract public void UnloadSceneAsync(string sceneId, Action onSceneUnload = null, Action onSceneUnloaded = null);
}