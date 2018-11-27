using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVAPI.Settings
{
    public interface IConfigSet
    {
        void RememberLoginName(bool remember);
        void RememberPassword(bool remember);
        void AutoLogin(bool auto);
        void EnableOneTimePassword(bool enable);
    }
}
