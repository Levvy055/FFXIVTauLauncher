using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using NLog;
using NLog.Fluent;

namespace FFXIVAPI.Settings
{
    internal class Config : IConfigGet, IConfigSet
    {
        public Config(ApplicationDataCompositeValue container)
        {
            this.Container = container;
        }

        private object DoGet(string key)
        {
            try
            {
                return Container[key];
            }
            catch (NullReferenceException ex)
            {
                Log.Error(ex, $"Cannot read setting: {key}!");
                throw;
            }
        }

        public bool RememberLoginName()
        {
            return (bool)DoGet("rem_login");
        }


        public bool RememberPassword()
        {
            return (bool)DoGet("rem_pswd");
        }

        public bool AutoLogin()
        {
            return (bool)DoGet("auto_login");
        }

        public bool EnableOneTimePassword()
        {
            return (bool)DoGet("en_ot_pswd");
        }

        public void RememberLoginName(bool remember)
        {
            Container["rem_login"] = remember;
        }

        public void RememberPassword(bool remember)
        {
            Container["rem_pswd"] = remember;
        }

        public void AutoLogin(bool auto)
        {
            Container["auto_login"] = auto;
        }

        public void EnableOneTimePassword(bool enable)
        {
            Container["en_ot_pswd"] = enable;
        }

        public ApplicationDataCompositeValue Container { get; }
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();
    }
}
