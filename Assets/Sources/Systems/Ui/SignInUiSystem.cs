using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WEngine;


/// <summary>
/// Handles the SignInUi:
/// * SignIn process.
/// * Rotating background.
/// </summary>
public class SignInUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public ABackend Backend;
    [Inject] public AConnectionChecker ConnectionChecker;
    [Inject] public AUi Ui;

    private Coroutine queryResponse;
    private string tempPassword;
    private float changeBackgroundTime;
    private int currentBackgroundIndex;


    private void SetInputsInteractable(bool value) {
        foreach(InputField inputField in GameData.Container.Ui.SignIn.EmailAddressInputField) {
            inputField.interactable = value;
        }

        foreach(InputField inputField in GameData.Container.Ui.SignIn.PasswordInputField) {
            inputField.interactable = value;
        }

        GameData.Container.Ui.SignIn.SigninButton.Interactable = value;
        GameData.Container.Ui.SignIn.CreateNewAccountButton.Interactable = value;
        GameData.Container.Ui.SignIn.BackToSignInButton.Interactable = value;
        GameData.Container.Ui.SignIn.SignupButton.Interactable = value;
        GameData.Container.Ui.SignIn.DisplayNameInputField.interactable = value;
        GameData.Container.Ui.SignIn.ConfirmPasswordInputField.interactable = value;
    }

    private void ResetInputs() {
        // Reset inputs
        foreach(InputField inputField in GameData.Container.Ui.SignIn.EmailAddressInputField) {
            inputField.text = "";
        }

        foreach(InputField inputField in GameData.Container.Ui.SignIn.PasswordInputField) {
            inputField.text = "";
        }

        // Reset data
        GameData.Transient.Player.UserName = "";
        GameData.Transient.Player.Password = "";
    }

    private IEnumerator ProcessSignin(bool newUser) {
        // Disable sign in buttons
        SetInputsInteractable(false);

        // Check connections
        if(!ConnectionChecker.IsInternetOk) {
            yield return queryResponse = StartCoroutine(QueryResponse("Internet connection failed. Please check your network settings.", 1f));
            SetInputsInteractable(true);
            yield break;
        }

        if(!Backend.IsAvailable) {
            yield return queryResponse = StartCoroutine(QueryResponse("Still trying to connect to the servers, please try again in some seconds.", 1f));
            SetInputsInteractable(true);
            yield break;
        }

        // Check email
        if(!Utilities.IsEmailValid(GameData.Transient.Player.UserName)) {
            yield return queryResponse = StartCoroutine(QueryResponse("Invalid email.", 1f));
            SetInputsInteractable(true);
            yield break;
        }

        // If new user then sign-up instead
        if(newUser) {
            if(GameData.Transient.Player.Password != tempPassword) {
                yield return queryResponse = StartCoroutine(QueryResponse("Passwords have a mismatch, please re-type your password again.", 1f));
                SetInputsInteractable(true);
                yield break;
            } else {
                Backend.Register();
                yield return new WaitWhile(() => Backend.IsRegistering);
                if(Backend.IsRegistered) {
                    yield return queryResponse = StartCoroutine(QueryResponse("Sign-up successful!", 1f));
                } else {
                    yield return queryResponse = StartCoroutine(QueryResponse("Sign-up unsuccessful!", 1f));
                    SetInputsInteractable(true);
                    yield break;
                }
            }
        }

        // Check if account exists
        // We do a check if it's not authenticated so that we don't re-authenticate
        // again once a register is successful
        if(!Backend.IsAuthenticated) {
            Backend.Authenticate();
            yield return queryResponse = StartCoroutine(QueryResponse("Checking account details.", 1f));
            yield return new WaitWhile(() => Backend.IsAuthenticating);
        }

        if(Backend.IsAuthenticated) {
            yield return queryResponse = StartCoroutine(QueryResponse("Sign-in successful!", 1f));
            yield return queryResponse = StartCoroutine(QueryResponse("Connecting to the server.", 100f));
        } else {
            yield return queryResponse = StartCoroutine(QueryResponse("Invalid email or password.", 1f));
            SetInputsInteractable(true);
            yield break;
        }
    }

    private IEnumerator ChangeBackground() {
        while(true) {
            // Hide the last query that we displayed
            if(queryResponse == null) {
                Ui.PopUi(GameData.Container.Ui.SignIn.QueryResponse.elementName, GameData.Container.Ui.SignIn.QueryResponse.elementCategory);
            }

            // Run the fader
            Ui.PushUi(GameData.Container.Ui.SignIn.SignInBackgroundFader.elementName, GameData.Container.Ui.SignIn.SignInBackgroundFader.elementCategory);
           
            // Let's wait until the fader is fully faded in
            bool fadeInDone = false;
            UnityAction onFadeInDone = () => {
                fadeInDone = true;
            };
            GameData.Container.Ui.SignIn.SignInBackgroundFader.OnInAnimationsFinish.AddListener(onFadeInDone);
            yield return new WaitUntil(() => fadeInDone);

            // Change background and query info
            SignInBackgroundData signInBackgroundData = GameData.Container.Asset.SignInBackgrounds[currentBackgroundIndex];
            currentBackgroundIndex = currentBackgroundIndex >= GameData.Container.Asset.SignInBackgrounds.Count - 1 ? 0 : currentBackgroundIndex + 1;

            GameData.Container.Ui.SignIn.BackgroundImage.sprite = signInBackgroundData.Background;
            
            // Do not change the query info if an actual query is being displayed
            // this is done so that we don't interfere with more important
            // info
            if(queryResponse == null && signInBackgroundData.Info.Length > 0) {
                Ui.PushUi(GameData.Container.Ui.SignIn.QueryResponse.elementName, GameData.Container.Ui.SignIn.QueryResponse.elementCategory);
                GameData.Container.Ui.SignIn.QueryResponseText.text = signInBackgroundData.Info;
            }

            // Fade out
            Ui.PopUi(GameData.Container.Ui.SignIn.SignInBackgroundFader.elementName, GameData.Container.Ui.SignIn.SignInBackgroundFader.elementCategory);

            // Remove the fader listener
            GameData.Container.Ui.SignIn.SignInBackgroundFader.OnInAnimationsFinish.RemoveListener(onFadeInDone);

            // Change background with an interval
            yield return new WaitForSeconds(GameData.Container.Ui.SignIn.ChangeBackgroundInterval);
        }
    }

    private IEnumerator QueryResponse(string message, float seconds) {
        Ui.PushUiHierarchy(GameData.Container.Ui.SignIn.QueryResponse.elementName, GameData.Container.Ui.SignIn.QueryResponse.elementCategory);
        GameData.Container.Ui.SignIn.QueryResponseText.text = message;
        yield return new WaitForSeconds(seconds);
        Ui.PopUiHierarchy(GameData.Container.Ui.SignIn.QueryResponse.elementName, GameData.Container.Ui.SignIn.QueryResponse.elementCategory);
        queryResponse = null;
    }

    private void InitUi() {
        // Show first screen
        Ui.PushUiHierarchy(GameData.Container.Ui.SignIn.SigninScreen.elementName, GameData.Container.Ui.SignIn.SigninScreen.elementCategory);

        // Loop background changer
        StartCoroutine(ChangeBackground());
    }

    private void DeinitializeUi() {
        StopAllCoroutines();
    }

    private void Start() {
        // Hook handlers
        GameData.Container.Ui.SignIn.SigninUi.OnInAnimationsStart.AddListener(InitUi);
        GameData.Container.Ui.SignIn.SigninUi.OnOutAnimationsFinish.AddListener(DeinitializeUi);

        // Hook elements
        // Shared
        foreach(InputField inputField in GameData.Container.Ui.SignIn.EmailAddressInputField) {
            inputField.onValueChanged.AddListener((string value) => {
                GameData.Transient.Player.UserName = value;
            });
        }

        foreach(InputField inputField in GameData.Container.Ui.SignIn.PasswordInputField) {
            inputField.onValueChanged.AddListener((string value) => {
                GameData.Transient.Player.Password = value;
            });
        }

        // Sign-in screen
        GameData.Container.Ui.SignIn.CreateNewAccountButton.OnPointerDown.AddListener(() => {
            Ui.SetUiHierarchy(GameData.Container.Ui.SignIn.SignupScreen.elementName, GameData.Container.Ui.SignIn.SignupScreen.elementCategory);
            ResetInputs();
        });

        GameData.Container.Ui.SignIn.SigninButton.OnPointerDown.AddListener(() => {
            StartCoroutine(ProcessSignin(false));
        });

        // Sign-up screen
        GameData.Container.Ui.SignIn.BackToSignInButton.OnPointerDown.AddListener(() => {
            Ui.SetUiHierarchy(GameData.Container.Ui.SignIn.SigninScreen.elementName, GameData.Container.Ui.SignIn.SigninScreen.elementCategory);
            ResetInputs();
        });

        GameData.Container.Ui.SignIn.SignupButton.OnPointerDown.AddListener(() => {
            StartCoroutine(ProcessSignin(true));
        });

        GameData.Container.Ui.SignIn.DisplayNameInputField.onValueChanged.AddListener((value) => {
            GameData.Transient.Player.DisplayName = value;
        });

        GameData.Container.Ui.SignIn.ConfirmPasswordInputField.onValueChanged.AddListener((value) => {
            tempPassword = value;
        });
    }
}
