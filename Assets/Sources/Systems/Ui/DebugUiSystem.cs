using System.Collections;
using UnityEngine;
using WEngine;


/// <summary>
/// Responsible for displaying Debug Uis
/// </summary>
public class DebugUiSystem : MonoBehaviour {
    [Inject] public AGameData GameData;
    [Inject] public AInput Input;

    private Reporter thisReporter;

    private void EnableDebug(bool enable) {
        // Enable DebugUi
        GameData.Container.Ui.Debug.DebugUi.SetActive(enable);
        Debug.Log("Debug mode: " + enable.ToString());
    }

    private IEnumerator VerifyIfAdmin() {
        while(!GameData.Transient.Player.IsAdmin) {
            // Only display debug when logged in user is an admin
            foreach(string adminId in GameData.Key.Admin) {
                if(adminId == GameData.Transient.Player.UserName) {
                    GameData.Transient.Player.IsAdmin = true;
                    break;
                }
            }
            yield return null;
        }

        Debug.Log("Developer mode access granted!");
        EnableDebug(true);
    }

    private void Start() {
        // Set debug module references
        thisReporter = Main.GetGameModule<Reporter>();

        // Set debug mode false at start
        EnableDebug(false);

        StartCoroutine(VerifyIfAdmin());
    }

    private void Update() {
        // Show reporter
        if(Input.GetButtonDown("Debug")) {
            thisReporter.doShow();
        }
    }
}
