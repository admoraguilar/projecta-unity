using UnityEngine;
using System;


/// <summary>
/// A collection of modules.
/// </summary>
[CreateAssetMenu(fileName = "Config", menuName = "WEngine/Config")]
public class Config : ScriptableObject {
    public string Label;
    public bool IsIncluded = true;
    public Module[] Modules;
}


/// <summary>
/// A collection of scriptable and game object modules.
/// </summary>
[Serializable]
public class Module {
    public string Label;
    public bool IsIncluded = true;
    public ScriptableObject[] ScriptableObjects;
    public GameObject[] GameObjects;
}