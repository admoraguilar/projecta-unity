using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;
using DoozyUI;


/// <summary>
/// Handles the InterfaceUi.
/// </summary>
public class InterfaceUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;    
    [Inject] public AUi Ui;
    [Inject] public ANetwork Network;

    private string userNameToJoin;


    private void InitializeUi() {
        //interfaces.Set(0);
        //worlds.Set(0);
    }

    private void DeinitializeUi() {

    }

    #region Ui Events
    private void Worlds_OnSet(UIElement uiElement) {
        
    }

    private void Interfaces_OnSet(UIElement uiElement) {
        // Show the last shown WorldsUi when we show WorldUi in the interface
        if(uiElement == GameData.Container.Ui.Interface.WorldsUi) {
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Worlds.GetCurrent().elementName, 
                               GameData.Container.Ui.Interface.Worlds.GetCurrent().elementCategory);
        }
    }
    #endregion

    private void Start() {
        // Hook handlers
        GameData.Container.Ui.Interface.InterfaceUi.OnInAnimationsStart.AddListener(InitializeUi);
        GameData.Container.Ui.Interface.InterfaceUi.OnOutAnimationsFinish.AddListener(DeinitializeUi);
        GameData.Container.Ui.Interface.Interfaces.OnSet += Interfaces_OnSet;
        GameData.Container.Ui.Interface.Worlds.OnSet += Worlds_OnSet;

        // Hook elements
        // Interface Enumerable
        GameData.Container.Ui.Interface.Interfaces.Elements.Add(GameData.Container.Ui.Interface.WorldsUi);
        GameData.Container.Ui.Interface.Interfaces.Elements.Add(GameData.Container.Ui.Interface.GameSettingsUi.GameSettingsUi);
        GameData.Container.Ui.Interface.Interfaces.Elements.Add(GameData.Container.Ui.Interface.AvatarMakerUi.AvatarMakerUi);

        // Interface controls
        GameData.Container.Ui.Interface.InterfacePrevButton.OnClick.AddListener(() => {
            Ui.PopUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName, 
                              GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
            GameData.Container.Ui.Interface.Interfaces.Prev();
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName, 
                               GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
        });

        GameData.Container.Ui.Interface.InterfaceNextButton.OnClick.AddListener(() => {
            Ui.PopUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName, 
                              GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
            GameData.Container.Ui.Interface.Interfaces.Next();
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName, 
                               GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
        });

        GameData.Container.Ui.Interface.InterfaceBackButton.OnClick.AddListener(() => {
            Ui.PopUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName,
                              GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.InterfacesGridUi.elementName,
                               GameData.Container.Ui.Interface.InterfacesGridUi.elementCategory);
        });

        // Interface buttons
        for(int i = 0; i < GameData.Container.Ui.Interface.Interfaces.Elements.Count; ++i) {
            UIElement @interface = GameData.Container.Ui.Interface.Interfaces.Elements[i];
            int indexCatch = i;
            InterfaceUiButton interfaceUiButton = UnityExtension.ExtensionInstantiate(GameData.Container.Ui.Interface.InterfaceUiButtonPrefab,
                (button) => {
                    // Configure the element
                    button.name = @interface.elementName;
                    button.GetComponent<Transform>().SetParent(GameData.Container.Ui.Interface.InterfacesGridUi.GetComponent<Transform>());

                    button.Label.text = @interface.elementName;

                    // Element controls
                    button.Button.OnClick.AddListener(() => {
                        Ui.PopUiHierarchy(GameData.Container.Ui.Interface.InterfaceHomescreenUi.elementName,
                                          GameData.Container.Ui.Interface.InterfaceHomescreenUi.elementCategory);
                        GameData.Container.Ui.Interface.Interfaces.Set(indexCatch);
                        Ui.PushUiHierarchy(@interface.elementName, @interface.elementCategory);
                    });
                });
        }

        GameData.Container.Ui.Input.ShowInterfaceButton.OnClick.AddListener(() => {
            // If interface ui is shown then hide it, but if not
            // then show it
            if(!GameData.Container.Ui.Interface.InterfaceUi.isVisible) {
                //GameData.Container.Ui.Interface.Interfaces.Set(GameData.Container.Ui.Interface.Interfaces.CurrentUiElementIndex);
                //Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementName, 
                //                   GameData.Container.Ui.Interface.Interfaces.GetCurrent().elementCategory);
                Ui.PushUiHierarchy(GameData.Container.Ui.Interface.InterfacesGridUi.elementName,
                                   GameData.Container.Ui.Interface.InterfacesGridUi.elementCategory);
            } else {
                Ui.PopUiHierarchy(GameData.Container.Ui.Interface.InterfaceUi.elementName, 
                                  GameData.Container.Ui.Interface.InterfaceUi.elementCategory);
            }
        });

        // Interface infos
        GameData.Container.Ui.Interface.AppVersionNumberText.text = GameData.Transient.App.AppVersion.ToString();

        // World Enumerable
        // Homespace World
        GameData.Container.Ui.Interface.WorldHomespaceGo.OnClick.AddListener(() => {
            Network.CreateRoomBySceneName(GameData.Key.Scene.LandingRoom, true);
        });

        GameData.Container.Ui.Interface.Worlds.Elements.Add(GameData.Container.Ui.Interface.WorldHomespace);

        // User Id World
        GameData.Container.Ui.Interface.WorldUserNameInputField.onValueChanged.AddListener((string value) => {
            userNameToJoin = value;
        });

        GameData.Container.Ui.Interface.WorldUserNameGo.OnClick.AddListener(() => {
            if(userNameToJoin.Length > 0) {
                Network.JoinUserRoom(userNameToJoin);
            }
        });

        GameData.Container.Ui.Interface.Worlds.Elements.Add(GameData.Container.Ui.Interface.WorldUserId);

        // Custom Worlds
        foreach(var kvp in GameData.Container.Asset.Worlds) {
            // Only show debug worlds for the admin
            if(kvp.Value.IsDebug && !GameData.Transient.Player.IsAdmin) continue;

            // Instantiate the element
            WorldInfoUiElement worldInfo = UnityExtension.ExtensionInstantiate(GameData.Container.Ui.Interface.WorldInfoPrefab, 
                (WorldInfoUiElement wi) => {
                // Configure the element
                wi.name = kvp.Key;
                wi.GetComponent<RectTransform>().SetParent(GameData.Container.Ui.Interface.WorldsUi.GetComponent<RectTransform>(), true);

                wi.GetComponent<UIElement>().elementName = kvp.Key;
                wi.WorldInfoText.text = kvp.Key;

                // Element controls
                wi.GoPublicButton.OnClick.AddListener(() => {
                    Network.JoinOrCreateRoomBySceneName(kvp.Value.Scene);
                });

                wi.GoPrivateButton.OnClick.AddListener(() => {
                    Network.CreateRoomBySceneName(kvp.Value.Scene, true);
                });
            });

            GameData.Container.Ui.Interface.Worlds.Elements.Add(worldInfo.GetComponent<UIElement>());
        }

        GameData.Container.Ui.Interface.WorldsPrevButton.OnClick.AddListener(() => {
            Ui.PopUiHierarchy(GameData.Container.Ui.Interface.Worlds.GetCurrent().elementName, 
                              GameData.Container.Ui.Interface.Worlds.GetCurrent().elementCategory);
            GameData.Container.Ui.Interface.Worlds.Prev();
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Worlds.GetCurrent().elementName, 
                               GameData.Container.Ui.Interface.Worlds.GetCurrent().elementCategory);
        });

        GameData.Container.Ui.Interface.WorldsNextButton.OnClick.AddListener(() => {
            Ui.PopUiHierarchy(GameData.Container.Ui.Interface.Worlds.GetCurrent().elementName, 
                              GameData.Container.Ui.Interface.Worlds.GetCurrent().elementCategory);
            GameData.Container.Ui.Interface.Worlds.Next();
            Ui.PushUiHierarchy(GameData.Container.Ui.Interface.Worlds.GetCurrent().elementName, 
                               GameData.Container.Ui.Interface.Worlds.GetCurrent().elementCategory);
        });
    }

    private void Update() {
        // Informative texts that are updated regularyly
        GameData.Container.Ui.Interface.UserNameText.text = "User: " + GameData.Transient.Player.UserName;
        GameData.Container.Ui.Interface.LocationText.text = "Location: " + Network.RoomName;

        // Disable back button if we're in the homescreen ui
        GameData.Container.Ui.Interface.InterfaceBackButton.Interactable = !GameData.Container.Ui.Interface.InterfaceHomescreenUi.isVisible;
    }
}
