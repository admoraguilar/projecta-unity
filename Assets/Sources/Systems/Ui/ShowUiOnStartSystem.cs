using UnityEngine;
using WEngine;


/// <summary>
/// Shows a Ui on start, useful for scenes that just really shows
/// a ui like SignIn.
/// </summary>
public class ShowUiOnStartSystem : MonoBehaviour {
    [SerializeField] private UiToShowOnStart uiToShow;

    [Inject] public AGameData GameData;
    [Inject] public AUi Ui;


    private void Start() {
        switch(uiToShow) {
            case UiToShowOnStart.SignIn:
                Ui.SetUiHierarchy(GameData.Container.Ui.SignIn.SigninUi.elementName, GameData.Container.Ui.SignIn.SigninUi.elementCategory);
                break;
            case UiToShowOnStart.UserOnBoarding:
                Ui.SetUiHierarchy(GameData.Container.Ui.UserOnBoarding.UserOnBoardingUi.elementName, GameData.Container.Ui.UserOnBoarding.UserOnBoardingUi.elementCategory);
                break;
            case UiToShowOnStart.Interface:
                Ui.SetUiHierarchy(GameData.Container.Ui.Interface.InterfaceUi.elementName, GameData.Container.Ui.Interface.InterfaceUi.elementCategory);
                break;
        }
    }
}


public enum UiToShowOnStart {
    SignIn,
    UserOnBoarding,
    Interface
}
