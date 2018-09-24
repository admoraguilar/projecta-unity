using UnityEngine;
using UnityEngine.UI;
using DoozyUI;


public class AvatarMakerUiData : MonoBehaviour {
    [Header("Ui")]
    public UIElement AvatarMakerUi;
    public UIElement CosplayUi;
    public UIElement CustomizeUi;
    
    [Header("Avatar Maker")]
    public RectTransform AvatarScrollViewContent;
    public AvatarUiButton AvatarButtonPrefab;
    public UIButton CosplayButton;
    public UIButton CustomizeButton;
    public UIButton ConfirmButton;
    public Text SelectedAvatarText;
}
