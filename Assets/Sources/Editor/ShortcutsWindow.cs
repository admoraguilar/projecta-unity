using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;


public class ShortcutsWindow : OdinEditorWindow {
    public MonoScript ComponentToAttach;
    public GameObjectShortcut[] GameObjectShortcuts;

    [PropertyOrder(-10)]
    [VerticalGroup()]
    [Button(ButtonSizes.Small)]
    public void AttachComponentToSelectedGameObject() {
        GameObject selectedGo = Selection.activeGameObject;
        if(selectedGo != null) {
            Debug.Log(ComponentToAttach.GetClass());
            if(selectedGo.GetComponent(ComponentToAttach.GetClass()) == null) {
                selectedGo.AddComponent(ComponentToAttach.GetClass());
            }
        }
    }


    [MenuItem("Tools/Open Shortcuts Window")]
    private static void OpenWindow() {
        ShortcutsWindowData data = Resources.LoadAll<ShortcutsWindowData>("").FirstOrDefault();

        // Load persistent data
        ShortcutsWindow window = GetWindow<ShortcutsWindow>();
        window.GameObjectShortcuts = data.GameObjectShortcuts;
        window.Show();

        // Persist changes
        window.OnClose += () => {
            data.GameObjectShortcuts = window.GameObjectShortcuts;
        };
    }
}
