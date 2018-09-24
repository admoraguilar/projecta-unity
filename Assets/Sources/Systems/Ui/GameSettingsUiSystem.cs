using UnityEngine;
using WEngine;


public class GameSettingsUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;


    private void InitializeInterfaceUiGameSettings() {
        // InterfaceUi GameSettings
        GameData.Container.Ui.Interface.GameSettingsUi.LookSensitivitySlider.value = GameData.Persistent.App.LookSensitivity;

    }

    private void DeinitializeInterfaceUiGameSettings() {

    }

    private void Start() {
        // InterfaceUi GameSettings
        GameData.Container.Ui.Interface.GameSettingsUi.GameSettingsUi.OnInAnimationsStart.AddListener(InitializeInterfaceUiGameSettings);
        GameData.Container.Ui.Interface.GameSettingsUi.GameSettingsUi.OnOutAnimationsFinish.AddListener(DeinitializeInterfaceUiGameSettings);

        GameData.Container.Ui.Interface.GameSettingsUi.LookSensitivitySlider.onValueChanged.AddListener((value) => {
            GameData.Persistent.App.LookSensitivity = value;
        });
    }
}
