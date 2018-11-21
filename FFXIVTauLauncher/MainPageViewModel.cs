using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FFXIVTauLauncher.Annotations;

namespace FFXIVTauLauncher
{
    public class MainPageViewModel: INotifyPropertyChanged
    {

        private bool _loginEnabled;


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
                if (_loginEnabled != value)
                {
                    _loginEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
