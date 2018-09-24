using System;
using UnityEngine;


/// <summary>
/// This module handles the Inputs of the game.
/// </summary>
[CreateAssetMenu(fileName = "DefaultUnityInput", menuName = "WEngine/Modules/IInput/DefaultUnityInput")]
public class DefaultUnityInput : AInput {
    [SerializeField] private bool enableJoystickInput = true;
    [SerializeField] private bool enableUiInput = true;
    [SerializeField] private bool enableUnityInput = true;
    [SerializeField] private bool enableInput = true;


    public override float GetAxis(string axisName, string inputName = "Default") {
        if(!enableInput) return 0f;

        float axis = 0f; 
        axis += GetUnityInputAxis(axisName);
        axis += GetUltimateJoystickAxis(inputName, axisName);

        return Mathf.Clamp(axis, -1f, 1f);
    }

    public override bool GetButton(string buttonName, string inputName = "Default") {
        if(!enableInput) return false;

        bool button = false;
        if(!button) button = GetUnityButton(buttonName);
        if(!button) button = GetUltimateButton(buttonName);

        return button;
    }

    public override bool GetButtonDown(string buttonName, string inputName = "Default") {
        if(!enableInput) return false;

        bool button = false;
        if(!button) button = GetUnityButtonDown(buttonName);
        if(!button) button = GetUltimateButtonDown(buttonName);

        return button;
    }

    public override bool GetButtonUp(string buttonName, string inputName = "Default") {
        if(!enableInput) return false;

        bool button = false;
        if(!button) button = GetUnityButtonUp(buttonName);
        if(!button) button = GetUltimateButtonUp(buttonName);

        return button;
    }

    public override bool GetKey(string keyCode, string inputName = "Default") {
        if(!enableInput) return false;

        bool key = false;
        if(!key) key = GetUnityKey((KeyCode)Enum.Parse(typeof(KeyCode), keyCode));

        return key;
    }

    public override bool GetKeyDown(string keyCode, string inputName = "Default") {
        if(!enableInput) return false;

        bool key = false;
        if(!key) key = GetUnityKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), keyCode));

        return key;
    }

    public override bool GetKeyUp(string keyCode, string inputName = "Default") {
        if(!enableInput) return false;

        bool key = false;
        if(!key) key = GetUnityKeyUp((KeyCode)Enum.Parse(typeof(KeyCode), keyCode));

        return key;
    }

    public override void EnableInput(string inputName) {
            
    }

    public override void EnableInputAll() {
        enableInput = true;
    }

    public override void DisableInput(string inputName) {
        
    }

    public override void DisableInputAll() {
        enableInput = false;
    }

    public float GetUnityInputAxis(string axisName) {
        return enableUnityInput ? Input.GetAxis(axisName) : 0f;
    }

    public bool GetUnityButton(string buttonName) {
        return enableUnityInput ? Input.GetButton(buttonName) : false;
    }

    public bool GetUnityButtonDown(string buttonName) {
        return enableUnityInput ? Input.GetButtonDown(buttonName) : false;
    }

    public bool GetUnityButtonUp(string buttonName) {
        return enableUnityInput ? Input.GetButtonUp(buttonName) : false;
    }

    public bool GetUnityKey(KeyCode keyCode) {
        return enableUnityInput ? Input.GetKey(keyCode) : false;
    }

    public bool GetUnityKeyDown(KeyCode keyCode) {
        return enableUnityInput ? Input.GetKeyDown(keyCode) : false;
    }

    public bool GetUnityKeyUp(KeyCode keyCode) {
        return enableUnityInput ? Input.GetKeyUp(keyCode) : false;
    }

    public float GetUltimateJoystickAxis(string joystickName, string axisName) {
        if(!enableJoystickInput) return 0f;

        switch(axisName) {
            case "Horizontal": return UltimateJoystick.GetHorizontalAxis(joystickName);
            case "Vertical": return UltimateJoystick.GetVerticalAxis(joystickName);
            case "Mouse X": return UltimateJoystick.GetHorizontalAxis(joystickName);
            case "Mouse Y": return UltimateJoystick.GetVerticalAxis(joystickName);
            default: return 0f;
        }
    }

    public bool GetUltimateButton(string buttonName) {
        return enableUiInput ? UltimateButton.GetButton(buttonName) : false;
    }

    public bool GetUltimateButtonDown(string buttonName) {
        return enableUiInput ? UltimateButton.GetButtonDown(buttonName) : false;
    }

    public bool GetUltimateButtonUp(string buttonName) {
        return enableUiInput ? UltimateButton.GetButtonUp(buttonName) : false;
    }
}


public abstract class AInput : ScriptableObject {
    abstract public float GetAxis(string axisName, string inputName = "Default");
    abstract public bool GetButton(string buttonName, string inputName = "Default");
    abstract public bool GetButtonDown(string buttonName, string inputName = "Default");
    abstract public bool GetButtonUp(string buttonName, string inputName = "Default");
    abstract public bool GetKey(string keyCode, string inputName = "Default");
    abstract public bool GetKeyDown(string keyCode, string inputName = "Default");
    abstract public bool GetKeyUp(string keyCode, string inputName = "Default");
    abstract public void EnableInput(string inputName);
    abstract public void EnableInputAll();
    abstract public void DisableInput(string inputName);
    abstract public void DisableInputAll();
}