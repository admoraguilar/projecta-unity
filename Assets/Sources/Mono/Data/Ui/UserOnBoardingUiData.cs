using UnityEngine;
using UnityEngine.UI;
using DoozyUI;


public class UserOnBoardingUiData : MonoBehaviour {
    [Header("Ui")]
    public UIElement UserOnBoardingUi;
    public AvatarMakerUiData AvatarMakerUi;

    [Header("On Boarding")]
    public EnumerableUiElements OnBoarding;
    public UIButton[] NextButton;
    public UIButton[] PrevButton;
    public UIButton GenderMaleButton;
    public UIButton GenderFemaleButton;
    public UIButton ShareToFacebookButton;
    public UIButton ShareToTwitterButton;
    public UIButton ShareToGPlusButton;
    public InputField DisplayNameInputField;

    public UIButton GenderNextButton;
    public UIButton DisplayNameInputFieldNextButton;
    public UIButton AvatarMakerNextButton;
    public UIButton LandingRoomNextButton;
}