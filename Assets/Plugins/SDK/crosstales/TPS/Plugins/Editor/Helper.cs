using UnityEngine;
using UnityEditor;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif

namespace Crosstales.TPS
{
    /// <summary>Various helper functions.</summary>
    public static class Helper
    {

        #region Static variables

        private static readonly System.Text.RegularExpressions.Regex lineEndingsRegex = new System.Text.RegularExpressions.Regex(@"\r\n|\r|\n");

        private const string WINDOWS_PATH_DELIMITER = @"\";
        private const string UNIX_PATH_DELIMITER = "/";

        private static Texture2D logo_asset;
        private static Texture2D logo_asset_small;
        private static Texture2D logo_ct;
        private static Texture2D logo_unity;

        private static Texture2D icon_reset;
        private static Texture2D icon_refresh;
        private static Texture2D icon_delete;
        private static Texture2D icon_delete_big;
        private static Texture2D icon_folder;

        private static Texture2D icon_manual;
        private static Texture2D icon_api;
        private static Texture2D icon_forum;
        private static Texture2D icon_product;

        private static Texture2D icon_check;

        private static Texture2D logo_windows;
        private static Texture2D logo_mac;
        private static Texture2D logo_linux;
        private static Texture2D logo_ios;
        private static Texture2D logo_android;
        private static Texture2D logo_wsa;
        private static Texture2D logo_webplayer;
        private static Texture2D logo_webgl;
        private static Texture2D logo_tvos;
        private static Texture2D logo_tizen;
        private static Texture2D logo_samsungtv;
        private static Texture2D logo_ps3;
        private static Texture2D logo_ps4;
        private static Texture2D logo_psp;
        private static Texture2D logo_xbox360;
        private static Texture2D logo_xboxone;
        private static Texture2D logo_wiiu;
        private static Texture2D logo_3ds;
        private static Texture2D logo_switch;
        private static Texture2D icon_cachefull;
        private static Texture2D icon_cacheempty;
        //private static Texture2D icon_delete;

        #endregion


        #region Static properties

        public static Texture2D Logo_Asset
        {
            get
            {
                return loadImage(ref logo_asset, "logo_asset.png");
            }
        }

        public static Texture2D Logo_Asset_Small
        {
            get
            {
                return loadImage(ref logo_asset_small, "logo_asset_small.png");
            }
        }

        public static Texture2D Logo_CT
        {
            get
            {
                return loadImage(ref logo_ct, "logo_ct.png");
            }
        }

        public static Texture2D Logo_Unity
        {
            get
            {
                return loadImage(ref logo_unity, "logo_unity.png");
            }
        }

        public static Texture2D Icon_Reset
        {
            get
            {
                return loadImage(ref icon_reset, "icon_reset.png");
            }
        }

        public static Texture2D Icon_Refresh
        {
            get
            {
                return loadImage(ref icon_refresh, "icon_refresh.png");
            }
        }

        public static Texture2D Icon_Delete
        {
            get
            {
                return loadImage(ref icon_delete, "icon_delete.png");
            }
        }

        public static Texture2D Icon_Delete_Big
        {
            get
            {
                return loadImage(ref icon_delete_big, "icon_delete_big.png");
            }
        }

        public static Texture2D Icon_Folder
        {
            get
            {
                return loadImage(ref icon_folder, "icon_folder.png");
            }
        }

        public static Texture2D Icon_Manual
        {
            get
            {
                return loadImage(ref icon_manual, "icon_manual.png");
            }
        }

        public static Texture2D Icon_API
        {
            get
            {
                return loadImage(ref icon_api, "icon_api.png");
            }
        }

        public static Texture2D Icon_Forum
        {
            get
            {
                return loadImage(ref icon_forum, "icon_forum.png");
            }
        }

        public static Texture2D Icon_Product
        {
            get
            {
                return loadImage(ref icon_product, "icon_product.png");
            }
        }

        public static Texture2D Icon_Check
        {
            get
            {
                return loadImage(ref icon_check, "icon_check.png");
            }
        }

        public static Texture2D Logo_Windows
        {
            get
            {
                return loadImage(ref logo_windows, "logo_windows.png");
            }
        }

        public static Texture2D Logo_Mac
        {
            get
            {
                return loadImage(ref logo_mac, "logo_mac.png");
            }
        }

        public static Texture2D Logo_Linux
        {
            get
            {
                return loadImage(ref logo_linux, "logo_linux.png");
            }
        }

        public static Texture2D Logo_Ios
        {
            get
            {
                return loadImage(ref logo_ios, "logo_ios.png");
            }
        }

        public static Texture2D Logo_Android
        {
            get
            {
                return loadImage(ref logo_android, "logo_android.png");
            }
        }
        public static Texture2D Logo_Wsa
        {
            get
            {
                return loadImage(ref logo_wsa, "logo_wsa.png");
            }
        }

        public static Texture2D Logo_Webplayer
        {
            get
            {
                return loadImage(ref logo_webplayer, "logo_webplayer.png");
            }
        }

        public static Texture2D Logo_Webgl
        {
            get
            {
                return loadImage(ref logo_webgl, "logo_webgl.png");
            }
        }

        public static Texture2D Logo_Tvos
        {
            get
            {
                return loadImage(ref logo_tvos, "logo_tvos.png");
            }
        }

        public static Texture2D Logo_Tizen
        {
            get
            {
                return loadImage(ref logo_tizen, "logo_tizen.png");
            }
        }

        public static Texture2D Logo_Samsungtv
        {
            get
            {
                return loadImage(ref logo_samsungtv, "logo_samsungtv.png");
            }
        }

        public static Texture2D Logo_Ps3
        {
            get
            {
                return loadImage(ref logo_ps3, "logo_ps3.png");
            }
        }

        public static Texture2D Logo_Ps4
        {
            get
            {
                return loadImage(ref logo_ps4, "logo_ps4.png");
            }
        }

        public static Texture2D Logo_Psp
        {
            get
            {
                return loadImage(ref logo_psp, "logo_psp.png");
            }
        }

        public static Texture2D Logo_Xbox360
        {
            get
            {
                return loadImage(ref logo_xbox360, "logo_xbox360.png");
            }
        }

        public static Texture2D Logo_Xboxone
        {
            get
            {
                return loadImage(ref logo_xboxone, "logo_xboxone.png");
            }
        }

        public static Texture2D Logo_Wiiu
        {
            get
            {
                return loadImage(ref logo_wiiu, "logo_wiiu.png");
            }
        }

        public static Texture2D Logo_3ds
        {
            get
            {
                return loadImage(ref logo_3ds, "logo_3ds.png");
            }
        }

        public static Texture2D Logo_Switch
        {
            get
            {
                return loadImage(ref logo_switch, "logo_switch.png");
            }
        }

        public static Texture2D Icon_Cachefull
        {
            get
            {
                return loadImage(ref icon_cachefull, "icon_cachefull.png");
            }
        }

        public static Texture2D Icon_Cacheempty
        {
            get
            {
                return loadImage(ref icon_cacheempty, "icon_cacheempty.png");
            }
        }

        /// <summary>Checks if the current platform is Windows.</summary>
        /// <returns>True if the current platform is Windows.</returns>
        public static bool isWindowsPlatform
        {
            get
            {
                return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
            }
        }

        /// <summary>Checks if we are in Editor mode.</summary>
        /// <returns>True if in Editor mode.</returns>
        public static bool isEditorMode
        {
            get
            {
                return (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) && !Application.isPlaying;
            }
        }

        /// <summary>Checks if the user has selected any architecture platforms.</summary>
        /// <returns>True if the user has selected any architecture platforms.</returns>
        public static bool hasActiveArchitecturePlatforms
        {
            get
            {
                return Constants.PLATFORM_WINDOWS || Constants.PLATFORM_MAC || Constants.PLATFORM_LINUX;
            }
        }

        /// <summary>Checks if the user has selected any texture platforms.</summary>
        /// <returns>True if the user has selected any texture platforms.</returns>
        public static bool hasActiveTexturePlatforms
        {
            get
            {
                return Constants.PLATFORM_ANDROID;
            }
        }

        #endregion


        #region Public static methods

        /// <summary>HTTPS-certification callback.</summary>
        public static bool RemoteCertificateValidationCallback(System.Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            bool isOk = true;

#if UNITY_5_4_OR_NEWER
			// If there are errors in the certificate chain, look at each error to determine the cause.
			if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
			{
				for (int i = 0; i < chain.ChainStatus.Length; i++)
				{
					if (chain.ChainStatus[i].Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown)
					{
						chain.ChainPolicy.RevocationFlag = System.Security.Cryptography.X509Certificates.X509RevocationFlag.EntireChain;
						chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
						chain.ChainPolicy.UrlRetrievalTimeout = new System.TimeSpan(0, 1, 0);
						chain.ChainPolicy.VerificationFlags = System.Security.Cryptography.X509Certificates.X509VerificationFlags.AllFlags;

						isOk = chain.Build((System.Security.Cryptography.X509Certificates.X509Certificate2)certificate);
					}
				}
			}
#endif

            return isOk;
        }

        /// <summary>Switches the current platform to the target.</summary>
        /// <param name="target">Target platform for the switch</param>
        /// <param name="build">Build type name for Unity, like 'win32'</param>
        /// <param name="subTarget">Texture format (Android)</param>
#if UNITY_5
        public static void SwitchPlatform(BuildTarget target, string build, MobileTextureSubtarget subTarget)
#else
		public static void SwitchPlatform(BuildTarget target, string build, MobileTextureSubtarget subTarget)
#endif
        {
            string savePathExtension = getExtension(EditorUserBuildSettings.activeBuildTarget, EditorUserBuildSettings.androidBuildSubtarget);
            string loadPathExtension = getExtension(target, subTarget);

            if (!Constants.CUSTOM_PATH_CACHE && Constants.VCS != 0)
            {
                if (Constants.VCS == 1)
                {
                    // git
                    if (!System.IO.File.Exists(Constants.PATH + ".gitignore"))
                    {
                        System.IO.File.WriteAllText(Constants.PATH + ".gitignore", Constants.CACHE_DIRNAME + "/");
                    }
                }
                else if (Constants.VCS == 2)
                {
                    // svn
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo.FileName = "svn";
                        process.StartInfo.Arguments = "propset svn: ignore " + Constants.CACHE_DIRNAME + ".";
                        process.StartInfo.WorkingDirectory = Constants.PATH;
                        process.StartInfo.UseShellExecute = false;

                        try
                        {
                            process.Start();

                            process.WaitForExit(Constants.KILL_TIME);
                        }
                        catch (System.Exception ex)
                        {
                            string errorMessage = "Could execute svn-ignore! Please do it manually in the console: 'svn propset svn:ignore " + Constants.CACHE_DIRNAME + ".'" + System.Environment.NewLine + ex;
                            UnityEngine.Debug.LogError(errorMessage);
                        }
                    }
                }
                else
                {
                    // mercurial
                    UnityEngine.Debug.LogError("Mercurial currently not supported. Please add the following lines to your .hgignore: " + System.Environment.NewLine + "syntax: glob" + System.Environment.NewLine + Constants.CACHE_DIRNAME + "/**");
                }
            }

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
#else
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
#endif

            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                try
                {

                    if (Application.platform == RuntimePlatform.WindowsEditor)
                    {
                        string scriptfile = System.IO.Path.GetTempPath() + "TPS-" + Constants.ASSET_ID + "-" + System.Guid.NewGuid() + ".cmd";

                        System.IO.File.WriteAllText(scriptfile, Helper.generateWindowsScript(target, build, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "\"" + scriptfile + "\"";
                    }
                    else if (Application.platform == RuntimePlatform.OSXEditor)
                    {
                        string scriptfile = System.IO.Path.GetTempPath() + "TPS-" + Constants.ASSET_ID + "-" + System.Guid.NewGuid() + ".sh";

                        System.IO.File.WriteAllText(scriptfile, Helper.generateMacScript(target, build, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "/bin/sh";
                        process.StartInfo.Arguments = "\"" + scriptfile + "\"";
                    }
#if UNITY_5_5_OR_NEWER
                    else if (Application.platform == RuntimePlatform.LinuxEditor)
                    {
                        string scriptfile = System.IO.Path.GetTempPath() + "TPS-" + Constants.ASSET_ID + "-" + System.Guid.NewGuid() + ".sh";

                        System.IO.File.WriteAllText(scriptfile, Helper.generateLinuxScript(target, build, subTarget, savePathExtension, loadPathExtension));

                        process.StartInfo.FileName = "/bin/sh";
                        process.StartInfo.Arguments = "\"" + scriptfile + "\"";
                    }
#endif
                    else
                    {
                        Debug.LogError("Unsupported platform: " + Application.platform);
                        return;
                    }

                    process.StartInfo.UseShellExecute = true;

                    //UnityEngine.Debug.LogWarning(process.StartInfo.FileName);
                    //UnityEngine.Debug.LogWarning(process.StartInfo.Arguments);

                    process.Start();

                    EditorApplication.Exit(0);
                }
                catch (System.Exception ex)
                {
                    string errorMessage = "Could execute TPS!" + System.Environment.NewLine + ex;
                    UnityEngine.Debug.LogError(errorMessage);
                }
            }
        }

        /// <summary>Checks if a platform is already cached.</summary>
        /// <param name="target">Platform to check</param>
        /// <returns>True if the platform is already cached</returns>
        /// <param name="subTarget">Texture format (Android)</param>
#if UNITY_5
        public static bool isCached(BuildTarget target, MobileTextureSubtarget subTarget)
#else
		public static bool isCached(BuildTarget target, MobileTextureSubtarget subTarget)
#endif
        {
            return System.IO.Directory.Exists(Constants.PATH_CACHE + target.ToString() + getExtension(target, subTarget));
        }

        /// <summary>Deletes a cache for a target platform.</summary>
        /// <param name="target">Platform to delete the cache</param>
        /// <param name="subTarget">Texture format (Android)</param>
#if UNITY_5
        public static void DeleteCacheFromTarget(BuildTarget target, MobileTextureSubtarget subTarget)
#else
		public static void DeleteCacheFromTarget(BuildTarget target, MobileTextureSubtarget subTarget)
#endif
        {
            if (isCached(target, subTarget))
            {
                try
                {
                    System.IO.Directory.Delete(Constants.PATH_CACHE + target.ToString() + getExtension(target, subTarget), true);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogWarning("Could not delete the cache for target: " + target + System.Environment.NewLine + ex);
                }
            }
        }

        /// <summary>Delete the cache for all platforms.</summary>
        public static void DeleteCache()
        {
            if (System.IO.Directory.Exists(Constants.PATH_CACHE))
            {
                try
                {
                    System.IO.Directory.Delete(Constants.PATH_CACHE, true);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogWarning("Could not delete the cache!" + System.Environment.NewLine + ex);
                }
            }
        }

        /// <summary>Delete all shell-scripts after a platform switch.</summary>
        public static void DeleteAllScripts()
        {
            //INFO: currently disabled since it could interfer with running scripts!

            //var dir = new DirectoryInfo(Path.GetTempPath());

            //try
            //{
            //    foreach (var file in dir.GetFiles("TPS-" + Constants.ASSET_ID + "*"))
            //    {
            //        if (Constants.DEBUG)
            //            UnityEngine.Debug.Log("Script file deleted: " + file);

            //        file.Delete();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    UnityEngine.Debug.LogWarning("Could not delete all script files!" + Environment.NewLine + ex);
            //}
        }

        /// <summary>Validates a given path and add missing slash.</summary>
        /// <param name="path">Path to validate</param>
        /// <param name="addEndDelimiter">Add delimiter at the end of the path (optional, default: true)</param>
        /// <returns>Valid path</returns>
        public static string ValidatePath(string path, bool addEndDelimiter = true)
        {
            string result;

            if (isWindowsPlatform)
            {
                result = path.Replace('/', '\\');

                if (addEndDelimiter)
                {
                    if (!result.EndsWith(WINDOWS_PATH_DELIMITER))
                    {
                        result += WINDOWS_PATH_DELIMITER;
                    }
                }
            }
            else
            {
                result = path.Replace('\\', '/');

                if (addEndDelimiter)
                {
                    if (!result.EndsWith(UNIX_PATH_DELIMITER))
                    {
                        result += UNIX_PATH_DELIMITER;
                    }
                }
            }

            return result;
        }

        /// <summary>Split the given text to lines and return it as list.</summary>
        /// <param name="text">Complete text fragment</param>
        /// <returns>Splitted lines as array</returns>
        public static System.Collections.Generic.List<string> SplitStringToLines(string text)
        {
            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

            if (string.IsNullOrEmpty(text))
            {
                UnityEngine.Debug.LogWarning("Parameter 'text' is null or empty!" + System.Environment.NewLine + "=> 'SplitStringToLines()' will return an empty string list.");
            }
            else
            {
                string[] lines = lineEndingsRegex.Split(text);

                for (int ii = 0; ii < lines.Length; ii++)
                {
                    if (!System.String.IsNullOrEmpty(lines[ii]))
                    {
                        result.Add(lines[ii]);
                    }
                }
            }

            return result;
        }

        /// <summary>Shows a separator-UI.</summary>
        public static void SeparatorUI(int space = 12)
        {
            GUILayout.Space(space);
            GUILayout.Box(string.Empty, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        #endregion


        #region Private static methods

#if UNITY_5
        private static string generateWindowsScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#else
		private static string generateWindowsScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#endif
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //setup
            sb.Append("@echo off");
            sb.AppendLine();
            sb.Append("cls");
            sb.AppendLine();

            //title
            sb.Append("title ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of the Unity project under ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.AppendLine();

            //header
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("echo #                                                                            #");
            sb.AppendLine();
            sb.Append("echo #  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.Append(" - Windows                                     #");
            sb.AppendLine();
            sb.Append("echo #  Copyright 2016-2017 by www.crosstales.com                                 #");
            sb.AppendLine();
            sb.Append("echo #                                                                            #");
            sb.AppendLine();
            sb.Append("echo #  The files will now be synchronized between the platforms.                 #");
            sb.AppendLine();
            sb.Append("echo #  This will take some time, so please be patient and DON'T close this       #");
            sb.AppendLine();
            sb.Append("echo #  window before the process is finished!                                    #");
            sb.AppendLine();
            sb.Append("echo #                                                                            #");
            sb.AppendLine();
            sb.Append("echo #  Unity will restart automatically after the sync.                          #");
            sb.AppendLine();
            sb.Append("echo #                                                                            #");
            sb.AppendLine();
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();

            //check if Unity is closed
            sb.Append(":waitloop");
            sb.AppendLine();
            sb.Append("if not exist \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp\\UnityLockfile\" goto waitloopend");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();
            sb.Append("echo Waiting for Unity to close...");
            sb.AppendLine();
            sb.Append("timeout /t 3");

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
            sb.AppendLine();
            sb.Append("del \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp\\UnityLockfile\" /q");
#endif

            sb.AppendLine();
            sb.Append("goto waitloop");
            sb.AppendLine();
            sb.Append(":waitloopend");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();

            //Save files
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("echo #  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.AppendLine();
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("robocopy \"");
            sb.Append(Constants.PATH);
            sb.Append("Library\" \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.Append("\\Library");
            sb.Append("\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("robocopy \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings\" \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("\\ProjectSettings");
                sb.Append("\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np");
                sb.AppendLine();
                sb.Append("echo.");
                sb.AppendLine();
            }

            //Restore files
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("echo #  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.AppendLine();
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("robocopy \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append("\\Library\" \"");
            sb.Append(Constants.PATH);
            sb.Append("Library\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("robocopy \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("\\ProjectSettings\" \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings\" /MIR /W:3 /MT /NFL /NDL /NJH /NJS /nc /ns /np");
                sb.AppendLine();
                sb.Append("echo.");
                sb.AppendLine();
            }

            //Restart Unity
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("echo #  Restarting Unity                                                          #");
            sb.AppendLine();
            sb.Append("echo ##############################################################################");
            sb.AppendLine();
            sb.Append("start \"\" \"");
            sb.Append(Helper.ValidatePath(EditorApplication.applicationPath, false));
            sb.Append("\" -projectPath \"");
            sb.Append(Constants.PATH.Substring(0, Constants.PATH.Length - 1));
            sb.Append("\" -buildTarget ");
            sb.Append(build);

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
			if (target == BuildTarget.Android)
			{
				sb.Append(" -executeMethod Crosstales.TPS.Helper.setAndroidTexture");
				//sb.Append(subTarget);
			}
			else
			{
				if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
				{
					sb.Append(" -executeMethod ");
					sb.Append(Constants.EXECUTE_METHOD);
				}
			}
#else
            if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Constants.EXECUTE_METHOD);
            }
#endif
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();

            //check if Unity is started
            sb.Append(":waitloop2");
            sb.AppendLine();
            sb.Append("if exist \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp\\UnityLockfile\" goto waitloopend2");
            sb.AppendLine();
            sb.Append("echo Waiting for Unity to start...");
            sb.AppendLine();
            sb.Append("timeout /t 2");
            sb.AppendLine();
            sb.Append("goto waitloop2");
            sb.AppendLine();
            sb.Append(":waitloopend2");
            sb.AppendLine();
            sb.Append("echo.");
            sb.AppendLine();
            sb.Append("echo Bye!");
            sb.AppendLine();
            sb.Append("timeout /t 1");
            sb.AppendLine();
            sb.Append("exit");

            return sb.ToString();
        }

#if UNITY_5
        private static string generateMacScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#else
		private static string generateMacScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#endif
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //setup
            sb.Append("#!/bin/bash");
            sb.AppendLine();
            sb.Append("set +v");
            sb.AppendLine();
            sb.Append("clear");
            sb.AppendLine();

            //title
            sb.Append("title='");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of the Unity project under ");
            sb.Append(target.ToString());
            sb.Append(savePathExtension);
            sb.Append("'");
            sb.AppendLine();
            sb.Append("echo -n -e \"\\033]0;$title\\007\"");
            sb.AppendLine();

            //header
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.Append(" - macOS                                       ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Copyright 2016-2017 by www.crosstales.com                                 ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  The files will now be synchronized between the platforms.                 ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  This will take some time, so please be patient and DON'T close this       ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  window before the process is finished!                                    ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Unity will restart automatically after the sync.                          ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            //check if Unity is closed
            sb.Append("while [ -f \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.Append("do");
            sb.AppendLine();
            sb.Append("  echo \"Waiting for Unity to close...\"");
            sb.AppendLine();
            sb.Append("  sleep 3");

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
            sb.AppendLine();
            sb.Append("rm \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\"");
#endif

            sb.AppendLine();
            sb.Append("done");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            //Save files
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(loadPathExtension);
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("mkdir -p \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.Append("//Library");
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("rsync -aq --delete \"");
            sb.Append(Constants.PATH);
            sb.Append("Library/\" \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.Append("//Library");
            sb.Append("/\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("\"");
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings/\" \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("/\"");
                sb.AppendLine();
                sb.Append("echo");
                sb.AppendLine();
            }

            //Restore files
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("rsync -aq --delete \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append("//Library");
            sb.Append("/\" \"");
            sb.Append(Constants.PATH);
            sb.Append("Library/\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("/\" \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings/\"");
                sb.AppendLine();
                sb.Append("echo");
                sb.AppendLine();
            }

            //Restart Unity
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Restarting Unity                                                          ¦\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("open -a \"");
            sb.Append(EditorApplication.applicationPath);
            sb.Append("\" --args -projectPath \"");
            sb.Append(Constants.PATH);
            sb.Append("\" -buildTarget ");
            sb.Append(build);

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
			if (target == BuildTarget.Android)
			{
				sb.Append(" -executeMethod Crosstales.TPS.Helper.setAndroidTexture");
				//sb.Append(subTarget);
			}
			else
			{
				if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
				{
					sb.Append(" -executeMethod ");
					sb.Append(Constants.EXECUTE_METHOD);
				}
			}
#else
            if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Constants.EXECUTE_METHOD);
            }
#endif

            sb.AppendLine();

            //check if Unity is started
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("while [ ! -f \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.Append("do");
            sb.AppendLine();
            sb.Append("  echo \"Waiting for Unity to start...\"");
            sb.AppendLine();
            sb.Append("  sleep 2");
            sb.AppendLine();
            sb.Append("done");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("echo \"Bye!\"");
            sb.AppendLine();
            sb.Append("sleep 1");
            sb.AppendLine();
            sb.Append("exit");

            return sb.ToString();
        }
#if UNITY_5
        private static string generateLinuxScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#else
		private static string generateLinuxScript(BuildTarget target, string build, MobileTextureSubtarget subTarget, string savePathExtension, string loadPathExtension)
#endif
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //setup
            sb.Append("#!/bin/bash");
            sb.AppendLine();
            sb.Append("set +v");
            sb.AppendLine();
            sb.Append("clear");
            sb.AppendLine();

            //title
            sb.Append("title='");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" - Relaunch of the Unity project under ");
            sb.Append(target.ToString());
            sb.Append(savePathExtension);
            sb.Append("'");
            sb.AppendLine();
            sb.Append("echo -n -e \"\\033]0;$title\\007\"");
            sb.AppendLine();

            //header
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  ");
            sb.Append(Constants.ASSET_NAME);
            sb.Append(" ");
            sb.Append(Constants.ASSET_VERSION);
            sb.Append(" - macOS                                       ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Copyright 2016-2017 by www.crosstales.com                                 ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  The files will now be synchronized between the platforms.                 ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  This will take some time, so please be patient and DON'T close this       ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  window before the process is finished!                                    ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Unity will restart automatically after the sync.                          ¦\"");
            sb.AppendLine();
            sb.Append("echo \"¦                                                                            ¦\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            //check if Unity is closed
            sb.Append("while [ -f \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.Append("do");
            sb.AppendLine();
            sb.Append("  echo \"Waiting for Unity to close...\"");
            sb.AppendLine();
            sb.Append("  sleep 3");

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
            sb.AppendLine();
            sb.Append("rm \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\"");
#endif

            sb.AppendLine();
            sb.Append("done");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            //Save files
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Saving files from ");
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(loadPathExtension);
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("mkdir -p \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.Append("//Library");
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("rsync -aq --delete \"");
            sb.Append(Constants.PATH);
            sb.Append("Library/\" \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
            sb.Append(savePathExtension);
            sb.Append("//Library");
            sb.Append("/\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("mkdir -p \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("\"");
                sb.AppendLine();
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings/\" \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(EditorUserBuildSettings.activeBuildTarget.ToString());
                sb.Append(savePathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("/\"");
                sb.AppendLine();
                sb.Append("echo");
                sb.AppendLine();
            }

            //Restore files
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Restoring files from ");
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append("\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("rsync -aq --delete \"");
            sb.Append(Constants.PATH_CACHE);
            sb.Append(target.ToString());
            sb.Append(loadPathExtension);
            sb.Append("//Library");
            sb.Append("/\" \"");
            sb.Append(Constants.PATH);
            sb.Append("Library/\"");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();

            if (Constants.COPY_SETTINGS)
            {
                sb.Append("rsync -aq --delete \"");
                sb.Append(Constants.PATH_CACHE);
                sb.Append(target.ToString());
                sb.Append(loadPathExtension);
                sb.Append("//ProjectSettings");
                sb.Append("/\" \"");
                sb.Append(Constants.PATH);
                sb.Append("ProjectSettings/\"");
                sb.AppendLine();
                sb.Append("echo");
                sb.AppendLine();
            }

            //Restart Unity
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            sb.Append("echo \"¦  Restarting Unity                                                          ¦\"");
            sb.AppendLine();
            sb.Append("echo \"+----------------------------------------------------------------------------+\"");
            sb.AppendLine();
            //sb.Append("nohup \"");
            sb.Append("\"");
            sb.Append(EditorApplication.applicationPath);
            sb.Append("\" --args -projectPath \"");
            sb.Append(Constants.PATH);
            sb.Append("\" -buildTarget ");
            sb.Append(build);
            sb.Append(" &");

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
			if (target == BuildTarget.Android)
			{
				sb.Append(" -executeMethod Crosstales.TPS.Helper.setAndroidTexture");
				//sb.Append(subTarget);
			}
			else
			{
				if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
				{
					sb.Append(" -executeMethod ");
					sb.Append(Constants.EXECUTE_METHOD);
				}
			}
#else
            if (!string.IsNullOrEmpty(Constants.EXECUTE_METHOD))
            {
                sb.Append(" -executeMethod ");
                sb.Append(Constants.EXECUTE_METHOD);
            }
#endif

            sb.AppendLine();

            //check if Unity is started
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("while [ ! -f \"");
            sb.Append(Constants.PATH);
            sb.Append("Temp/UnityLockfile\" ]");
            sb.AppendLine();
            sb.Append("do");
            sb.AppendLine();
            sb.Append("  echo \"Waiting for Unity to start...\"");
            sb.AppendLine();
            sb.Append("  sleep 2");
            sb.AppendLine();
            sb.Append("done");
            sb.AppendLine();
            sb.Append("echo");
            sb.AppendLine();
            sb.Append("echo \"Bye!\"");
            sb.AppendLine();
            sb.Append("sleep 1");
            sb.AppendLine();
            sb.Append("exit");

            return sb.ToString();
        }

#if UNITY_5
        private static string getExtension(BuildTarget target, MobileTextureSubtarget subTarget)
        {
            if (target == BuildTarget.Android && subTarget != MobileTextureSubtarget.Generic)
            {
                return "_" + subTarget;
            }

            return string.Empty;
        }
#else
		private static string getExtension(BuildTarget target, MobileTextureSubtarget subTarget)
		{
			if (target == BuildTarget.Android && subTarget != MobileTextureSubtarget.Generic)
			{
				return "_" + subTarget;
			}
			
			return string.Empty;
		}
#endif

#if UNITY_4_6 || UNITY_4_7
        public static void setAndroidTexture()
        {
            if (CTPlayerPrefs.HasKey(Constants.KEY_TEX_ANDROID))
			{
				AndroidBuildSubtarget subTarget = AndroidBuildSubtarget.Generic;

                int selectedTexture = CTPlayerPrefs.GetInt(Constants.KEY_TEX_ANDROID);

                if (selectedTexture == 1)
                {
					subTarget = AndroidBuildSubtarget.DXT;
                }
                else if (selectedTexture == 2)
                {
					subTarget = AndroidBuildSubtarget.PVRTC;
                }
                else if (selectedTexture == 3)
                {
					subTarget = AndroidBuildSubtarget.ATC;
                }
                else if (selectedTexture == 4)
                {
					subTarget = AndroidBuildSubtarget.ETC;
                }

                EditorUserBuildSettings.androidBuildSubtarget = subTarget;

                //UnityEngine.Debug.Log("new format: " + subTarget);
            }
        }
#else
        public static void setAndroidTexture()
        {
            if (CTPlayerPrefs.HasKey(Constants.KEY_TEX_ANDROID))
            {
                MobileTextureSubtarget subTarget = MobileTextureSubtarget.Generic;

                int selectedTexture = CTPlayerPrefs.GetInt(Constants.KEY_TEX_ANDROID);

                if (selectedTexture == 1)
                {
                    subTarget = MobileTextureSubtarget.DXT;
                }
                else if (selectedTexture == 2)
                {
                    subTarget = MobileTextureSubtarget.PVRTC;
                }
                else if (selectedTexture == 3)
                {
                    subTarget = MobileTextureSubtarget.ETC;
                }
                else if (selectedTexture == 4)
                {
                    subTarget = MobileTextureSubtarget.ETC;
                }
                else if (selectedTexture == 5)
                {
                    subTarget = MobileTextureSubtarget.ETC2;
                }
                else if (selectedTexture == 6)
                {
                    subTarget = MobileTextureSubtarget.ASTC;
                }

                EditorUserBuildSettings.androidBuildSubtarget = subTarget;
            }
        }
#endif

        /// <summary>Loads an image as Texture2D from 'Editor Default Resources'.</summary>
        /// <param name="logo">Logo to load.</param>
        /// <param name="fileName">Name of the image.</param>
        /// <returns>Image as Texture2D from 'Editor Default Resources'.</returns>
        private static Texture2D loadImage(ref Texture2D logo, string fileName)
        {
            if (logo == null)
            {
                //logo = (Texture2D)Resources.Load(fileName, typeof(Texture2D));

#if tps_ignore_setup
                logo = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets" + Constants.ASSET_PATH + "Icons/" + fileName, typeof(Texture2D));
#else
				logo = (Texture2D)EditorGUIUtility.Load("TPS/" + fileName);
#endif

                if (logo == null)
                {
                    Debug.LogWarning("Image not found: " + fileName);
                }
            }

            return logo;
        }

        #endregion
    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)