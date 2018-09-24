using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "AssetContainerData", menuName = "WEngine/Containers/AssetContainerData")]
public class AssetContainerData : SerializedScriptableObject {
    public Dictionary<string, AvatarData> Avatars = new Dictionary<string, AvatarData>();
    public Dictionary<string, WorldData> Worlds = new Dictionary<string, WorldData>();
    public List<SignInBackgroundData> SignInBackgrounds = new List<SignInBackgroundData>();
    public GameObject PlayerPrefab;

    [Title("Paths")]
    [FolderPath(ParentFolder = "Assets/Resources")] public string[] AvatarPaths;
    [FolderPath(ParentFolder = "Assets/Resources")] public string[] WorldPaths;
    [FolderPath(ParentFolder = "Assets/Resources")] public string[] SignInBackgroundPaths;
}
