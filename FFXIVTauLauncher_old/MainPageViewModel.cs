using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FFXIVAPI.Configs;
using FFXIVTauLauncher.Annotations;

namespace FFXIVTauLauncher
{
    /// <summary>
    /// A ViewModel from MVVM arch to <see cref="MainPage"/> class
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _loginEnabled;
        private bool _loginRemember;
        private bool _pswdRemember;
        private bool _oneTimePswdEnabled;
        private bool _autoLoginEnabled;
        private string _loginName;

        public void LoadConfig()
        {
            AutoLoginEnabled = Settings.Get().AutoLogin();
            LoginRemember = Settings.Get().RememberLoginName();
            PswdRemember = Settings.Get().RememberPassword();
            OneTimePswdEnabled = Settings.Get().EnableOneTimePassword();
        }

        public void SaveToConfig()
        {
            Settings.Set().AutoLogin(AutoLoginEnabled);
            Settings.Set().RememberLoginName(LoginRemember);
            Settings.Set().RememberPassword(PswdRemember);
            Settings.Set().EnableOneTimePassword(OneTimePswdEnabled);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool LoginEnabled
        {
            get => _loginEnabled;
            set
            {
                if (_loginEnabled == value) { return; }
                _loginEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool LoginRemember
        {
            get => _loginRemember;
            set
            {
                if (_loginRemember == value) { return; }
                _loginRemember = value;
                OnPropertyChanged();
            }
        }

        public bool PswdRemember
        {
            get => _pswdRemember;
            set
            {
                if (_pswdRemember == value) { return; }
                _pswdRemember = value;
                OnPropertyChanged();
            }
        }


        public bool OneTimePswdEnabled
        {
            get => _oneTimePswdEnabled;
            set
            {
                if (_oneTimePswdEnabled == value) { return; }
                _oneTimePswdEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool AutoLoginEnabled
        {
            get => _autoLoginEnabled;
            set
            {
                if (_autoLoginEnabled == value) { return; }
                _autoLoginEnabled = value;
                OnPropertyChanged();
            }
        }

        public string LoginName
        {
            get => _loginName;
            set
            {
                if (value == null) { value = ""; }
                if (value.Equals(_loginName)) { return; }
                _loginName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
