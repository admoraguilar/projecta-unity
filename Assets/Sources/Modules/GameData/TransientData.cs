using System;
using UnityEngine;


[Serializable]
public class TransientData {
    public AppTransientData App = new AppTransientData();
    public PlayerTransientData Player = new PlayerTransientData();
}


[Serializable]
public class PlayerTransientData {
    public string UserName = "User";
    public string Password = "Pass";
    public string DisplayName = "User";
    public bool IsNewPlayer = true;
    public bool IsAdmin = false;

    public float MoveVerticalInput = 0f;
    public bool JumpInput = false;
    public bool EmoteInput = false;
}


[Serializable]
public class AppTransientData {
    public string AppVersion = "0.05";
}