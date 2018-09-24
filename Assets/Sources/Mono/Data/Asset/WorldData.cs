using UnityEngine;
using WEngine;


public class WorldData : MonoBehaviour {
    public string Id;
    [ObjectField(ObjectFieldType.SceneAsset)] public string Scene;
    public bool IsDebug;
}
