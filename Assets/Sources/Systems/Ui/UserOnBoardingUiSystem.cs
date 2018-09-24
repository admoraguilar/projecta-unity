using System.Collections;
using UnityEngine;
using WEngine;
using DoozyUI;


/// <summary>
/// Handles the UserOnBoardingUi.
/// </summary>
public class UserOnBoardingUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public AUi Ui;

    private int currentScreen;


    private void InitUi() {
        // Show first screen
        Ui.SetUiHierarchy(GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementName,
                          GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementCategory);

        StartCoroutine(NextButtonHandlers());
    }

    private void DeinitializeUi() {
        StopAllCoroutines();
    }

    private IEnumerator NextButtonHandlers() {
        while(true) {
            // Disable some inputs if conditions aren't met
            GameData.Container.Ui.UserOnBoarding.GenderNextButton.Interactable = GameData.Persistent.Player.GenderId.Length > 0;
            GameData.Container.Ui.UserOnBoarding.DisplayNameInputFieldNextButton.Interactable = GameData.Transient.Player.DisplayName.Length > 2;
            GameData.Container.Ui.UserOnBoarding.AvatarMakerNextButton.Interactable = GameData.Persistent.Player.AvatarId == "Default" || GameData.Persistent.Player.AvatarId.Length == 0 
                                                                                      ? false
                                                                                      : true; 
            yield return null;
        }
    }

    private void Start() {
        // Hook handlers
        GameData.Container.Ui.UserOnBoarding.UserOnBoardingUi.OnInAnimationsStart.AddListener(InitUi);
        GameData.Container.Ui.UserOnBoarding.UserOnBoardingUi.OnOutAnimationsFinish.AddListener(DeinitializeUi);

	    // Hook elements
        // Next buttons
        foreach(UIButton nextButton in GameData.Container.Ui.UserOnBoarding.NextButton) {
            nextButton.OnClick.AddListener(() => {
                GameData.Container.Ui.UserOnBoarding.OnBoarding.Next(false);
                Ui.SetUiHierarchy(GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementName,
                                  GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementCategory);
            });
        }

        // Prev buttons
        foreach(UIButton prevButton in GameData.Container.Ui.UserOnBoarding.PrevButton) {
            prevButton.OnClick.AddListener(() => {
                GameData.Container.Ui.UserOnBoarding.OnBoarding.Prev(false);
                Ui.SetUiHierarchy(GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementName,
                                  GameData.Container.Ui.UserOnBoarding.OnBoarding.GetCurrent().elementCategory);
            });
        }

        // Landing room next button (Going to landing room)
        GameData.Container.Ui.UserOnBoarding.LandingRoomNextButton.OnClick.AddListener(() => {
            GameData.Persistent.Player.IsUserOnBoard = true;
        });

        // Share to social media buttons

        // Gender buttons
        GameData.Container.Ui.UserOnBoarding.GenderMaleButton.OnClick.AddListener(() => {
            GameData.Persistent.Player.GenderId = GameData.Key.Player.GenderMale;
        });

        GameData.Container.Ui.UserOnBoarding.GenderFemaleButton.OnClick.AddListener(() => {
            GameData.Persistent.Player.GenderId = GameData.Key.Player.GenderFemale;
        });

        // We moved this implementation on the sign-in screen because
        // we need the display name upfront for Photon and
        // Gamesparks
        // Display name input field
        //GameData.Container.Ui.UserOnBoarding.DisplayNameInputField.onValueChanged.AddListener((value) => {
        //    GameData.Transient.Player.DisplayName = value;
        //});
    }
}
