using UnityEngine;
using UnityEditor;

namespace Crosstales.TPS
{
    /// <summary>Base class for editor windows.</summary>
    public abstract class ConfigBase : EditorWindow
    {

        #region Variables

        protected static string updateText = UpdateCheck.TEXT_NOT_CHECKED;

        private static System.Threading.Thread worker;

        private static Vector2 scrollPosSwitch;
        private static Vector2 scrollPosPlatforms;
        private static Vector2 scrollPosConfig;
        private static Vector2 scrollPosAbout;
        private static Vector2 scrollPosHelp;

        private const int rowWidth = 10000;
        private static int rowHeight = 0; //later = 36
        private static int rowCounter = 0;

        private const int logoWidth = 36;
        private const int platformWidth = 120;
        private const int architectureWidth = 65;
        private const int textureWidth = 70;
        private const int cacheWidth = 40;
        private const int actionWidth = 50;

        private static int platformX = 0;
        private static int platformY = 0;
        private static int platformTextSpace = 12; //later = 18

        private static int cacheTextSpace = 6; //later = 18

        private static int actionTextSpace = 6; //later = 8

        private static string[] vcsOptions = { "None", "git", "SVN", "Mercurial" };

        private static string[] archWinOptions = { "32bit", "64bit" };
        private static string[] archMacOptions = { "32bit", "64bit", "Universal" };
        private static string[] archLinuxOptions = { "32bit", "64bit", "Universal" };

        private static string[] texAndroidOptions = { "Generic", "DXT", "PVRTC", "ATC", "ETC", "ETC2", "ASTC" };

        #endregion

        #region Properties

        private static string buildWindows
        {
            get
            {
                if (Constants.ARCH_WINDOWS == 0)
                {
                    return "win32";
                }

                return "win64";
            }
        }

        private static string buildMac
        {
            get
            {
                if (Constants.ARCH_MAC == 0)
                {
                    return "osx";
                }
                else if (Constants.ARCH_MAC == 1)
                {
                    return "osxintel64";
                }

                return "osxuniversal";
            }
        }

        private static string buildLinux
        {
            get
            {
                if (Constants.ARCH_LINUX == 0)
                {
                    return "linux";
                }
                else if (Constants.ARCH_LINUX == 1)
                {
                    return "linux64";
                }

                return "linuxuniversal";
            }
        }

        private static BuildTarget targetWindows
        {
            get
            {
                if (Constants.ARCH_WINDOWS == 0)
                {
                    return BuildTarget.StandaloneWindows;
                }

                return BuildTarget.StandaloneWindows64;
            }
        }

        private static BuildTarget targetMac
        {
            get
            {
                //if (Constants.ARCH_MAC == 0)
                //{
                //    return BuildTarget.StandaloneOSXIntel;
                //}
                //else if (Constants.ARCH_MAC == 1)
                //{
                //    return BuildTarget.StandaloneOSXIntel64;
                //}

                return BuildTarget.StandaloneOSX;
            }
        }

        private static BuildTarget targetLinux
        {
            get
            {
                if (Constants.ARCH_LINUX == 0)
                {
                    return BuildTarget.StandaloneLinux;
                }
                else if (Constants.ARCH_LINUX == 1)
                {
                    return BuildTarget.StandaloneLinux64;
                }

                return BuildTarget.StandaloneLinuxUniversal;
            }
        }

#if UNITY_5
        private static MobileTextureSubtarget texAndroid
        {
            get
            {
                if (Constants.TEX_ANDROID == 1)
                {
                    return MobileTextureSubtarget.DXT;
                }
                else if (Constants.TEX_ANDROID == 2)
                {
                    return MobileTextureSubtarget.PVRTC;
                }
                else if (Constants.TEX_ANDROID == 3)
                {
                    return MobileTextureSubtarget.ATC;
                }
                else if (Constants.TEX_ANDROID == 4)
                {
                    return MobileTextureSubtarget.ETC;
                }
                else if (Constants.TEX_ANDROID == 5)
                {
                    return MobileTextureSubtarget.ETC2;
                }
                else if (Constants.TEX_ANDROID == 6)
                {
                    return MobileTextureSubtarget.ASTC;
                }

                return MobileTextureSubtarget.Generic;
            }
        }
#else
		private static MobileTextureSubtarget texAndroid
		{
			get
			{
				if (Constants.TEX_ANDROID == 1)
				{
					return MobileTextureSubtarget.DXT;
				}
				else if (Constants.TEX_ANDROID == 2)
				{
					return MobileTextureSubtarget.PVRTC;
				}
				else if (Constants.TEX_ANDROID == 3)
				{
					return MobileTextureSubtarget.ETC;
				}
				else if (Constants.TEX_ANDROID == 4)
				{
					return MobileTextureSubtarget.ETC;

				}
				
				return MobileTextureSubtarget.Generic;
			}
		}

#endif

        #endregion


        #region Protected methods

        protected static void showSwitch()
        {
            if (Helper.isEditorMode)
            {
                platformX = 0;
                platformY = 0;
                platformTextSpace = 12; //later = 18

                rowHeight = 0; //later = 36
                rowCounter = 0;

                cacheTextSpace = 6; //later = 18
                actionTextSpace = 6; //later = 8

                // header
                drawHeader();

                // scrollPosSwitch = EditorGUILayout.BeginScrollView(scrollPosSwitch, false, false);
                // {
                //header
                // drawHeader();
                // }

                scrollPosSwitch = EditorGUILayout.BeginScrollView(scrollPosSwitch, false, false);
                {
                    //content
                    drawContent();
                }
                EditorGUILayout.EndScrollView();

                Helper.SeparatorUI();

                Constants.SHOW_DELETE = EditorGUILayout.Toggle(new GUIContent("Show Delete Buttons", "Shows or hides the delete button for the cache."), Constants.SHOW_DELETE);

                GUILayout.Space(6);
            }
            else
            {
                GUILayout.Label("Disabled in Play-mode!");
            }
        }

        protected static void showConfiguration()
        {
            scrollPosPlatforms = EditorGUILayout.BeginScrollView(scrollPosPlatforms, false, false);
            {
                GUILayout.Label("General Settings", EditorStyles.boldLabel);
                Constants.CUSTOM_PATH_CACHE = EditorGUILayout.BeginToggleGroup(new GUIContent("Custom Cache Path", "Enable or disable a custom cache path (default: " + Constants.DEFAULT_CUSTOM_PATH_CACHE + ")."), Constants.CUSTOM_PATH_CACHE);
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.SelectableLabel(Constants.PATH_CACHE);

                        if (GUILayout.Button(new GUIContent("Select", Helper.Icon_Folder, "Select path for the cache")))
                        {
                            string path = EditorUtility.OpenFolderPanel("Select path for the cache", Constants.PATH_CACHE.Substring(0, Constants.PATH_CACHE.Length - (Constants.CACHE_DIRNAME.Length + 1)), "");
                            //string path = EditorUtility.OpenFolderPanel("Select path for the cache", "", "");

                            if (!string.IsNullOrEmpty(path))
                            {
                                Constants.PATH_CACHE = path + (Helper.isWindowsPlatform ? "\\" : "/") + Constants.CACHE_DIRNAME;
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndToggleGroup();

                if (Constants.CUSTOM_PATH_CACHE)
                {
                    GUI.enabled = false;
                }
                Constants.VCS = EditorGUILayout.Popup("Version Control", Constants.VCS, vcsOptions);
                GUI.enabled = true;

                Constants.EXECUTE_METHOD = EditorGUILayout.TextField(new GUIContent("Execute Method", "Execute static method <ClassName.MethodName> in Unity after a switch (default: empty)."), Constants.EXECUTE_METHOD);

                Constants.COPY_SETTINGS = EditorGUILayout.Toggle(new GUIContent("Copy Settings", "Enable or disable copying the 'ProjectSettings'-folder (default: " + Constants.DEFAULT_COPY_SETTINGS + ")."), Constants.COPY_SETTINGS);

                //Constants.DEBUG = EditorGUILayout.Toggle(new GUIContent("Debug", "Enable or disable debug logs (default: " + Constants.DEFAULT_DEBUG + ")."), Constants.DEBUG);

                Constants.UPDATE_CHECK = EditorGUILayout.BeginToggleGroup(new GUIContent("Update Check", "Enable or disable the update-check (default: " + Constants.DEFAULT_UPDATE_CHECK + ")."), Constants.UPDATE_CHECK);
                {
                    EditorGUI.indentLevel++;
                    Constants.UPDATE_OPEN_UAS = EditorGUILayout.Toggle(new GUIContent("Open UAS-site", "Automatically opens the direct link to 'Unity AssetStore' if an update was found (default: " + Constants.DEFAULT_UPDATE_OPEN_UAS + ")."), Constants.UPDATE_OPEN_UAS);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndToggleGroup();

                Helper.SeparatorUI();

                GUILayout.Label("UI Settings", EditorStyles.boldLabel);
                Constants.CONFIRM_SWITCH = EditorGUILayout.Toggle(new GUIContent("Confirm Switch", "Enable or disable the switch confirmation dialog (default: " + Constants.DEFAULT_CONFIRM_SWITCH + ")."), Constants.CONFIRM_SWITCH);
                Constants.SHOW_COLUMN_PLATFORM = EditorGUILayout.Toggle(new GUIContent("Column: Platform", "Enable or disable the column 'Platform' in the 'Switch'-tab (default: " + Constants.DEFAULT_SHOW_COLUMN_PLATFORM + ")."), Constants.SHOW_COLUMN_PLATFORM);
                Constants.SHOW_COLUMN_ARCHITECTURE = EditorGUILayout.Toggle(new GUIContent("Column: Arch", "Enable or disable the column 'Arch' in the 'Switch'-tab (default: " + Constants.DEFAULT_SHOW_COLUMN_ARCHITECTURE + ")."), Constants.SHOW_COLUMN_ARCHITECTURE);
                Constants.SHOW_COLUMN_TEXTURE = EditorGUILayout.Toggle(new GUIContent("Column: Texture", "Enable or disable the column 'Texture' in the 'Switch'-tab (default: " + Constants.DEFAULT_SHOW_COLUMN_TEXTURE + ")."), Constants.SHOW_COLUMN_TEXTURE);
                Constants.SHOW_COLUMN_CACHE = EditorGUILayout.Toggle(new GUIContent("Column: Cache", "Enable or disable the column 'Cache' in the 'Switch'-tab (default: " + Constants.DEFAULT_SHOW_COLUMN_CACHE + ")."), Constants.SHOW_COLUMN_CACHE);

                Helper.SeparatorUI();

                GUILayout.Label("Active Platforms", EditorStyles.boldLabel);

                //scrollPosPlatforms = EditorGUILayout.BeginScrollView(scrollPosPlatforms, false, false);
                Constants.PLATFORM_WINDOWS = EditorGUILayout.Toggle(new GUIContent("Windows", "Enable or disable the support for the Windows platform (default: " + Constants.DEFAULT_PLATFORM_WINDOWS + ")."), Constants.PLATFORM_WINDOWS);
                Constants.PLATFORM_MAC = EditorGUILayout.Toggle(new GUIContent("Mac", "Enable or disable the support for the macOS platform (default: " + Constants.DEFAULT_PLATFORM_MAC + ")."), Constants.PLATFORM_MAC);
                Constants.PLATFORM_LINUX = EditorGUILayout.Toggle(new GUIContent("Linux", "Enable or disable the support for the Linux platform (default: " + Constants.DEFAULT_PLATFORM_LINUX + ")."), Constants.PLATFORM_LINUX);
                Constants.PLATFORM_ANDROID = EditorGUILayout.Toggle(new GUIContent("Android", "Enable or disable the support for the Android platform (default: " + Constants.DEFAULT_PLATFORM_ANDROID + ")."), Constants.PLATFORM_ANDROID);
                Constants.PLATFORM_IOS = EditorGUILayout.Toggle(new GUIContent("iOS", "Enable or disable the support for the iOS platform (default: " + Constants.DEFAULT_PLATFORM_IOS + ")."), Constants.PLATFORM_IOS);

#if UNITY_5
                Constants.PLATFORM_WSA = EditorGUILayout.Toggle(new GUIContent("WSA", "Enable or disable the support for the WSA platform (default: " + Constants.DEFAULT_PLATFORM_WSA + ")."), Constants.PLATFORM_WSA);
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                Constants.PLATFORM_WEBPLAYER = EditorGUILayout.Toggle(new GUIContent("WebPlayer", "Enable or disable the support for the WebPlayer platform (default: " + Constants.DEFAULT_PLATFORM_WEBPLAYER + ")."), Constants.PLATFORM_WEBPLAYER);
#endif

#if UNITY_5
                Constants.PLATFORM_WEBGL = EditorGUILayout.Toggle(new GUIContent("WebGL", "Enable or disable the support for the WebGL platform (default: " + Constants.DEFAULT_PLATFORM_WEBGL + ")."), Constants.PLATFORM_WEBGL);
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                Constants.PLATFORM_TVOS = EditorGUILayout.Toggle(new GUIContent("tvOS", "Enable or disable the support for the tvOS platform (default: " + Constants.DEFAULT_PLATFORM_TVOS + ")."), Constants.PLATFORM_TVOS);
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                Constants.PLATFORM_TIZEN = EditorGUILayout.Toggle(new GUIContent("Tizen", "Enable or disable the support for the Tizen platform (default: " + Constants.DEFAULT_PLATFORM_TIZEN + ")."), Constants.PLATFORM_TIZEN);
#endif
                Constants.PLATFORM_SAMSUNGTV = EditorGUILayout.Toggle(new GUIContent("SamsungTV", "Enable or disable the support for the SamsungTV platform (default: " + Constants.DEFAULT_PLATFORM_SAMSUNGTV + ")."), Constants.PLATFORM_SAMSUNGTV);

#if !UNITY_5_5_OR_NEWER
                Constants.PLATFORM_PS3 = EditorGUILayout.Toggle(new GUIContent("PS3", "Enable or disable the support for the Sony PS3 platform (default: " + Constants.DEFAULT_PLATFORM_PS3 + ")."), Constants.PLATFORM_PS3);
#endif

                Constants.PLATFORM_PS4 = EditorGUILayout.Toggle(new GUIContent("PS4", "Enable or disable the support for the Sony PS4 platform (default: " + Constants.DEFAULT_PLATFORM_PS4 + ")."), Constants.PLATFORM_PS4);
                Constants.PLATFORM_PSP2 = EditorGUILayout.Toggle(new GUIContent("PSP2 (Vita)", "Enable or disable the support for the Sony PSP2 (Vita) platform (default: " + Constants.DEFAULT_PLATFORM_PSP2 + ")."), Constants.PLATFORM_PSP2);

#if !UNITY_5_5_OR_NEWER
                Constants.PLATFORM_XBOX360 = EditorGUILayout.Toggle(new GUIContent("XBox360", "Enable or disable the support for the Microsoft XBox360 platform (default: " + Constants.DEFAULT_PLATFORM_XBOX360 + ")."), Constants.PLATFORM_XBOX360);
#endif

                Constants.PLATFORM_XBOXONE = EditorGUILayout.Toggle(new GUIContent("XBoxOne", "Enable or disable the support for the Microsoft XBoxOne platform (default: " + Constants.DEFAULT_PLATFORM_XBOXONE + ")."), Constants.PLATFORM_XBOXONE);

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                Constants.PLATFORM_WIIU = EditorGUILayout.Toggle(new GUIContent("WiiU", "Enable or disable the support for the Nintendo WiiU platform (default: " + Constants.DEFAULT_PLATFORM_WIIU + ")."), Constants.PLATFORM_WIIU);
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                Constants.PLATFORM_3DS = EditorGUILayout.Toggle(new GUIContent("3DS", "Enable or disable the support for the Nintendo 3DS platform (default: " + Constants.DEFAULT_PLATFORM_3DS + ")."), Constants.PLATFORM_3DS);
#endif

#if UNITY_5_6_OR_NEWER
			Constants.PLATFORM_SWITCH = EditorGUILayout.Toggle(new GUIContent("Switch", "Enable or disable the support for the Nintendo Switch platform (default: " + Constants.DEFAULT_PLATFORM_SWITCH + ")."), Constants.PLATFORM_SWITCH);
#endif
            }
            EditorGUILayout.EndScrollView();

            Helper.SeparatorUI();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent("Delete Cache", Helper.Icon_Delete, "Delete the complete cache")))
                {
                    if (EditorUtility.DisplayDialog("Delete the complete cache?", "If you delete the complete cache, Unity must re-import all assets for every platform switch." + System.Environment.NewLine + "This operation could take some time." + System.Environment.NewLine + System.Environment.NewLine + "Would you like to delete the cache?", "Yes", "No"))
                    {
                        if (Constants.DEBUG)
                            Debug.Log("Complete cache deleted");

                        Helper.DeleteCache();
                    }
                }

                if (GUILayout.Button(new GUIContent("Reset", Helper.Icon_Reset, "Resets the configuration settings for this project.")))
                {
                    if (EditorUtility.DisplayDialog("Reset configuration?", "Reset the configuration of " + Constants.ASSET_NAME + "?", "Yes", "No"))
                    {
                        Constants.Reset();
                        save();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(6);

            //            GUILayout.Label("Cache", EditorStyles.boldLabel);
            //
            //            if (GUILayout.Button(new GUIContent("Delete Cache", "Delete the complete cache")))
            //            {
            //                if (EditorUtility.DisplayDialog("Delete the complete cache?", "If you delete the complete cache, Unity must re-import all assets for every platform switch." + System.Environment.NewLine + "This operation could take some time." + System.Environment.NewLine + System.Environment.NewLine + "Would you like to delete the cache?", "Yes", "No"))
            //                {
            //                    if (Constants.DEBUG)
            //                        Debug.Log("Complete cache deleted");
            //
            //                    Helper.DeleteCache();
            //                }
            //            }
        }

        protected static void showHelp()
        {
            scrollPosHelp = EditorGUILayout.BeginScrollView(scrollPosHelp, false, false);
            {
                GUILayout.Label("Resources", EditorStyles.boldLabel);

                //GUILayout.Space(8);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {

                        if (GUILayout.Button(new GUIContent("Manual", Helper.Icon_Manual, "Show the manual.")))
                        {
                            Application.OpenURL(Constants.ASSET_MANUAL_URL);
                        }

                        GUILayout.Space(6);

                        if (GUILayout.Button(new GUIContent("Forum", Helper.Icon_Forum, "Visit the forum page.")))
                        {
                            Application.OpenURL(Constants.ASSET_FORUM_URL);
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {

                        if (GUILayout.Button(new GUIContent("API", Helper.Icon_API, "Show the API.")))
                        {
                            Application.OpenURL(Constants.ASSET_API_URL);
                        }

                        GUILayout.Space(6);

                        if (GUILayout.Button(new GUIContent("Product", Helper.Icon_Product, "Visit the product page.")))
                        {
                            Application.OpenURL(Constants.ASSET_WEB_URL);
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(6);
        }

        protected static void showAbout()
        {
            scrollPosAbout = EditorGUILayout.BeginScrollView(scrollPosAbout, false, false);
            {
                GUILayout.Label(Constants.ASSET_NAME, EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical(GUILayout.Width(60));
                    {
                        GUILayout.Label("Version:");

                        GUILayout.Space(12);

                        GUILayout.Label("Web:");

                        GUILayout.Space(2);

                        GUILayout.Label("Email:");

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(170));
                    {
                        GUILayout.Space(0);

                        GUILayout.Label(Constants.ASSET_VERSION);

                        GUILayout.Space(12);

                        EditorGUILayout.SelectableLabel(Constants.ASSET_AUTHOR_URL, GUILayout.Height(16), GUILayout.ExpandHeight(false));

                        GUILayout.Space(2);

                        EditorGUILayout.SelectableLabel(Constants.ASSET_CONTACT, GUILayout.Height(16), GUILayout.ExpandHeight(false));
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                    {
                        //GUILayout.Space(0);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(GUILayout.Width(64));
                    {
                        //GUILayout.Label(logo_asset, GUILayout.Height(80));

                        if (GUILayout.Button(new GUIContent(string.Empty, Helper.Logo_Asset, "Visit asset website")))
                        {
                            Application.OpenURL(Constants.ASSET_URL);
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();


                //GUILayout.Space(12);
                GUILayout.Label("© 2016-2017 by " + Constants.ASSET_AUTHOR);

                Helper.SeparatorUI();

                if (worker == null || (worker != null && !worker.IsAlive))
                {
                    if (GUILayout.Button(new GUIContent("Check For Update", Helper.Icon_Check, "Checks for available updates of " + Constants.ASSET_NAME)))
                    {

                        worker = new System.Threading.Thread(() => UpdateCheck.UpdateCheckForEditor(out updateText));
                        worker.Start();
                    }
                }
                else
                {
                    GUILayout.Label("Checking... Please wait.", EditorStyles.boldLabel);
                }

                Color fgColor = GUI.color;

                if (updateText.Equals(UpdateCheck.TEXT_NOT_CHECKED))
                {
                    GUI.color = Color.cyan;
                    GUILayout.Label(updateText);
                }
                else if (updateText.Equals(UpdateCheck.TEXT_NO_UPDATE))
                {
                    GUI.color = Color.green;
                    GUILayout.Label(updateText);
                }
                else
                {
                    GUI.color = Color.yellow;
                    GUILayout.Label(updateText);

                    if (GUILayout.Button(new GUIContent("Download", "Visit the 'Unity AssetStore' to download the latest version.")))
                    {
                        Application.OpenURL(Constants.ASSET_URL);
                    }
                }

                GUI.color = fgColor;
            }
            EditorGUILayout.EndScrollView();
            Helper.SeparatorUI();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(new GUIContent("AssetStore", Helper.Logo_Unity, "Visit the 'Unity AssetStore' website.")))
                {
                    Application.OpenURL(Constants.ASSET_CT_URL);
                }

                if (GUILayout.Button(new GUIContent(Constants.ASSET_AUTHOR, Helper.Logo_CT, "Visit the '" + Constants.ASSET_AUTHOR + "' website.")))
                {
                    Application.OpenURL(Constants.ASSET_AUTHOR_URL);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(6);
        }

        protected static void save()
        {
            Constants.Save();

            if (Constants.DEBUG)
                Debug.Log("Config data saved");
        }

        #endregion

        #region Private methods

        private static void drawColumnZebra()
        {
            if (Constants.PLATFORM_WINDOWS)
            {
                drawZebra(targetWindows);
            }

            if (Constants.PLATFORM_MAC)
            {
                drawZebra(targetMac);
            }

            if (Constants.PLATFORM_LINUX)
            {
                drawZebra(targetLinux);
            }

            if (Constants.PLATFORM_ANDROID)
            {
                drawZebra(BuildTarget.Android);
            }

            if (Constants.PLATFORM_IOS)
            {
#if UNITY_5
                drawZebra(BuildTarget.iOS);
#else
                drawZebra(BuildTarget.iOS);
#endif
            }

#if UNITY_5
            if (Constants.PLATFORM_WSA)
            {
                drawZebra(BuildTarget.WSAPlayer);
            }
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
            if (Constants.PLATFORM_WEBPLAYER)
            {
                drawZebra(BuildTarget.WebPlayer);
            }
#endif

#if UNITY_5
            if (Constants.PLATFORM_WEBGL)
            {
                drawZebra(BuildTarget.WebGL);
            }
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
            if (Constants.PLATFORM_TVOS)
            {
                drawZebra(BuildTarget.tvOS);
            }
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            if (Constants.PLATFORM_TIZEN)
            {
                drawZebra(BuildTarget.Tizen);
            }
#endif

            //if (Constants.PLATFORM_SAMSUNGTV)
            //{
            //    drawZebra(BuildTarget.SamsungTV);
            //}

#if !UNITY_5_5_OR_NEWER
            if (Constants.PLATFORM_PS3)
            {
                drawZebra(BuildTarget.PS3);
            }
#endif

            if (Constants.PLATFORM_PS4)
            {
                drawZebra(BuildTarget.PS4);
            }

            if (Constants.PLATFORM_PSP2)
            {
                drawZebra(BuildTarget.PSP2);
            }

#if !UNITY_5_5_OR_NEWER
            if (Constants.PLATFORM_XBOX360)
            {
                drawZebra(BuildTarget.XBOX360);
            }
#endif

            if (Constants.PLATFORM_XBOXONE)
            {
                drawZebra(BuildTarget.XboxOne);
            }

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            if (Constants.PLATFORM_WIIU)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                drawZebra(BuildTarget.WiiU);
#pragma warning restore CS0618 // Type or member is obsolete
            }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
            if (Constants.PLATFORM_3DS)
            {
#if UNITY_5_5_OR_NEWER
                drawZebra(BuildTarget.N3DS);
#else
                drawZebra(BuildTarget.Nintendo3DS);
#endif
            }
#endif


#if UNITY_5_6_OR_NEWER
            if (Constants.PLATFORM_SWITCH)
            {
                drawZebra(BuildTarget.Switch);
            }
#endif

        }

        private static void drawColumnLogo()
        {
            GUILayout.BeginVertical(GUILayout.Width(logoWidth));
            {
                if (Constants.PLATFORM_WINDOWS)
                {
                    drawLogo(Helper.Logo_Windows);
                }

                if (Constants.PLATFORM_MAC)
                {
                    drawLogo(Helper.Logo_Mac);
                }

                if (Constants.PLATFORM_LINUX)
                {
                    drawLogo(Helper.Logo_Linux);
                }

                if (Constants.PLATFORM_ANDROID)
                {
                    drawLogo(Helper.Logo_Android);
                }

                if (Constants.PLATFORM_IOS)
                {
                    drawLogo(Helper.Logo_Ios);
                }

#if UNITY_5
                if (Constants.PLATFORM_WSA)
                {
                    drawLogo(Helper.Logo_Wsa);
                }
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                if (Constants.PLATFORM_WEBPLAYER)
                {
                    drawLogo(Helper.Logo_Webplayer);
                }
#endif

#if UNITY_5
                if (Constants.PLATFORM_WEBGL)
                {
                    drawLogo(Helper.Logo_Webgl);
                }
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TVOS)
                {
                    drawLogo(Helper.Logo_Tvos);
                }
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TIZEN)
                {
                    drawLogo(Helper.Logo_Tizen);
                }
#endif

                if (Constants.PLATFORM_SAMSUNGTV)
                {
                    drawLogo(Helper.Logo_Samsungtv);
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_PS3)
                {
                    drawLogo(Helper.Logo_Ps3);
                }
#endif

                if (Constants.PLATFORM_PS4)
                {
                    drawLogo(Helper.Logo_Ps4);
                }

                if (Constants.PLATFORM_PSP2)
                {
                    drawLogo(Helper.Logo_Psp);
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_XBOX360)
                {
                    drawLogo(Helper.Logo_Xbox360);
                }
#endif

                if (Constants.PLATFORM_XBOXONE)
                {
                    drawLogo(Helper.Logo_Xboxone);
                }

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_WIIU)
                {
                    drawLogo(Helper.Logo_Wiiu);
                }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_3DS)
                {
                    drawLogo(Helper.Logo_3ds);
                }
#endif

#if UNITY_5_6_OR_NEWER
                if (Constants.PLATFORM_SWITCH)
                {
                    drawLogo(Helper.Logo_Switch);
                }
#endif

            }
            GUILayout.EndVertical();
        }

        private static void drawColumnPlatform()
        {
            GUILayout.BeginVertical(GUILayout.Width(platformWidth));
            {
                if (Constants.PLATFORM_WINDOWS)
                {
                    drawPlatform("Standalone Windows");
                }

                if (Constants.PLATFORM_MAC)
                {
                    drawPlatform("Standalone Mac");
                }

                if (Constants.PLATFORM_LINUX)
                {
                    drawPlatform("Standalone Linux");
                }

                if (Constants.PLATFORM_ANDROID)
                {
                    drawPlatform("Android");
                }

                if (Constants.PLATFORM_IOS)
                {
                    drawPlatform("iOS");
                }

#if UNITY_5
                if (Constants.PLATFORM_WSA)
                {
                    drawPlatform("WSA");
                }
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                if (Constants.PLATFORM_WEBPLAYER)
                {
                    drawPlatform("WebPlayer");
                }
#endif

#if UNITY_5
                if (Constants.PLATFORM_WEBGL)
                {
                    drawPlatform("WebGL");
                }
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TVOS)
                {
                    drawPlatform("tvOS");
                }
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TIZEN)
                {
                    drawPlatform("Tizen");
                }
#endif

                if (Constants.PLATFORM_SAMSUNGTV)
                {
                    drawPlatform("Samsung TV");
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_PS3)
                {
                    drawPlatform("PS3");
                }
#endif

                if (Constants.PLATFORM_PS4)
                {
                    drawPlatform("PS4");
                }

                if (Constants.PLATFORM_PSP2)
                {
                    drawPlatform("PSP2 (Vita)");
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_XBOX360)
                {
                    drawPlatform("XBox360");
                }
#endif

                if (Constants.PLATFORM_XBOXONE)
                {
                    drawPlatform("XBoxOne");
                }

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_WIIU)
                {
                    drawPlatform("WiiU");
                }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_3DS)
                {
                    drawPlatform("3DS");
                }
#endif

#if UNITY_5_6_OR_NEWER
                if (Constants.PLATFORM_SWITCH)
                {
                    drawPlatform("Switch");
                }
#endif
            }
            GUILayout.EndVertical();
        }

        private static void drawColumnArchitecture()
        {
            GUILayout.BeginVertical(GUILayout.Width(architectureWidth));
            {
                int heightSpace = 12;


                if (Constants.PLATFORM_WINDOWS)
                {
                    GUILayout.Space(heightSpace);
                    Constants.ARCH_WINDOWS = EditorGUILayout.Popup("", Constants.ARCH_WINDOWS, archWinOptions, new GUILayoutOption[] { GUILayout.Width(architectureWidth - 10) });
                    heightSpace = 18;
                }

                if (Constants.PLATFORM_MAC)
                {
                    GUILayout.Space(heightSpace);
                    Constants.ARCH_MAC = EditorGUILayout.Popup("", Constants.ARCH_MAC, archMacOptions, new GUILayoutOption[] { GUILayout.Width(architectureWidth - 10) });
                    heightSpace = 18;
                }

                if (Constants.PLATFORM_LINUX)
                {
                    GUILayout.Space(heightSpace);
                    Constants.ARCH_LINUX = EditorGUILayout.Popup("", Constants.ARCH_LINUX, archLinuxOptions, new GUILayoutOption[] { GUILayout.Width(architectureWidth - 10) });
                    heightSpace = 18;
                }
            }
            GUILayout.EndVertical();
        }

        private static void drawColumnTexture()
        {
            GUILayout.BeginVertical(GUILayout.Width(textureWidth));
            {
                int heightSpace = 12;

                if (Constants.PLATFORM_WINDOWS)
                {
                    GUILayout.Space(heightSpace);
                    heightSpace = 35;
                }

                if (Constants.PLATFORM_MAC)
                {
                    GUILayout.Space(heightSpace);
                    heightSpace = 35;
                }

                if (Constants.PLATFORM_LINUX)
                {
                    GUILayout.Space(heightSpace);
                    heightSpace = 35;
                }

                if (Constants.PLATFORM_ANDROID)
                {
                    GUILayout.Space(heightSpace);
                    Constants.TEX_ANDROID = EditorGUILayout.Popup("", Constants.TEX_ANDROID, texAndroidOptions, new GUILayoutOption[] { GUILayout.Width(textureWidth - 10) });
                    heightSpace = 18;
                }
            }
            GUILayout.EndVertical();
        }

        private static void drawColumnCached()
        {
            GUILayout.BeginVertical(GUILayout.Width(cacheWidth));
            {
                if (Constants.PLATFORM_WINDOWS)
                {
                    drawCached(targetWindows);
                }

                if (Constants.PLATFORM_MAC)
                {
                    drawCached(targetMac);
                }

                if (Constants.PLATFORM_LINUX)
                {
                    drawCached(targetLinux);
                }

                if (Constants.PLATFORM_ANDROID)
                {
                    drawCached(BuildTarget.Android);
                }

                if (Constants.PLATFORM_IOS)
                {
#if UNITY_5
                    drawCached(BuildTarget.iOS);
#else
                    drawCached(BuildTarget.iOS);
#endif
                }

#if UNITY_5
                if (Constants.PLATFORM_WSA)
                {
                    drawCached(BuildTarget.WSAPlayer);
                }
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                if (Constants.PLATFORM_WEBPLAYER)
                {
                    drawCached(BuildTarget.WebPlayer);
                }
#endif

#if UNITY_5
                if (Constants.PLATFORM_WEBGL)
                {
                    drawCached(BuildTarget.WebGL);
                }
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TVOS)
                {
                    drawCached(BuildTarget.tvOS);
                }
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TIZEN)
                {
                    drawCached(BuildTarget.Tizen);
                }
#endif

                //if (Constants.PLATFORM_SAMSUNGTV)
                //{
                //    drawCached(BuildTarget.SamsungTV);
                //}

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_PS3)
                {
                    drawCached(BuildTarget.PS3);
                }
#endif

                if (Constants.PLATFORM_PS4)
                {
                    drawCached(BuildTarget.PS4);
                }

                if (Constants.PLATFORM_PSP2)
                {
                    drawCached(BuildTarget.PSP2);
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_XBOX360)
                {
                    drawCached(BuildTarget.XBOX360);
                }
#endif

                if (Constants.PLATFORM_XBOXONE)
                {
                    drawCached(BuildTarget.XboxOne);
                }

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_WIIU)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    drawCached(BuildTarget.WiiU);
#pragma warning restore CS0618 // Type or member is obsolete
                }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_3DS)
                {
#if UNITY_5_5_OR_NEWER
                    drawCached(BuildTarget.N3DS);
#else
                    drawCached(BuildTarget.Nintendo3DS);
#endif
                }
#endif

#if UNITY_5_6_OR_NEWER
                if (Constants.PLATFORM_SWITCH)
                {
                    drawCached(BuildTarget.Switch);
                }
#endif

            }
            GUILayout.EndVertical();
        }

        private static void drawColumnAction()
        {
            GUILayout.BeginVertical();
            {
                if (Constants.PLATFORM_WINDOWS)
                {
                    drawAction(targetWindows, buildWindows, Helper.Logo_Windows);
                }

                if (Constants.PLATFORM_MAC)
                {
                    drawAction(targetMac, buildMac, Helper.Logo_Mac);
                }

                if (Constants.PLATFORM_LINUX)
                {
                    drawAction(targetLinux, buildLinux, Helper.Logo_Linux);
                }

                if (Constants.PLATFORM_ANDROID)
                {
                    drawAction(BuildTarget.Android, "android", Helper.Logo_Android);
                }

                if (Constants.PLATFORM_IOS)
                {
#if UNITY_5
                    drawAction(BuildTarget.iOS, "iOS", Helper.Logo_Ios);
#else
                    drawAction(BuildTarget.iOS, "iPhone", Helper.Logo_Ios);
#endif
                }

#if UNITY_5
                if (Constants.PLATFORM_WSA)
                {
                    drawAction(BuildTarget.WSAPlayer, "wsaplayer", Helper.Logo_Wsa);
                }
#endif

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
                if (Constants.PLATFORM_WEBPLAYER)
                {
                    drawAction(BuildTarget.WebPlayer, "webplayer", Helper.Logo_Webplayer);
                }
#endif

#if UNITY_5
                if (Constants.PLATFORM_WEBGL)
                {
                    drawAction(BuildTarget.WebGL, "webgl", Helper.Logo_Webgl);
                }
#endif

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TVOS)
                {
                    drawAction(BuildTarget.tvOS, "tvOS", Helper.Logo_Tvos);
                }
#endif

#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_TIZEN)
                {
                    drawAction(BuildTarget.Tizen, "tizen", Helper.Logo_Tizen);
                }
#endif

                //if (Constants.PLATFORM_SAMSUNGTV)
                //{
                //    drawAction(BuildTarget.SamsungTV, "samsungtv", Helper.Logo_Samsungtv);
                //}

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_PS3)
                {
                    drawAction(BuildTarget.PS3, "ps3", Helper.Logo_Ps3);
                }
#endif

                if (Constants.PLATFORM_PS4)
                {
                    drawAction(BuildTarget.PS4, "ps4", Helper.Logo_Ps4);
                }

                if (Constants.PLATFORM_PSP2)
                {
                    drawAction(BuildTarget.PSP2, "psp2", Helper.Logo_Psp);
                }

#if !UNITY_5_5_OR_NEWER
                if (Constants.PLATFORM_XBOX360)
                {
                    drawAction(BuildTarget.XBOX360, "xbox360", Helper.Logo_Xbox360);
                }
#endif

                if (Constants.PLATFORM_XBOXONE)
                {
                    drawAction(BuildTarget.XboxOne, "xboxone", Helper.Logo_Xboxone);
                }

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_WIIU)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    drawAction(BuildTarget.WiiU, "wiiu", Helper.Logo_Wiiu);
#pragma warning restore CS0618 // Type or member is obsolete
                }
#endif

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
                if (Constants.PLATFORM_3DS)
                {
#if UNITY_5_5_OR_NEWER
                    drawAction(BuildTarget.N3DS, "N3DS", Helper.Logo_3ds);
#else
                    drawAction(BuildTarget.Nintendo3DS, "N3DS", Helper.Logo_3ds);
#endif
                }
#endif

#if UNITY_5_6_OR_NEWER
                if (Constants.PLATFORM_SWITCH)
                {
                    drawAction(BuildTarget.Switch, "switch", Helper.Logo_Switch);
                }
#endif

            }
            GUILayout.EndVertical();
        }

        private static void drawVerticalSeparator(bool title = false)
        {
            GUILayout.BeginVertical(GUILayout.Width(2));
            {
                if (title)
                {
                    GUILayout.Box(string.Empty, new GUILayoutOption[] { /*GUILayout.ExpandHeight(true)*/ GUILayout.Height(24), GUILayout.Width(1) });
                }
                else
                {
                    GUILayout.Box(string.Empty, new GUILayoutOption[] { GUILayout.Height(platformY + rowHeight - 4), GUILayout.Width(1) });
                }
            }
            GUILayout.EndVertical();
        }

        private static void drawZebra(BuildTarget target)
        {
            platformY += rowHeight;
            rowHeight = 36;

            if (EditorUserBuildSettings.activeBuildTarget == target)
            {
                Color currentPlatform = new Color(0f, 0.33f, 0.71f); //CT-blue

                if (target == BuildTarget.Android)
                {
                    if (EditorUserBuildSettings.androidBuildSubtarget == texAndroid)
                    {
                        EditorGUI.DrawRect(new Rect(platformX, platformY, rowWidth, rowHeight), currentPlatform);
                    }
                    else
                    {
                        if (rowCounter % 2 == 0)
                        {
                            EditorGUI.DrawRect(new Rect(platformX, platformY, rowWidth, rowHeight), Color.gray);
                        }
                    }
                }
                else
                {
                    EditorGUI.DrawRect(new Rect(platformX, platformY, rowWidth, rowHeight), currentPlatform);
                }
            }
            else
            {

                if (rowCounter % 2 == 0)
                {
                    EditorGUI.DrawRect(new Rect(platformX, platformY, rowWidth, rowHeight), Color.gray);
                }
            }

            rowCounter++;
        }

        private static void drawLogo(Texture2D logo)
        {
            platformY += rowHeight;
            rowHeight = 36;

            GUILayout.Label(string.Empty);

            GUI.DrawTexture(new Rect(platformX + 4, platformY + 4, 28, 28), logo);
        }


        private static void drawPlatform(string label)
        {
            GUILayout.Space(platformTextSpace);
            GUILayout.Label(label /*, EditorStyles.boldLabel */);

            platformTextSpace = 18;
        }

        private static void drawCached(BuildTarget target)
        {
            GUILayout.Space(cacheTextSpace);

            if (Helper.isCached(target, texAndroid))
            {
                //GUILayout.Label(Helper.Icon_Cachefull);
                //GUILayout.Label(new GUIContent (" x", Helper.Icon_Cachefull, "Cached"));
                GUILayout.Label(new GUIContent(string.Empty, Helper.Icon_Cachefull, "Cached: " + target));
            }
            else
            {
                //GUILayout.Label(Helper.Icon_Cacheempty);
                //GUILayout.Label(new GUIContent (" -", Helper.Icon_Cacheempty));
                GUILayout.Label(new GUIContent(string.Empty, Helper.Icon_Cacheempty, "Not cached: " + target));
            }

            cacheTextSpace = 11;
        }

        private static void drawAction(BuildTarget target, string build, Texture2D logo)
        {
            GUILayout.Space(actionTextSpace);

            GUILayout.BeginHorizontal();
            {
                if (EditorUserBuildSettings.activeBuildTarget == target)
                {
                    if (target == BuildTarget.Android)
                    {
                        GUI.enabled = EditorUserBuildSettings.androidBuildSubtarget != texAndroid;
                    }
                    else
                    {
                        GUI.enabled = false;
                    }
                }


                if (GUILayout.Button(new GUIContent(string.Empty, logo, "Switch to " + target)))
                {
                    //if (GUILayout.Button (new GUIContent (" Switch", logo, "Switch to " + build))) {
                    if (!Constants.CONFIRM_SWITCH || EditorUtility.DisplayDialog("Switch to " + target + "?", "TPS will now close Unity, save and restore the necessary files and then restart Unity." + System.Environment.NewLine + "This operation could take some time." + System.Environment.NewLine + System.Environment.NewLine + "Would you like to switch the platform?", "Yes, switch to " + target, "Cancel"))
                    {
                        if (Constants.DEBUG)
                            Debug.Log("Switch initiated: " + target);

                        save();

                        if (target == BuildTarget.Android)
                        {

                            if (Helper.isCached(target, texAndroid))
                            {
                                Helper.SwitchPlatform(target, build, texAndroid);
                            }
                            else
                            {
                                EditorUserBuildSettings.androidBuildSubtarget = texAndroid;

                                Helper.SwitchPlatform(target, build, texAndroid);
                            }
                        }
                        else
                        {
#if UNITY_5
                            Helper.SwitchPlatform(target, build, MobileTextureSubtarget.Generic);
#else
						Helper.SwitchPlatform(target, build, MobileTextureSubtarget.Generic);
#endif

                        }
                    }
                }
                GUI.enabled = true;

                if (Constants.SHOW_DELETE && Helper.isCached(target, texAndroid))
                {
                    if (GUILayout.Button(new GUIContent(string.Empty, Helper.Icon_Delete_Big, "Delete cache from " + target)))
                    {
                        //if (GUILayout.Button (new GUIContent ("Delete", Helper.Icon_Delete, "Delete cache from " + build))) {
                        if (EditorUtility.DisplayDialog("Delete the cache for " + target + "?", "If you delete the cache, Unity must re-import all assets for this platform after a switch." + System.Environment.NewLine + "This operation could take some time." + System.Environment.NewLine + System.Environment.NewLine + "Would you like to delete the cache?", "Yes", "No"))
                        {
                            if (Constants.DEBUG)
                                Debug.Log("Cache deleted: " + target);

                            Helper.DeleteCacheFromTarget(target, texAndroid);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();

            actionTextSpace = 8;
        }

        private static void drawHeader()
        {
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            {
                if (Constants.SHOW_COLUMN_PLATFORM)
                {
                    GUILayout.BeginVertical(GUILayout.Width(platformWidth + (Constants.SHOW_COLUMN_PLATFORM_LOGO ? logoWidth + 4 : 0)));
                    {
                        GUILayout.Label(new GUIContent("Platform", "Platform name"), EditorStyles.boldLabel);
                    }
                    GUILayout.EndVertical();

                    drawVerticalSeparator(true);
                }

                if (Constants.SHOW_COLUMN_ARCHITECTURE && Helper.hasActiveArchitecturePlatforms)
                {
                    GUILayout.BeginVertical(GUILayout.Width(architectureWidth));
                    {
                        GUILayout.Label(new GUIContent("Arch", "Architecture of the target platform."), EditorStyles.boldLabel);
                    }
                    GUILayout.EndVertical();

                    drawVerticalSeparator(true);
                }

                if (Constants.SHOW_COLUMN_TEXTURE && Helper.hasActiveTexturePlatforms)
                {
                    GUILayout.BeginVertical(GUILayout.Width(textureWidth));
                    {
                        GUILayout.Label(new GUIContent("Texture", "Texture format"), EditorStyles.boldLabel);
                    }
                    GUILayout.EndVertical();

                    drawVerticalSeparator(true);
                }

                if (Constants.SHOW_COLUMN_CACHE)
                {
                    GUILayout.BeginVertical(GUILayout.Width(cacheWidth));
                    {
                        GUILayout.Label(new GUIContent("Cache", "Cache-status of the platform."), EditorStyles.boldLabel);
                    }
                    GUILayout.EndVertical();

                    drawVerticalSeparator(true);
                }

                GUILayout.BeginVertical(GUILayout.Width(actionWidth));
                {
                    GUILayout.Label(new GUIContent("Action", "Action for the platform."), EditorStyles.boldLabel);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            Helper.SeparatorUI(0);
            //GUILayout.Box(string.Empty, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        private static void drawContent()
        {
            GUILayout.BeginHorizontal();
            {
                drawColumnZebra();

                if (Constants.SHOW_COLUMN_PLATFORM)
                {
                    if (Constants.SHOW_COLUMN_PLATFORM_LOGO)
                    {
                        platformY = 0;
                        rowHeight = 0;
                        drawColumnLogo();
                    }

                    drawColumnPlatform();

                    drawVerticalSeparator();
                }

                if (Constants.SHOW_COLUMN_ARCHITECTURE && Helper.hasActiveArchitecturePlatforms)
                {
                    drawColumnArchitecture();

                    drawVerticalSeparator();
                }

                if (Constants.SHOW_COLUMN_TEXTURE && Helper.hasActiveTexturePlatforms)
                {
                    drawColumnTexture();

                    drawVerticalSeparator();
                }

                if (Constants.SHOW_COLUMN_CACHE)
                {
                    drawColumnCached();

                    drawVerticalSeparator();
                }

                drawColumnAction();
            }
            GUILayout.EndHorizontal();
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)