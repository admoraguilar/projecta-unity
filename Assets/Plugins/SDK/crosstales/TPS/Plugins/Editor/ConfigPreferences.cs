using UnityEditor;
using UnityEngine;

namespace Crosstales.TPS
{
    /// <summary>Unity "Preferences" extension.</summary>
    public class ConfigPreferences : ConfigBase
    {

        #region Variables

        private static int tab = 0;
        private static int lastTab = 0;

        #endregion

        #region Static methods

        [PreferenceItem("TPS")]
        private static void PreferencesGUI()
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

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)