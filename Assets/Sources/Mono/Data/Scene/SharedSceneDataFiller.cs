using System;
using System.Collections.Generic;
using UnityEngine;
using WEngine;


public class SharedSceneDataFiller : MonoBehaviour {
    public SharedSceneData Data;

    [Inject] public AGameData GameData;


    private void Awake() {
        // We manual inject here because again, auto-injection doesn't
        // work on Awake
        Main.Inject(this);

        GameData.Container.Scene.Shared.SpawnPoints.AddRange(Data.SpawnPoints);    
    }

    private void OnDestroy() {
        foreach(Transform spawnPoint in Data.SpawnPoints) {
            GameData.Container.Scene.Shared.SpawnPoints.Remove(spawnPoint);
        }
    }
}


[Serializable]
public class SharedSceneData {
    public List<Transform> SpawnPoints = new List<Transform>();
}
