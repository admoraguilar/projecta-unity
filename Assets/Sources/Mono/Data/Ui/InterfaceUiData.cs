using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WEngine;
using DoozyUI;
using DoozyUI.Gestures;


public class InterfaceUiData : MonoBehaviour {
    [Header("UI")]
    public UIElement InterfaceUi;
    public GameSettingsUiData GameSettingsUi;
    public UIElement WorldsUi;
    public AvatarMakerUiData AvatarMakerUi;
    public UIElement InterfaceHomescreenUi;
    public UIElement InterfacesGridUi;

    [Header("Shared")]
    public UIButton InterfacePrevButton;
    public UIButton InterfaceNextButton;
    public UIButton InterfaceBackButton;
    public InterfaceUiButton InterfaceUiButtonPrefab;
    public Text UserNameText;
    public Text LocationText;

    [Header("Settings")]
    public Text AppVersionNumberText;

    [Header("Worlds")]
    public EnumerableUiElements Interfaces = new EnumerableUiElements();
    public EnumerableUiElements Worlds = new EnumerableUiElements();
    public UIElement WorldHomespace;
    public UIElement WorldUserId;
    public UIButton WorldHomespaceGo;
    public InputField WorldUserNameInputField;
    public UIButton WorldUserNameGo;
    public WorldInfoUiElement WorldInfoPrefab;
    public UIButton WorldsPrevButton;
    public UIButton WorldsNextButton;
}