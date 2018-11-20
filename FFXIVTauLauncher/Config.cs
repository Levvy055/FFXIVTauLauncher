using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using NLog;

namespace FFXIVTauLauncher
{
    public class Config
    {
        private static readonly string PATH = "ffxiv_launcher.conf";
        private static readonly StorageFolder StorageFolder = ApplicationData.Current.RoamingFolder;
        private static StorageFile ConfigFile;

        private static readonly Property[] Defaults =
        {
            new Property(PropertyType.SAVE_LOGIN, true),
            new Property(PropertyType.SAVE_PASSWORD, false),
        };

        public Config()
        {
            if (!File.Exists(PATH))
            {
                Init().Wait(10000);
            }
        }

        private async Task Init()
        {
            ConfigFile = await StorageFolder.CreateFileAsync(PATH, CreationCollisionOption.ReplaceExisting);
            Properties = new List<Property>(Defaults);
            Save();
            Properties = null;
        }

        public bool Save()
        {
            try
            {
                var str = JsonConvert.SerializeObject(Properties);
                var task = FileIO.WriteTextAsync(ConfigFile, str);
                task.GetResults();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during saving!");
                return false;
            }
        }

        public bool Load()
        {
            try
            {
                var str = FileIO.ReadTextAsync(ConfigFile).GetResults();
                Properties = JsonConvert.DeserializeObject<List<Property>>(str);
                return Properties != null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Cannot load setttings from file!");
                return false;
            }
        }

        private Logger Log { get; } = LogManager.GetCurrentClassLogger();

        public List<Property> Properties { get; private set; }
    }

    public class Property
    {
        public Property(PropertyType propertyType, object value)
        {
            PropertyType = propertyType;
            Value = value;
        }

        public object Value { get; set; }

        public PropertyType PropertyType { get; }
    }

    public enum PropertyType
    {
        SAVE_LOGIN, SAVE_PASSWORD
    }
}
