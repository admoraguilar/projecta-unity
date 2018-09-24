using UnityEngine;
using UnityEngine.SceneManagement;
using WEngine;


/// <summary>
/// Loads the loading screen whenever a scene load happens.
/// </summary>
public class LoadingScreenSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public AUi Ui;
    [Inject] public ASceneLoader SceneLoader;


    private void Start() {
        // Register handlers
        SceneLoader.OnSceneLoad += OnSceneLoad;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoad() {
        Ui.SetUiHierarchy(GameData.Container.Ui.Loading.DefaultLoading.elementName, GameData.Container.Ui.Loading.DefaultLoading.elementCategory);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
        Ui.PopUiHierarchy(GameData.Container.Ui.Loading.DefaultLoading.elementName, GameData.Container.Ui.Loading.DefaultLoading.elementCategory);
    }
}
