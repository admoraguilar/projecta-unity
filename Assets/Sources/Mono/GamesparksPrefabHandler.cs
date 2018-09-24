using UnityEngine;
using GameSparks.Core;


public class GamesparksPrefabHandler : MonoBehaviour {
    private void ShutdownGamesparks() {
        GS.Disconnect();
        GS.ShutDown();
    }

    private void OnDestroy() {
        ShutdownGamesparks();
    }

    private void OnApplicationQuit() {
        ShutdownGamesparks();
    }
}
