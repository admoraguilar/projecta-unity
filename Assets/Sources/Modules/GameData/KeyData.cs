using System;
using UnityEngine;
using WEngine;


[Serializable]
public class KeyData {
    public SceneKeyData Scene = new SceneKeyData();
    public PlayerKeyData Player = new PlayerKeyData();
    public RoomKeyData Room = new RoomKeyData();
    public string[] Admin = new string[0];
}


[Serializable]
public class PlayerKeyData {
    public string GenderMale = "Male";
    public string GenderFemale = "Female";

    public string CustomPropertyAvatarIdKey = "AvatarId";
    public string CustomPropertyMoveHorizontalInputKey = "MoveHorizontalInput";
    public string CustomPropertyMoveVerticalInputKey = "MoveVerticalInput";
    public string CustomPropertyJumpInputKey = "MoveJumpInput";
    public string CustomPropertyEmoteInputKey = "MoveEmoteInput";
}



[Serializable]
public class SceneKeyData {
    [ObjectField(ObjectFieldType.SceneAsset)] public string LandingRoom;
    [ObjectField(ObjectFieldType.SceneAsset)] public string SignIn;
    [ObjectField(ObjectFieldType.SceneAsset)] public string UserOnBoarding;
}

[Serializable]
public class RoomKeyData {
    public string CustomPropertySceneNameKey = "SceneName";
    public string CustomPropertyIsWorldPrivateKey = "IsWorldPrivate";
}