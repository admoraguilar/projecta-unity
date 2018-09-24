using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This module makes the game's Ui management easier to do.
/// </summary>
[CreateAssetMenu(fileName = "DefaultUnityUi", menuName = "WEngine/Modules/AUi/DefaultUnityInput")]
public class DefaultUnityUi : AUi {
    public override List<T> GetUi<T>(string uiName, string uiCategory = "Uncategorized") {
        return null;
    }

    public override void SetUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void SetUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void PushUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void PushUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void PopUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void PopUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
            
    }

    public override void PushLastUi() {
            
    }

    public override void ClearUi(bool instantAction = false) {
            
    }
}


public abstract class AUi : ScriptableObject {
    public event Action<string, string> OnPushUi = delegate { };
    public event Action<string, string> OnPopUi = delegate { };

    protected void _OnPushUi(string uiName, string uiCategory) { OnPushUi(uiName, uiCategory); }
    protected void _OnPopUi(string uiName, string uiCategory) { OnPopUi(uiName, uiCategory); }

    abstract public List<T> GetUi<T>(string uiName, string uiCategory = "Uncategorized");
    abstract public void SetUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);

    /// <summary>
    /// Same as SetUi but also activates parent UiElements on top of the indicated UiElements.
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="uiCategory"></param>
    /// <param name="instantAction"></param>
    abstract public void SetUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);
    abstract public void PushUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);

    /// <summary>
    /// Same as PushUi but also activates parent UiElements on top of the indicated UiElements.
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="uiCategory"></param>
    /// <param name="instantAction"></param>
    abstract public void PushUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);
    abstract public void PopUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);

    /// <summary>
    /// Same as PopUi but also deactivates parent UiElements on top of the indicated UiElements.
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="uiCategory"></param>
    /// <param name="instantAction"></param>
    abstract public void PopUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false);

    /// <summary>
    /// Pushes the last activated Ui.
    /// </summary>
    abstract public void PushLastUi();
    abstract public void ClearUi(bool instantAction = false);
}