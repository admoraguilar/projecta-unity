using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WEngine;
using DoozyUI;
using DoozyUI.Gestures;


public class SignInUiData : MonoBehaviour {
    [Header("Values")]
    public float ChangeBackgroundInterval;

    [Header("UI")]
    public UIElement SigninUi;

    [Header("Shared")]
    public Image BackgroundImage;
    public UIElement SignInBackgroundFader;
    public UIElement QueryResponse;
    public Text QueryResponseText;
    public InputField[] EmailAddressInputField;
    public InputField[] PasswordInputField;

    [Header("Sign-in Screen")]
    public UIElement SigninScreen;
    public UIButton SigninButton;
    public UIButton CreateNewAccountButton;

    [Header("Sign-up Screen")]
    public UIElement SignupScreen;
    public UIButton BackToSignInButton;
    public UIButton SignupButton;
    public InputField DisplayNameInputField;
    public InputField ConfirmPasswordInputField;
}