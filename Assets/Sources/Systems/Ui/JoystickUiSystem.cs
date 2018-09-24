using UnityEngine;
using WEngine;


public class JoystickUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public AVoice Voice;

    private Color talkButtonPrevColor;


    private void Start() {
        talkButtonPrevColor = GameData.Container.Ui.Joystick.TalkButtonImage.color; 
    }

    private void Update() {
        if(Voice.IsMuted) {
            GameData.Container.Ui.Joystick.TalkButtonImage.color = new Color(talkButtonPrevColor.r,
                                                                             talkButtonPrevColor.g,
                                                                             talkButtonPrevColor.b,
                                                                             .125f);
        } else {
            GameData.Container.Ui.Joystick.TalkButtonImage.color = talkButtonPrevColor;
        }
    }
}
