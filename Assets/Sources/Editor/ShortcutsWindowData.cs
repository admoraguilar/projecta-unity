using System;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "ShortcutsWindowData", menuName = "WEngine/Settings/ShortcutsWindowData")]
public class ShortcutsWindowData : SerializedScriptableObject {
    public GameObjectShortcut[] GameObjectShortcuts;
}


[Serializable]
public class GameObjectShortcut {
    public string Label;
    public UnityEngine.Object GameObject;
}