using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FFXIVAPI;
using FFXIVTauLauncher.Annotations;

namespace FFXIVTauLauncher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 400));
            ApplicationView.GetForCurrentView().TryResizeView(new Size(1220, 720));
            this.ViewModel = new MainPageViewModel();
            this.DataContext = ViewModel;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            this.FfApi = new FfApi();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ButtonAccount_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoginEnabled = !ViewModel.LoginEnabled;
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
        }

        public MainPageViewModel ViewModel { get; }
        public FfApi FfApi { get; }
    }
}