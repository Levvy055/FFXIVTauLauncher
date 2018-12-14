namespace FFXIVAPI.Configs
{
    public interface IConfigGet
    {
        bool RememberLoginName();
        bool RememberPassword();
        bool AutoLogin();
        bool EnableOneTimePassword();
        string GamePath();
    }
}
