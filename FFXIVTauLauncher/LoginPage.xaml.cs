using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.UI.Core;
using FFXIVTauLauncher.Configs;
using NLog.Fluent;

namespace FFXIVTauLauncher
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public MainPageViewModel ViewModel { get; }

        public LoginPage()
        {
            InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            this.DataContext = ViewModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.LoadConfig();
            }
            catch (NullReferenceException)
            {
                Settings.RestoreDefaults();
                try
                {
                    ViewModel.LoadConfig();
                }
                catch (NullReferenceException)
                {
                    Log.Warn("Cannot read config after second try. Giving up :/");
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveToConfig();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonAccount_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
