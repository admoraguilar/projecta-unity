using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEngine;
using DoozyUI;


public class UiTester : MonoBehaviour {
    [SerializeField] private UIElement uiToDisplay;

    [Inject] public AUi Ui;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            Ui.PushUiHierarchy(uiToDisplay.elementName, uiToDisplay.elementCategory);
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            Ui.SetUiHierarchy(uiToDisplay.elementName, uiToDisplay.elementCategory);
        }

        if(Input.GetKeyDown(KeyCode.T)) {
            Ui.ClearUi();
        }
	}
}
