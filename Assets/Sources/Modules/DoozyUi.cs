using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;
using RSG.Scene.Query;


/// <summary>
/// DoozyUi integration for DefaultUnityUi module.
/// </summary>
[CreateAssetMenu(fileName = "DoozyUi", menuName = "WEngine/Modules/AUi/DoozyUi")]
public class DoozyUi : AUi {
    private SceneTraversal sceneTraversal = new SceneTraversal();

    public override List<T> GetUi<T>(string uiName, string uiCategory = "Uncategorized") {
        return UIManager.GetUiElements(uiName, uiCategory).Cast<T>().ToList();
    }

    public override void SetUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        ClearUi();
        PushUi(uiName, uiCategory, instantAction);
    }

    public override void SetUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        List<UIElement> uis = UIManager.GetUiElements(uiName, uiCategory).ToList();
        List<UIElement> uisToShow = new List<UIElement>();
        for(int i = 0; i < uis.Count; ++i) {
            IEnumerable<GameObject> ancestors = sceneTraversal.Ancestors(uis[i].gameObject);
            foreach(GameObject go in ancestors) {
                UIElement element = go.GetComponent<UIElement>();
                if(element) uisToShow.Add(element);
            }
            uisToShow.Add(uis[i]);
        }
        IEnumerable<UIElement> uisToHide = UIManager.GetVisibleUIElements().Except(uisToShow);

        foreach(UIElement ui in uisToHide) {
            PopUi(ui.elementName, ui.elementCategory, instantAction);
        }
        foreach(UIElement ui in uisToShow) {
            PushUi(ui.elementName, ui.elementCategory, instantAction);
        }
    }

    public override void PushUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        // Don't push already visible ui elements
        var uiElements = UIManager.GetUiElements(uiName, uiCategory).ToArray();
        foreach(var uiElement in uiElements) {
            if(uiElement.isVisible) continue;
            uiElement.Show(instantAction);
            _OnPushUi(uiName, uiCategory);
        }
    }

    public override void PushUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        List<UIElement> uis = UIManager.GetUiElements(uiName, uiCategory).ToList();
        List<UIElement> uisToPush = new List<UIElement>();
        for(int i = 0; i < uis.Count; ++i) {
            IEnumerable<GameObject> ancestors = sceneTraversal.Ancestors(uis[i].gameObject);
            foreach(GameObject go in ancestors) {
                UIElement element = go.GetComponent<UIElement>();
                if(element) uisToPush.Add(element);
            }
            uisToPush.Add(uis[i]);
        }

        foreach(UIElement ui in uisToPush) {
            PushUi(ui.elementName, ui.elementCategory, instantAction);
        }
    }

    public override void PopUi(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        // Don't pop already hidden ui elements
        var uiElements = UIManager.GetUiElements(uiName, uiCategory).ToArray();
        foreach(var uiElement in uiElements) {
            if(!uiElement.isVisible) return;
            uiElement.Hide(instantAction);
            _OnPopUi(uiName, uiCategory);
        }
    }

    public override void PopUiHierarchy(string uiName, string uiCategory = "Uncategorized", bool instantAction = false) {
        List<UIElement> uis = UIManager.GetUiElements(uiName, uiCategory).ToList();
        List<UIElement> uisToHide = new List<UIElement>();
        for(int i = 0; i < uis.Count; ++i) {
            IEnumerable<GameObject> descendants = sceneTraversal.Descendents(uis[i].gameObject);
            foreach(GameObject go in descendants) {
                UIElement element = go.GetComponent<UIElement>();
                if(element) uisToHide.Add(element);
            }
            uisToHide.Add(uis[i]);
        }

        foreach(UIElement ui in uisToHide) PopUi(ui.elementName, ui.elementCategory, instantAction);
    }

    public override void PushLastUi() {
        UIManager.BackButtonEvent();
    }

    public override void ClearUi(bool instantAction = false) {
        List<UIElement> visibleUis = UIManager.GetVisibleUIElements();
        foreach(UIElement ui in visibleUis) {
            ui.Hide(instantAction);
        }
    }
}


/// <summary>
/// Useful for UiElements that can be traversed via index.
/// </summary>
[Serializable]
public class EnumerableUiElements {
    public event Action<UIElement> OnSet = delegate { };
    public event Action<UIElement> OnPrev = delegate { };
    public event Action<UIElement> OnNext = delegate { };

    public List<UIElement> Elements { get { return elements; } }
    public int CurrentUiElementIndex { get { return currentUiElementIndex; } }

    [SerializeField] private List<UIElement> elements = new List<UIElement>();
    private int currentUiElementIndex;


    public UIElement GetCurrent() {
        return elements.Count > 0 ? elements[currentUiElementIndex] : null;
    }

    public UIElement Set(int index) {
        if(elements.Count == 0) return null;

        currentUiElementIndex = index;
        OnSet(GetCurrent());
        return elements[currentUiElementIndex];
    }

    public UIElement Prev(bool repeat = true) {
        if(elements.Count == 0) return null;

        if(currentUiElementIndex == 0) {
            currentUiElementIndex = repeat ? elements.Count - 1 : 0;
        } else {
            currentUiElementIndex--;
        }
        OnPrev(GetCurrent());
        OnSet(GetCurrent());
        return elements[currentUiElementIndex];

    }

    public UIElement Next(bool repeat = true) {
        if(elements.Count == 0) return null;

        if(currentUiElementIndex == elements.Count - 1) {
            currentUiElementIndex = repeat ? 0 : elements.Count - 1;
        } else {
            currentUiElementIndex++;
        }  
        OnNext(GetCurrent());
        OnSet(GetCurrent());
        return elements[currentUiElementIndex];
    }
}