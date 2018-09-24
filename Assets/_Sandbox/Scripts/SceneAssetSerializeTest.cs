using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WEngine;

public class SceneAssetSerializeTest : MonoBehaviour {
    [ObjectField(ObjectFieldType.SceneAsset)] public string Scene;
    public Text Text;
    public UnityEngine.Object TestObject;
    public UnityEngine.Object TestObject2;

    private Dictionary<UnityEngine.Object, string> dict = new Dictionary<Object, string>();


    private void Start() {
        // Init
        dict[TestObject] = "TestObject success!";
        dict[TestObject2] = "TestObject2 success!";

        // Query
        if(Scene != null) {
            Debug.Log("Scene: " + Scene);
            Text.text = "Scene: " + Scene;
        } else {
            Debug.Log("Scene is not available!");
            Text.text = "Scene is not available!";
        }

        Debug.Log(dict[TestObject]);
        Debug.Log(dict[TestObject2]);
    }
}
