using Windows.Storage;
using NLog;

namespace FFXIVTauLauncher.Configs
{
    /// <summary>
    /// Main class to manage all app settings
    /// </summary>
    public static class Settings
    {
        private const string PREFIX = "ffxiv_launcher";
        private const int SETTINGS_VERSION = 1;

        private static readonly ApplicationDataContainer _roamingSettings = ApplicationData.Current.RoamingSettings;
        //private static readonly StorageFolder _roamingFolder = ApplicationData.Current.RoamingFolder;

        public static void Init()
        {
            if (!_roamingSettings.Values.ContainsKey(PREFIX))
            {
                var composite = CreateNewSettingsComposite();
                _roamingSettings.Values[PREFIX] = composite;
            }
            var conf = (ApplicationDataCompositeValue)_roamingSettings.Values[PREFIX];
            if (conf.ContainsKey("VERSION"))
            {
                var v = (int)conf["VERSION"];
                if (v < SETTINGS_VERSION)
                {
                    Log.Info($"Older config found! Migrating {v} -> {SETTINGS_VERSION}");
                    ApplyMigration(conf);
                }
            }
            Config = new Config(conf);
        }

        private static ApplicationDataCompositeValue CreateNewSettingsComposite()
        {
            var c = new ApplicationDataCompositeValue();
            var tc = new Config(c) as IConfigSet;
            tc.AutoLogin(false);
            tc.EnableOneTimePassword(false);
            tc.RememberLoginName(false);
            tc.RememberPassword(false);
            c["VERSION"] = SETTINGS_VERSION;
            return c;
        }

        private static void ApplyMigration(ApplicationDataCompositeValue conf)
        {
            //TODO:Future Migrations

            conf["VERSION"] = SETTINGS_VERSION;
        }

        public static IConfigGet Get()
        {
            return Config;
        }

        public static IConfigSet Set()
        {
            return Config;
        }

        public static void Save()
        {
            _roamingSettings.Values[PREFIX] = Config.Container;
        }

        public static void RestoreDefaults()
        {
            Log.Info("Restoring defaults!");
            if (_roamingSettings.Values.ContainsKey(PREFIX))
            {
                _roamingSettings.Values.Remove(PREFIX);
            }
            Init();
        }

        private static Config Config { get; set; }
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();
    }
}
