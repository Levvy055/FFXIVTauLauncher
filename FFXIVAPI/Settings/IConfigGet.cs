using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIVAPI.Settings
{
    public interface IConfigGet
    {
        bool RememberLoginName();
        bool RememberPassword();
        bool AutoLogin();
        bool EnableOneTimePassword();
    }
}
