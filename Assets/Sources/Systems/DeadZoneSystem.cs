using UnityEngine;
using WEngine;


/// <summary>
/// Class responsible for respawning an object to a spawn point if the player goes out of the world
/// </summary>
public class DeadZoneSystem : MonoBehaviour {
    [Inject] public AGameData GameData;


    private void OnTriggerEnter(Collider other) {
        Transform otherT = other.GetComponent<Transform>();
        
        // We choose a spawn point
        Transform spawnPoint = null;
        if(GameData.Container.Scene.Shared.SpawnPoints != null) {
            if(GameData.Container.Scene.Shared.SpawnPoints.Count > 0) {
                spawnPoint = GameData.Container.Scene.Shared.SpawnPoints[Random.Range(0, GameData.Container.Scene.Shared.SpawnPoints.Count)];
            }
        }

        otherT.position = spawnPoint ? spawnPoint.position : new Vector3(0f, 5f, 0f);
        otherT.rotation = spawnPoint ? spawnPoint.rotation : Quaternion.identity;
    }
}
