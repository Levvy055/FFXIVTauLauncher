namespace FFXIVTauLauncher.Configs
{
    public interface IConfigSet
    {
        void RememberLoginName(bool remember);
        void RememberPassword(bool remember);
        void AutoLogin(bool auto);
        void EnableOneTimePassword(bool enable);
        void GamePath(string path);
    }
}
