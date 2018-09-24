using System.Linq;
using UnityEngine;
using WEngine;
using DoozyUI;


public class AvatarMakerUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public AUi Ui;
    [Inject] public ANetwork Network;

    private string selectedAvatar;


    private void InitializeInterfaceUiAvatarMaker() {
        SharedInitialize(GameData.Container.Ui.Interface.AvatarMakerUi);
    }

    private void DeinitializeInterfaceUiAvatarMaker() {
        SharedDeinitialize(GameData.Container.Ui.Interface.AvatarMakerUi);
    }

    private void InitializeUserOnBoardingUiAvatarMaker() {
        SharedInitialize(GameData.Container.Ui.UserOnBoarding.AvatarMakerUi);
    }

    private void DeinitializeUserOnBoardingUiAvatarMaker() {
        SharedDeinitialize(GameData.Container.Ui.UserOnBoarding.AvatarMakerUi);
    }

    private void SharedInitialize(AvatarMakerUiData data) {
        //selectedAvatar = GameData.Persistent.Player.AvatarId;
        data.SelectedAvatarText.text = selectedAvatar;

        // Show first screen
        data.AvatarMakerUi.Show(false);
        data.CosplayUi.Show(false);
    }

    private void SharedDeinitialize(AvatarMakerUiData data) {
        
    }

    private void InstantiateAvatarButtons(AvatarMakerUiData data, bool directWriteAvatarToGameData) {
        foreach(var kvp in GameData.Container.Asset.Avatars) {
            // Only show debug avatars for the admin
            if(kvp.Value.IsDebug && !GameData.Transient.Player.IsAdmin) continue;

            // Instantiate the element
            UnityExtension.ExtensionInstantiate(data.AvatarButtonPrefab, (ab) => {
                // Configure the element
                ab.name = kvp.Key;
                ab.GetComponent<Transform>().SetParent(data.AvatarScrollViewContent);

                ab.Image.sprite = kvp.Value.Banner;
                ab.Text.text = kvp.Key;

                // Element controls
                ab.Button.OnClick.AddListener(() => {
                    selectedAvatar = kvp.Key;
                    data.SelectedAvatarText.text = selectedAvatar;
                    if(directWriteAvatarToGameData) WriteAvatarToGameData();
                });
            });
        }
    }

    private void CosplayButton(AvatarMakerUiData data) {
        data.CosplayButton.OnClick.AddListener(() => {
            data.CustomizeUi.Hide(false);
            data.CosplayUi.Show(false);
        });
    }

    private void CustomizeButton(AvatarMakerUiData data) {
        data.CustomizeButton.OnClick.AddListener(() => {
            data.CustomizeUi.Show(false);
            data.CosplayUi.Hide(false);
        });
    }

    private void ConfirmButton(AvatarMakerUiData data) {
        data.ConfirmButton.OnClick.AddListener(() => {
            WriteAvatarToGameData();
        });
    }

    private void WriteAvatarToGameData() {
        if(GameData.Container.Asset.Avatars.ContainsKey(selectedAvatar)) {
            GameData.Persistent.Player.AvatarId = selectedAvatar;
            Debug.Log("Found avatar: " + selectedAvatar);
        }
    }

    private void Start() {
        // InterfaceUi AvatarMaker
        GameData.Container.Ui.Interface.AvatarMakerUi.AvatarMakerUi.OnInAnimationsStart.AddListener(InitializeInterfaceUiAvatarMaker);
        GameData.Container.Ui.Interface.AvatarMakerUi.AvatarMakerUi.OnOutAnimationsFinish.AddListener(DeinitializeInterfaceUiAvatarMaker);
        InstantiateAvatarButtons(GameData.Container.Ui.Interface.AvatarMakerUi, false);
        CosplayButton(GameData.Container.Ui.Interface.AvatarMakerUi);
        CustomizeButton(GameData.Container.Ui.Interface.AvatarMakerUi);
        ConfirmButton(GameData.Container.Ui.Interface.AvatarMakerUi);

        // UserOnBoardingUi AvatarMaker
        UIElement userOnBoardingAvatarMaker = GameData.Container.Ui.UserOnBoarding.OnBoarding.Elements.FirstOrDefault(ui => ui.GetComponentInChildren<AvatarMakerUiData>());
        if(userOnBoardingAvatarMaker != null) {
            userOnBoardingAvatarMaker.OnInAnimationsStart.AddListener(InitializeUserOnBoardingUiAvatarMaker);
            userOnBoardingAvatarMaker.OnOutAnimationsFinish.AddListener(DeinitializeUserOnBoardingUiAvatarMaker);
            InstantiateAvatarButtons(GameData.Container.Ui.UserOnBoarding.AvatarMakerUi, true);
            CosplayButton(GameData.Container.Ui.UserOnBoarding.AvatarMakerUi);
            CustomizeButton(GameData.Container.Ui.UserOnBoarding.AvatarMakerUi);
        }
    }
}
