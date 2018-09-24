using System.Collections;
using UnityEngine;
using WEngine;


/// <summary>
/// Loads the landing room on start.
/// </summary>
public class LoadLandingRoomOnStartSystem : MonoBehaviour {
    [SerializeField] private LoadLandingRoomFrom loadLandingRoomFrom;

    [Inject] public AConnectionChecker ConnectionChecker;
    [Inject] public ANetwork Network;
    [Inject] public AGameData GameData;
    [Inject] public ABackend Backend;
    [Inject] public ASceneLoader SceneLoader;


    private IEnumerator LoadLandingRoom() {
        yield return new WaitUntil(() => ConnectionChecker.IsAllConnectionOk);
        yield return new WaitUntil(() => Network.IsConnectedAndReady);

        switch(loadLandingRoomFrom) {
            case LoadLandingRoomFrom.Buffer:

                break;
            case LoadLandingRoomFrom.SignIn:
                yield return new WaitUntil(() => Backend.IsAuthenticated);
                yield return new WaitUntil(() => GameData.IsLoaded);

                // Wait some seconds so we can still display a 
                // sign-in successful response
                // We also wait some time so that GameDataHandlerSystem has
                // time to load up some persistent data
                yield return new WaitForSeconds(2f);

                // If the player hasn't been on boarded yet (For new player)
                if(!GameData.Persistent.Player.IsUserOnBoard) {
                    SceneLoader.LoadSceneAsync(GameData.Key.Scene.UserOnBoarding);
                    yield break;
                }
                break;
            case LoadLandingRoomFrom.UserOnBoarding:
                yield return new WaitUntil(() => Backend.IsAuthenticated);

                while(!GameData.Persistent.Player.IsUserOnBoard) {
                    yield return null;
                }
                break;
        }

        Network.CreateRoomBySceneName(GameData.Key.Scene.LandingRoom, true);
    }

    private void Start() {
        StartCoroutine(LoadLandingRoom());
    }
}


public enum LoadLandingRoomFrom {
    Buffer,
    SignIn,
    UserOnBoarding
}