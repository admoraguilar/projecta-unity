using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;


public class PlayerDataSaverTest : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public ABackend Backend;


    private void Update() {
        //if(Input.GetKeyDown(KeyCode.E)) {
        //    object appData = null;
        //    Backend.LoadLocalData("App", typeof(AppPersistentData), ref appData);
        //    GameData.Persistent.App = (AppPersistentData)appData ?? new AppPersistentData();

        //    object data = null;
        //    Backend.LoadCloudData("Player", typeof(PlayerPersistentData), ref data);
        //    GameData.Persistent.Player = (PlayerPersistentData)data ?? new PlayerPersistentData();
        //}
        //if(Input.GetKeyDown(KeyCode.R)) {
        //    Backend.SaveLocalData("App", GameData.Persistent.App);
        //    Backend.SaveCloudData("Player", GameData.Persistent.Player);
        //}
    }
}
