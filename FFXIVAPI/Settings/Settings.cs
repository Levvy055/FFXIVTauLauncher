using Windows.Storage;
using NLog;

namespace FFXIVAPI.Settings
{
    public static class Settings
    {
        private static readonly string PREFIX = "ffxiv_launcher";
        private static bool _initialized = false;
        private static int _settingsVersion = 1;
        private static readonly ApplicationDataContainer _roamingSettings = ApplicationData.Current.RoamingSettings;
        private static readonly StorageFolder _roamingFolder = ApplicationData.Current.RoamingFolder;

        public static void Init()
        {
            if (_initialized)
            { return; }

            if (!_roamingSettings.Values.ContainsKey(PREFIX))
            {
                var composite = CreateNewSettingsComposite();
                _roamingSettings.Values[PREFIX] = composite;
            }
            var conf = (ApplicationDataCompositeValue)_roamingSettings.Values[PREFIX];
            if (conf.ContainsKey("VERSION"))
            {
                var v = (int)conf["VERSION"];
                if (v < _settingsVersion)
                {
                    Log.Info($"Older config found! Migrating {v} -> {_settingsVersion}");
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
            c["VERSION"] = _settingsVersion;
            return c;
        }

        private static void ApplyMigration(ApplicationDataCompositeValue conf)
        {
            //TODO:Future Migrations

            conf["VERSION"] = _settingsVersion;
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
