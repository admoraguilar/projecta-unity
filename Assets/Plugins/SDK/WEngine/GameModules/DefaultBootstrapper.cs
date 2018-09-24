using UnityEngine;
using UnityEngine.SceneManagement;
using WEngine;


/// <summary>
/// This module loads a starting scene of your choice.
/// </summary>
[CreateAssetMenu(fileName = "DefaultBootstrapper", menuName = "WEngine/Modules/ABootstrapper/DefaultBootstrapper")]
public class DefaultBootstrapper : ABootstrapper, IInitializable {
    public int Priority { get { return 0; } }

    [ObjectField(ObjectFieldType.SceneAsset), SerializeField] private string startingScene;
    [SerializeField] private bool isLoadStartingScene;


    public override void Bootstrap() {
        if(isLoadStartingScene) {
            SceneManager.LoadScene(startingScene);
        }
    }

    public void Initialize() {
        Bootstrap();
    }
}


public abstract class ABootstrapper : ScriptableObject {
    abstract public void Bootstrap();
}