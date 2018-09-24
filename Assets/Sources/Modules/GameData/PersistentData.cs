using System;
using UnityEngine;


[Serializable]
public class PersistentData {
    public AppPersistentData App = new AppPersistentData();
    public PlayerPersistentData Player = new PlayerPersistentData();
}


[Serializable]
public class PlayerPersistentData {
    public string AvatarId = "Default";
    public string GenderId = "";
    public bool IsUserOnBoard = false;
}


[Serializable]
public class AppPersistentData {
    public float LookSensitivity = 1f;
    public bool IsMusicOn = true;
}
