using System.Collections;
using UnityEngine;
using WEngine;


/// <summary>
/// Instantiates a player on the network on start.
/// </summary>
public class InstantiatePlayerOnStartSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public ANetwork Network;
    [Inject] public AUi Ui;


    private IEnumerator InstantiatePlayer() {
        yield return new WaitUntil(() => Network.IsConnectedAndReady);
        yield return new WaitUntil(() => Network.IsInRoom);

        // We choose a spawn point along the scene
        Transform spawnPoint = null;
        if(GameData.Container.Scene.Shared.SpawnPoints != null) {
            if(GameData.Container.Scene.Shared.SpawnPoints.Count > 0) {
                spawnPoint = GameData.Container.Scene.Shared.SpawnPoints[Random.Range(0, GameData.Container.Scene.Shared.SpawnPoints.Count)];
            }
        }

        // Spawn player prefab
        //Network.Instantiate(GameData.Container.Asset.PlayerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
        Network.Instantiate(GameData.Container.Asset.PlayerPrefab,
                            spawnPoint ? spawnPoint.position : new Vector3(0f, 5f, 0f),
                            spawnPoint ? spawnPoint.rotation : Quaternion.identity,
                            0,
                            null);

        // Show ui inputs
        Ui.SetUiHierarchy(GameData.Container.Ui.Input.InputUi.elementName, GameData.Container.Ui.Input.InputUi.elementCategory);
        Ui.PushUiHierarchy(GameData.Container.Ui.Joystick.JoystickUi.elementName, GameData.Container.Ui.Joystick.JoystickUi.elementCategory);
    }

    private void Start() {
        StartCoroutine(InstantiatePlayer());
    }
}
