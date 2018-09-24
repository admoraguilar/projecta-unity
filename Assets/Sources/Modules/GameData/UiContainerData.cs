using UnityEngine;
using WEngine;


public class UiContainerData : MonoBehaviour {
    [Header("Containers")]
    public Transform MasterContainer;

    [Header("Canvas")]
    public RectTransform OverlayNonScaledMasterCanvas;
    public RectTransform OverlayMasterCanvas;

    [Header("UIElements")]
    public LoadingUiData Loading;
    public SignInUiData SignIn;
    public UserOnBoardingUiData UserOnBoarding;
    public InputUiData Input;
    public InterfaceUiData Interface;
    public JoystickUiData Joystick;
    public DebugUiData Debug;
}