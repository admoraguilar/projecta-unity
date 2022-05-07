using UnityEngine;

namespace Crosstales.TPS
{
    /// <summary>Collected constants of very general utility for the asset.</summary>
    public static class Constants
    {

        #region Constant variables

        /// <summary>Name of the asset.</summary>
        public const string ASSET_NAME = "Turbo Platform Switch";

        /// <summary>Version of the asset.</summary>
        public const string ASSET_VERSION = "1.4.1";

        /// <summary>Build number of the asset.</summary>
        public const int ASSET_BUILD = 141;

        /// <summary>Create date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CREATED = new System.DateTime(2016, 9, 22);

        /// <summary>Change date of the asset (YYYY, MM, DD).</summary>
        public static readonly System.DateTime ASSET_CHANGED = new System.DateTime(2017, 4, 9);

        /// <summary>Author of the asset.</summary>
        public const string ASSET_AUTHOR = "crosstales LLC";

        /// <summary>URL of the asset author.</summary>
        public const string ASSET_AUTHOR_URL = "https://www.crosstales.com";

        /// <summary>URL of the crosstales assets in UAS.</summary>
        public const string ASSET_CT_URL = "https://www.assetstore.unity3d.com/#!/list/42213-crosstales?aid=1011lNGT"; // crosstales list

        /// <summary>ID of the asset in the UAS.</summary>
        public const string ASSET_ID = "60040";

        /// <summary>URL of the asset in the UAS.</summary>
		public const string ASSET_URL = "https://www.assetstore.unity3d.com/#!/content/60040?aid=1011lNGT";

        /// <summary>URL for update-checks of the asset</summary>
        public const string ASSET_UPDATE_CHECK_URL = "https://www.crosstales.com/media/assets/tps_versions.txt";

        /// <summary>Contact to the owner of the asset.</summary>
        public const string ASSET_CONTACT = "tps@crosstales.com";

        /// <summary>UID of the asset.</summary>
        public static readonly System.Guid ASSET_UID = new System.Guid("2d03d693-219a-4fa4-a9b0-83e5a59ebe01");

        /// <summary>URL of the asset manual.</summary>
		public const string ASSET_MANUAL_URL = "https://www.crosstales.com/media/data/assets/tps/TPS-doc.pdf";

        /// <summary>URL of the asset API.</summary>
		public const string ASSET_API_URL = "https://goo.gl/NDTja0"; // checked: 08.03.2017
        //public const string ASSET_API_URL = "http://www.crosstales.com/en/assets/tps/api/";

        /// <summary>URL of the asset forum.</summary>
        public const string ASSET_FORUM_URL = "https://goo.gl/d7SjL2"; // checked: 08.03.2017
        //public const string ASSET_FORUM_URL = "https://forum.unity3d.com/threads/turbo-platform-switch.434860/";

        /// <summary>URL of the asset in crosstales.</summary>
        public const string ASSET_WEB_URL = "https://www.crosstales.com/en/portfolio/tps/";

        // Keys for the configuration of the asset
        private const string KEY_PREFIX = "TPS_CFG_";
        public const string KEY_CUSTOM_PATH_CACHE = KEY_PREFIX + "CUSTOM_PATH_CACHE";
        public const string KEY_PATH_CACHE = KEY_PREFIX + "PATH_CACHE";
        public const string KEY_VCS = KEY_PREFIX + "VCS";
        public const string KEY_EXECUTE_METHOD = KEY_PREFIX + "EXECUTE_METHOD";
        public const string KEY_COPY_SETTINGS = KEY_PREFIX + "COPY_SETTINGS";
        public const string KEY_CONFIRM_SWITCH = KEY_PREFIX + "CONFIRM_SWITCH";
        public const string KEY_DEBUG = KEY_PREFIX + "DEBUG";
        public const string KEY_UPDATE_CHECK = KEY_PREFIX + "UPDATE_CHECK";
        public const string KEY_UPDATE_OPEN_UAS = KEY_PREFIX + "UPDATE_OPEN_UAS";
        public const string KEY_UPDATE_DATE = KEY_PREFIX + "UPDATE_DATE";

        public const string KEY_PLATFORM_WINDOWS = KEY_PREFIX + "PLATFORM_WINDOWS";
        public const string KEY_PLATFORM_MAC = KEY_PREFIX + "PLATFORM_MAC";
        public const string KEY_PLATFORM_LINUX = KEY_PREFIX + "PLATFORM_LINUX";
        public const string KEY_PLATFORM_ANDROID = KEY_PREFIX + "PLATFORM_ANDROID";
        public const string KEY_PLATFORM_IOS = KEY_PREFIX + "PLATFORM_IOS";
        public const string KEY_PLATFORM_WSA = KEY_PREFIX + "PLATFORM_WSA";
        public const string KEY_PLATFORM_WEBPLAYER = KEY_PREFIX + "PLATFORM_WEBPLAYER";
        public const string KEY_PLATFORM_WEBGL = KEY_PREFIX + "PLATFORM_WEBGL";
        public const string KEY_PLATFORM_TVOS = KEY_PREFIX + "PLATFORM_TVOS";
        public const string KEY_PLATFORM_TIZEN = KEY_PREFIX + "PLATFORM_TIZEN";
        public const string KEY_PLATFORM_SAMSUNGTV = KEY_PREFIX + "PLATFORM_SAMSUNGTV";
        public const string KEY_PLATFORM_PS3 = KEY_PREFIX + "PLATFORM_PS3";
        public const string KEY_PLATFORM_PS4 = KEY_PREFIX + "PLATFORM_PS4";
        public const string KEY_PLATFORM_PSP2 = KEY_PREFIX + "PLATFORM_PSP2";
        public const string KEY_PLATFORM_XBOX360 = KEY_PREFIX + "PLATFORM_XBOX360";
        public const string KEY_PLATFORM_XBOXONE = KEY_PREFIX + "PLATFORM_XBOXONE";
        public const string KEY_PLATFORM_WIIU = KEY_PREFIX + "PLATFORM_WIIU";
        public const string KEY_PLATFORM_3DS = KEY_PREFIX + "PLATFORM_3DS";
        public const string KEY_PLATFORM_SWITCH = KEY_PREFIX + "PLATFORM_SWITCH";

        public const string KEY_ARCH_WINDOWS = KEY_PREFIX + "ARCH_WINDOWS";
        public const string KEY_ARCH_MAC = KEY_PREFIX + "ARCH_MAC";
        public const string KEY_ARCH_LINUX = KEY_PREFIX + "ARCH_LINUX";

        public const string KEY_TEX_ANDROID = KEY_PREFIX + "TEX_ANDROID";

        public const string KEY_SHOW_COLUMN_PLATFORM = KEY_PREFIX + "SHOW_COLUMN_PLATFORM";
        public const string KEY_SHOW_COLUMN_ARCHITECTURE = KEY_PREFIX + "SHOW_COLUMN_ARCHITECTURE";
        public const string KEY_SHOW_COLUMN_TEXTURE = KEY_PREFIX + "SHOW_COLUMN_TEXTURE";
        public const string KEY_SHOW_COLUMN_CACHE = KEY_PREFIX + "SHOW_COLUMN_CACHE";

        public const string CACHE_DIRNAME = "TPS_cache";

        /// <summary>Application path.</summary>
        public static readonly string PATH = Helper.ValidatePath(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1));

        // Default values
        public static readonly string DEFAULT_PATH_CACHE = Helper.ValidatePath(PATH + CACHE_DIRNAME);
        public const bool DEFAULT_CUSTOM_PATH_CACHE = false;
        public const int DEFAULT_VCS = 1; //git
        public const bool DEFAULT_COPY_SETTINGS = true;
        public const bool DEFAULT_CONFIRM_SWITCH = true;
        public const bool DEFAULT_DEBUG = false;
        public const bool DEFAULT_UPDATE_CHECK = true;
        public const bool DEFAULT_UPDATE_OPEN_UAS = false;
        public const bool DEFAULT_PLATFORM_WINDOWS = true;
        public const bool DEFAULT_PLATFORM_MAC = true;
        public const bool DEFAULT_PLATFORM_LINUX = true;
        public const bool DEFAULT_PLATFORM_ANDROID = true;
        public const bool DEFAULT_PLATFORM_IOS = true;
        public const bool DEFAULT_PLATFORM_WSA = false;
        public const bool DEFAULT_PLATFORM_WEBPLAYER = false;
        public const bool DEFAULT_PLATFORM_WEBGL = true;
        public const bool DEFAULT_PLATFORM_TVOS = false;
        public const bool DEFAULT_PLATFORM_TIZEN = false;
        public const bool DEFAULT_PLATFORM_SAMSUNGTV = false;
        public const bool DEFAULT_PLATFORM_PS3 = false;
        public const bool DEFAULT_PLATFORM_PS4 = false;
        public const bool DEFAULT_PLATFORM_PSP2 = false;
        public const bool DEFAULT_PLATFORM_XBOX360 = false;
        public const bool DEFAULT_PLATFORM_XBOXONE = false;
        public const bool DEFAULT_PLATFORM_WIIU = false;
        public const bool DEFAULT_PLATFORM_3DS = false;
        public const bool DEFAULT_PLATFORM_SWITCH = false;
        public const int DEFAULT_ARCH_WINDOWS = 0;
        public const int DEFAULT_ARCH_MAC = 0;
        public const int DEFAULT_ARCH_LINUX = 0;
        public const int DEFAULT_TEX_ANDROID = 0;
        public const bool DEFAULT_SHOW_COLUMN_PLATFORM = true;
        public const bool DEFAULT_SHOW_COLUMN_PLATFORM_LOGO = false;
        public const bool DEFAULT_SHOW_COLUMN_ARCHITECTURE = true;
        public const bool DEFAULT_SHOW_COLUMN_TEXTURE = false;
        public const bool DEFAULT_SHOW_COLUMN_CACHE = true;

        #endregion


        #region Changable variables

        /// <summary>Path to the asset inside the Unity project.</summary>
        public static string ASSET_PATH = "/crosstales/TPS/";

        /// <summary>Enable or disable custom location for the cache.</summary>
        public static bool CUSTOM_PATH_CACHE = DEFAULT_CUSTOM_PATH_CACHE;

        /// <summary>TPS-cache path.</summary>
        private static string pathCache = DEFAULT_PATH_CACHE;
        public static string PATH_CACHE
        {
            get { return CUSTOM_PATH_CACHE && !string.IsNullOrEmpty(pathCache) ? Helper.ValidatePath(pathCache) : DEFAULT_PATH_CACHE; }
            set { pathCache = value; }
        }

        /// <summary>Selected VCS-system (default: 0, 0 = none, 1 = git, 2 = SVN, 3 Mercurial).</summary>
        public static int VCS = DEFAULT_VCS;

        /// <summary>Execute static method <ClassName.MethodName> in Unity after a switch.</summary>
        public static string EXECUTE_METHOD = string.Empty;

        /// <summary>Enable or disable copying the 'ProjectSettings'-folder.</summary>
        public static bool COPY_SETTINGS = DEFAULT_COPY_SETTINGS;

        /// <summary>Enable or disable the switch confirmation dialog.</summary>
        public static bool CONFIRM_SWITCH = DEFAULT_CONFIRM_SWITCH;

        /// <summary>Enable or disable debug logging for the asset.</summary>
        public static bool DEBUG = DEFAULT_DEBUG;

        /// <summary>Enable or disable update-checks for the asset.</summary>
        public static bool UPDATE_CHECK = DEFAULT_UPDATE_CHECK;

        /// <summary>Open the UAS-site when an update is found.</summary>
        public static bool UPDATE_OPEN_UAS = DEFAULT_UPDATE_OPEN_UAS;

        /// <summary>Enable or disable the Windows platform.</summary>
        public static bool PLATFORM_WINDOWS = DEFAULT_PLATFORM_WINDOWS;

        /// <summary>Enable or disable the macOS platform.</summary>
        public static bool PLATFORM_MAC = DEFAULT_PLATFORM_MAC;

        /// <summary>Enable or disable the Linux platform.</summary>
        public static bool PLATFORM_LINUX = DEFAULT_PLATFORM_LINUX;

        /// <summary>Enable or disable the Android platform.</summary>
        public static bool PLATFORM_ANDROID = DEFAULT_PLATFORM_ANDROID;

        /// <summary>Enable or disable the iOS platform.</summary>
        public static bool PLATFORM_IOS = DEFAULT_PLATFORM_IOS;

        /// <summary>Enable or disable the WSA platform.</summary>
        public static bool PLATFORM_WSA = DEFAULT_PLATFORM_WSA;

        /// <summary>Enable or disable the WebPlayer platform.</summary>
        public static bool PLATFORM_WEBPLAYER = DEFAULT_PLATFORM_WEBPLAYER;

        /// <summary>Enable or disable the WebGL platform.</summary>
        public static bool PLATFORM_WEBGL = DEFAULT_PLATFORM_WEBGL;

        /// <summary>Enable or disable the tvOS platform.</summary>
        public static bool PLATFORM_TVOS = DEFAULT_PLATFORM_TVOS;

        /// <summary>Enable or disable the Tizen platform.</summary>
        public static bool PLATFORM_TIZEN = DEFAULT_PLATFORM_TIZEN;

        /// <summary>Enable or disable the SamsungTV platform.</summary>
        public static bool PLATFORM_SAMSUNGTV = DEFAULT_PLATFORM_SAMSUNGTV;

        /// <summary>Enable or disable the PS3 platform.</summary>
        public static bool PLATFORM_PS3 = DEFAULT_PLATFORM_PS3;

        /// <summary>Enable or disable the PS4 platform.</summary>
        public static bool PLATFORM_PS4 = DEFAULT_PLATFORM_PS4;

        /// <summary>Enable or disable the PSP2 (Vita) platform.</summary>
        public static bool PLATFORM_PSP2 = DEFAULT_PLATFORM_PSP2;

        /// <summary>Enable or disable the XBox360 platform.</summary>
        public static bool PLATFORM_XBOX360 = DEFAULT_PLATFORM_XBOX360;

        /// <summary>Enable or disable the XBoxOne platform.</summary>
        public static bool PLATFORM_XBOXONE = DEFAULT_PLATFORM_XBOXONE;

        /// <summary>Enable or disable the WiiU platform.</summary>
        public static bool PLATFORM_WIIU = DEFAULT_PLATFORM_WIIU;

        /// <summary>Enable or disable the 3DS platform.</summary>
        public static bool PLATFORM_3DS = DEFAULT_PLATFORM_3DS;

        /// <summary>Enable or disable the Nintendo Switch platform.</summary>
        public static bool PLATFORM_SWITCH = DEFAULT_PLATFORM_SWITCH;

        /// <summary>Architecture of the Windows platform.</summary>
        public static int ARCH_WINDOWS = DEFAULT_ARCH_WINDOWS;

        /// <summary>Architecture of the macOS platform.</summary>
        public static int ARCH_MAC = DEFAULT_ARCH_MAC;

        /// <summary>Architecture of the Linux platform.</summary>
        public static int ARCH_LINUX = DEFAULT_ARCH_LINUX;

        /// <summary>Texture format of the Android platform.</summary>
        public static int TEX_ANDROID = DEFAULT_TEX_ANDROID;

        /// <summary>Shows or hides the delete button for the cache.</summary>
        public static bool SHOW_DELETE = false;

        /// <summary>Shows or hides the column for the platform.</summary>
        public static bool SHOW_COLUMN_PLATFORM = DEFAULT_SHOW_COLUMN_PLATFORM;

        /// <summary>Shows or hides the column for the platform.</summary>
        public static bool SHOW_COLUMN_PLATFORM_LOGO = DEFAULT_SHOW_COLUMN_PLATFORM_LOGO;

        /// <summary>Shows or hides the column for the architecture.</summary>
        public static bool SHOW_COLUMN_ARCHITECTURE = DEFAULT_SHOW_COLUMN_ARCHITECTURE;

        /// <summary>Shows or hides the column for the texture format.</summary>
        public static bool SHOW_COLUMN_TEXTURE = DEFAULT_SHOW_COLUMN_TEXTURE;

        /// <summary>Shows or hides the column for the cache.</summary>
        public static bool SHOW_COLUMN_CACHE = DEFAULT_SHOW_COLUMN_CACHE;

        // Technical settings
        /// <summary>Kill processes after 3000 milliseconds.</summary>
        public static int KILL_TIME = 3000;

        #endregion

        #region Public static methods

        /// <summary>Resets all changable variables to their default value.</summary>
        public static void Reset()
        {
            CUSTOM_PATH_CACHE = DEFAULT_CUSTOM_PATH_CACHE;
            pathCache = DEFAULT_PATH_CACHE;
            VCS = DEFAULT_VCS;
            EXECUTE_METHOD = string.Empty;
            COPY_SETTINGS = DEFAULT_COPY_SETTINGS;
            CONFIRM_SWITCH = DEFAULT_CONFIRM_SWITCH;
            DEBUG = DEFAULT_DEBUG;
            UPDATE_CHECK = DEFAULT_UPDATE_CHECK;
            UPDATE_OPEN_UAS = DEFAULT_UPDATE_OPEN_UAS;
            PLATFORM_WINDOWS = DEFAULT_PLATFORM_WINDOWS;
            PLATFORM_MAC = DEFAULT_PLATFORM_MAC;
            PLATFORM_LINUX = DEFAULT_PLATFORM_LINUX;
            PLATFORM_ANDROID = DEFAULT_PLATFORM_ANDROID;
            PLATFORM_IOS = DEFAULT_PLATFORM_IOS;
            PLATFORM_WSA = DEFAULT_PLATFORM_WSA;
            PLATFORM_WEBPLAYER = DEFAULT_PLATFORM_WEBPLAYER;
            PLATFORM_WEBGL = DEFAULT_PLATFORM_WEBGL;
            PLATFORM_TVOS = DEFAULT_PLATFORM_TVOS;
            PLATFORM_TIZEN = DEFAULT_PLATFORM_TIZEN;
            PLATFORM_SAMSUNGTV = DEFAULT_PLATFORM_SAMSUNGTV;
            PLATFORM_PS3 = DEFAULT_PLATFORM_PS3;
            PLATFORM_PS4 = DEFAULT_PLATFORM_PS4;
            PLATFORM_PSP2 = DEFAULT_PLATFORM_PSP2;
            PLATFORM_XBOX360 = DEFAULT_PLATFORM_XBOX360;
            PLATFORM_XBOXONE = DEFAULT_PLATFORM_XBOXONE;
            PLATFORM_WIIU = DEFAULT_PLATFORM_WIIU;
            PLATFORM_3DS = DEFAULT_PLATFORM_3DS;
            PLATFORM_SWITCH = DEFAULT_PLATFORM_SWITCH;
            ARCH_WINDOWS = DEFAULT_ARCH_WINDOWS;
            ARCH_MAC = DEFAULT_ARCH_MAC;
            ARCH_LINUX = DEFAULT_ARCH_LINUX;
            TEX_ANDROID = DEFAULT_TEX_ANDROID;
            SHOW_COLUMN_PLATFORM = DEFAULT_SHOW_COLUMN_PLATFORM;
            SHOW_COLUMN_PLATFORM_LOGO = DEFAULT_SHOW_COLUMN_PLATFORM_LOGO;
            SHOW_COLUMN_ARCHITECTURE = DEFAULT_SHOW_COLUMN_ARCHITECTURE;
            SHOW_COLUMN_TEXTURE = DEFAULT_SHOW_COLUMN_TEXTURE;
            SHOW_COLUMN_CACHE = DEFAULT_SHOW_COLUMN_CACHE;
        }

        /// <summary>Loads the all changable variables.</summary>
        public static void Load()
        {
            if (CTPlayerPrefs.HasKey(KEY_CUSTOM_PATH_CACHE))
            {
                CUSTOM_PATH_CACHE = CTPlayerPrefs.GetBool(KEY_CUSTOM_PATH_CACHE);
            }

            if (CTPlayerPrefs.HasKey(KEY_PATH_CACHE))
            {
                PATH_CACHE = CTPlayerPrefs.GetString(KEY_PATH_CACHE);
            }

            if (CTPlayerPrefs.HasKey(KEY_VCS))
            {
                VCS = CTPlayerPrefs.GetInt(KEY_VCS);
            }

            if (CTPlayerPrefs.HasKey(KEY_EXECUTE_METHOD))
            {
                EXECUTE_METHOD = CTPlayerPrefs.GetString(KEY_EXECUTE_METHOD);
            }

            if (CTPlayerPrefs.HasKey(KEY_COPY_SETTINGS))
            {
                COPY_SETTINGS = CTPlayerPrefs.GetBool(KEY_COPY_SETTINGS);
            }

            if (CTPlayerPrefs.HasKey(KEY_CONFIRM_SWITCH))
            {
                CONFIRM_SWITCH = CTPlayerPrefs.GetBool(KEY_CONFIRM_SWITCH);
            }

            if (CTPlayerPrefs.HasKey(KEY_DEBUG))
            {
                DEBUG = CTPlayerPrefs.GetBool(KEY_DEBUG);
            }

            if (CTPlayerPrefs.HasKey(KEY_UPDATE_CHECK))
            {
                UPDATE_CHECK = CTPlayerPrefs.GetBool(KEY_UPDATE_CHECK);
            }

            if (CTPlayerPrefs.HasKey(KEY_UPDATE_OPEN_UAS))
            {
                UPDATE_OPEN_UAS = CTPlayerPrefs.GetBool(KEY_UPDATE_OPEN_UAS);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_WINDOWS))
            {
                PLATFORM_WINDOWS = CTPlayerPrefs.GetBool(KEY_PLATFORM_WINDOWS);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_MAC))
            {
                PLATFORM_MAC = CTPlayerPrefs.GetBool(KEY_PLATFORM_MAC);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_LINUX))
            {
                PLATFORM_LINUX = CTPlayerPrefs.GetBool(KEY_PLATFORM_LINUX);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_ANDROID))
            {
                PLATFORM_ANDROID = CTPlayerPrefs.GetBool(KEY_PLATFORM_ANDROID);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_IOS))
            {
                PLATFORM_IOS = CTPlayerPrefs.GetBool(KEY_PLATFORM_IOS);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_WSA))
            {
                PLATFORM_WSA = CTPlayerPrefs.GetBool(KEY_PLATFORM_WSA);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_WEBPLAYER))
            {
                PLATFORM_WEBPLAYER = CTPlayerPrefs.GetBool(KEY_PLATFORM_WEBPLAYER);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_WEBGL))
            {
                PLATFORM_WEBGL = CTPlayerPrefs.GetBool(KEY_PLATFORM_WEBGL);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_TVOS))
            {
                PLATFORM_TVOS = CTPlayerPrefs.GetBool(KEY_PLATFORM_TVOS);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_TIZEN))
            {
                PLATFORM_TIZEN = CTPlayerPrefs.GetBool(KEY_PLATFORM_TIZEN);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_SAMSUNGTV))
            {
                PLATFORM_SAMSUNGTV = CTPlayerPrefs.GetBool(KEY_PLATFORM_SAMSUNGTV);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_PS3))
            {
                PLATFORM_PS3 = CTPlayerPrefs.GetBool(KEY_PLATFORM_PS3);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_PS4))
            {
                PLATFORM_PS4 = CTPlayerPrefs.GetBool(KEY_PLATFORM_PS4);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_PSP2))
            {
                PLATFORM_PSP2 = CTPlayerPrefs.GetBool(KEY_PLATFORM_PSP2);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_XBOX360))
            {
                PLATFORM_XBOX360 = CTPlayerPrefs.GetBool(KEY_PLATFORM_XBOX360);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_XBOXONE))
            {
                PLATFORM_XBOXONE = CTPlayerPrefs.GetBool(KEY_PLATFORM_XBOXONE);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_WIIU))
            {
                PLATFORM_WIIU = CTPlayerPrefs.GetBool(KEY_PLATFORM_WIIU);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_3DS))
            {
                PLATFORM_3DS = CTPlayerPrefs.GetBool(KEY_PLATFORM_3DS);
            }

            if (CTPlayerPrefs.HasKey(KEY_PLATFORM_SWITCH))
            {
                PLATFORM_SWITCH = CTPlayerPrefs.GetBool(KEY_PLATFORM_SWITCH);
            }

            if (CTPlayerPrefs.HasKey(KEY_ARCH_WINDOWS))
            {
                ARCH_WINDOWS = CTPlayerPrefs.GetInt(KEY_ARCH_WINDOWS);
            }

            if (CTPlayerPrefs.HasKey(KEY_ARCH_MAC))
            {
                ARCH_MAC = CTPlayerPrefs.GetInt(KEY_ARCH_MAC);
            }

            if (CTPlayerPrefs.HasKey(KEY_ARCH_LINUX))
            {
                ARCH_LINUX = CTPlayerPrefs.GetInt(KEY_ARCH_LINUX);
            }

            if (CTPlayerPrefs.HasKey(KEY_TEX_ANDROID))
            {
                TEX_ANDROID = CTPlayerPrefs.GetInt(KEY_TEX_ANDROID);
            }

            if (CTPlayerPrefs.HasKey(KEY_SHOW_COLUMN_PLATFORM))
            {
                SHOW_COLUMN_PLATFORM = CTPlayerPrefs.GetBool(KEY_SHOW_COLUMN_PLATFORM);
            }

            if (CTPlayerPrefs.HasKey(KEY_SHOW_COLUMN_ARCHITECTURE))
            {
                SHOW_COLUMN_ARCHITECTURE = CTPlayerPrefs.GetBool(KEY_SHOW_COLUMN_ARCHITECTURE);
            }

            if (CTPlayerPrefs.HasKey(KEY_SHOW_COLUMN_TEXTURE))
            {
                SHOW_COLUMN_TEXTURE = CTPlayerPrefs.GetBool(KEY_SHOW_COLUMN_TEXTURE);
            }

            if (CTPlayerPrefs.HasKey(KEY_SHOW_COLUMN_CACHE))
            {
                SHOW_COLUMN_CACHE = CTPlayerPrefs.GetBool(KEY_SHOW_COLUMN_CACHE);
            }
        }

        /// <summary>Saves the all changable variables.</summary>
        public static void Save()
        {
            CTPlayerPrefs.SetBool(KEY_CUSTOM_PATH_CACHE, CUSTOM_PATH_CACHE);
            CTPlayerPrefs.SetString(KEY_PATH_CACHE, PATH_CACHE);
            CTPlayerPrefs.SetInt(KEY_VCS, VCS);
            CTPlayerPrefs.SetString(KEY_EXECUTE_METHOD, EXECUTE_METHOD);
            CTPlayerPrefs.SetBool(KEY_COPY_SETTINGS, COPY_SETTINGS);
            CTPlayerPrefs.SetBool(KEY_CONFIRM_SWITCH, CONFIRM_SWITCH);
            CTPlayerPrefs.SetBool(KEY_DEBUG, DEBUG);
            CTPlayerPrefs.SetBool(KEY_UPDATE_CHECK, UPDATE_CHECK);
            CTPlayerPrefs.SetBool(KEY_UPDATE_OPEN_UAS, UPDATE_OPEN_UAS);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_WINDOWS, PLATFORM_WINDOWS);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_MAC, PLATFORM_MAC);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_LINUX, PLATFORM_LINUX);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_ANDROID, PLATFORM_ANDROID);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_IOS, PLATFORM_IOS);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_WSA, PLATFORM_WSA);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_WEBPLAYER, PLATFORM_WEBPLAYER);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_WEBGL, PLATFORM_WEBGL);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_TVOS, PLATFORM_TVOS);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_TIZEN, PLATFORM_TIZEN);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_SAMSUNGTV, PLATFORM_SAMSUNGTV);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_PS3, PLATFORM_PS3);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_PS4, PLATFORM_PS4);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_PSP2, PLATFORM_PSP2);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_XBOX360, PLATFORM_XBOX360);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_XBOXONE, PLATFORM_XBOXONE);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_WIIU, PLATFORM_WIIU);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_3DS, PLATFORM_3DS);
            CTPlayerPrefs.SetBool(KEY_PLATFORM_SWITCH, PLATFORM_SWITCH);
            CTPlayerPrefs.SetInt(KEY_ARCH_WINDOWS, ARCH_WINDOWS);
            CTPlayerPrefs.SetInt(KEY_ARCH_MAC, ARCH_MAC);
            CTPlayerPrefs.SetInt(KEY_ARCH_LINUX, ARCH_LINUX);
            CTPlayerPrefs.SetInt(KEY_TEX_ANDROID, TEX_ANDROID);
            CTPlayerPrefs.SetBool(KEY_SHOW_COLUMN_PLATFORM, SHOW_COLUMN_PLATFORM);
            CTPlayerPrefs.SetBool(KEY_SHOW_COLUMN_ARCHITECTURE, SHOW_COLUMN_ARCHITECTURE);
            CTPlayerPrefs.SetBool(KEY_SHOW_COLUMN_TEXTURE, SHOW_COLUMN_TEXTURE);
            CTPlayerPrefs.SetBool(KEY_SHOW_COLUMN_CACHE, SHOW_COLUMN_CACHE);

            CTPlayerPrefs.Save();
        }

        #endregion

    }
}
// © 2016-2017 crosstales LLC (https://www.crosstales.com)