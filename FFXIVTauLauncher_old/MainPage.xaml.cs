﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FFXIVAPI;
using FFXIVAPI.Configs;
using FFXIVTauLauncher.Annotations;
using NLog;

namespace FFXIVTauLauncher
{
    /// <summary>
    /// Main Page (and only) of Launcher
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
            Loaded += OnLoaded;
            Window.Current.VisibilityChanged += OnUnloaded;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            this.FfApi = new FfApi();
            ViewModel.LoginName = "ffxivservice://location";
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

        private void OnUnloaded(object sender, VisibilityChangedEventArgs e)
        {
            if (!e.Visible)
            {
                ViewModel.SaveToConfig();
            }
        }


        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void ButtonAccount_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoginEnabled = !ViewModel.LoginEnabled;
            //await Launcher.LaunchFileAsync(new StorageFile("FFXIVGameLaunchService.exe"), new LauncherOptions { });
            Console.WriteLine(ViewModel.LoginName);
            try
            {
                var s=await Package.Current.InstalledLocation.GetFilesAsync();
                foreach (var sf in s)
                {
                    Debug.WriteLine(sf.Name);
                }
                var t = await Package.Current.InstalledLocation.GetFileAsync(ViewModel.LoginName);
                Debug.WriteLine(t);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ButtonOptions_Click(object sender, RoutedEventArgs e)
        {
        }

        public MainPageViewModel ViewModel { get; }
        public FfApi FfApi { get; }
        private static Logger Log { get; } = LogManager.GetCurrentClassLogger();
    }
}