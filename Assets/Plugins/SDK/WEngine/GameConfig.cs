using UnityEngine;


/// <summary>
/// A collection of Configs, useful for better organization of your
/// configs, and modules.
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "WEngine/GameConfig")]
public class GameConfig : ScriptableObject {
    public bool IsEnabled = true;
    public Config[] Configs;
}