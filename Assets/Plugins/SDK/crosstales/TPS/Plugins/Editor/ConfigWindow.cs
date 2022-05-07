using UnityEditor;
using UnityEngine;

namespace Crosstales.TPS
{
    /// <summary>Editor window extension.</summary>
    [InitializeOnLoad]
    public class ConfigWindow : ConfigBase
    {

        #region Variables

        private int tab = 0;
        private int lastTab = 0;

        #endregion


        #region EditorWindow methods

        [MenuItem("Window/TPS", false, 1010)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ConfigWindow));
        }

        public void OnEnable()
        {

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            titleContent = new GUIContent("TPS", Helper.Logo_Asset_Small);
#else
			title = "TPS";
#endif

        }

        public void OnDestroy()
        {
            save();
        }

        public void OnLostFocus()
        {
            save();
        }

        public void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new string[] { "Switch", "Config", "Help", "About" });

            if (tab != lastTab)
            {
                lastTab = tab;
                GUI.FocusControl(null);
            }

            if (tab == 0)
            {
                showSwitch();
            }
            else if (tab == 1)
            {
                showConfiguration();
            }
            else if (tab == 2)
            {
                showHelp();
            }
            else
            {
                showAbout();
            }
        }

        public void OnInspectorUpdate()
        {
            Repaint();
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)