using System.Collections;
using UnityEngine;
using WEngine;


public class ConnectToNetworkSystem : MonoBehaviour {
    [Inject] public ANetwork Network;
    [Inject] public AGameData GameData;
    [Inject] public ABackend Backend;

    [SerializeField] private bool requiresBackendSignin = true;


    private IEnumerator ConnectToNetwork() {
        // We connect to the network when the user is signed in
        // and the cloud data is fully loaded, we're waiting for the
        // data to be fully loaded to avoid a bug where the more
        // technical name is the one being assigned as the user name
        if(requiresBackendSignin) {
            yield return new WaitUntil(() => Backend.IsAvailable);
            yield return new WaitUntil(() => Backend.IsAuthenticated);
            yield return new WaitUntil(() => GameData.IsLoaded);
        }

        Network.SetUserId(GameData.Transient.Player.DisplayName);
        Network.SetGameVersion(GameData.Transient.App.AppVersion);

        Network.ConnectToServer();
    }

    private void Start() {
        StartCoroutine(ConnectToNetwork());
    }
}
