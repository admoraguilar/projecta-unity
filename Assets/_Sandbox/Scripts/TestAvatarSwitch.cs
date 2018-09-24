using System.Collections;
using System.Linq;
using UnityEngine;
using WEngine;


public class TestAvatarSwitch : MonoBehaviour {
    [Inject] public ANetwork Network;
    [Inject] public AGameData GameData;

    [SerializeField] private int avatarIndex;

    private void Update() {
        // Test avatar switch
        if(Input.GetKeyDown(KeyCode.F)) {
            Network.SetPlayerCustomProperties(new DictionaryEntry[] {
                new DictionaryEntry(GameData.Key.Player.CustomPropertyAvatarIdKey, 
                                    GameData.Container.Asset.Avatars.Keys.ToArray()[avatarIndex])
            });
            avatarIndex = avatarIndex == GameData.Container.Asset.Avatars.Count - 1 ? 0 : avatarIndex + 1;
        }
    }
}
