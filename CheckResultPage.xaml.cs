using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NEA_Solution
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckResultPage : Page
    {
        public CheckResultPage()
        {
            this.InitializeComponent();
        }

        private AcrylicBrush backBrush;
        private Windows.UI.Color tintColor;
        private ApplicationViewTitleBar titleBar;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Password password = (Password)e.Parameter;
            ScoreTextBlock.Text = password.Score.ToString();
            
            titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;

            backBrush = new AcrylicBrush() { BackgroundSource = AcrylicBackgroundSource.HostBackdrop, TintColor = (Windows.UI.Color)App.Current.Resources["SystemChromeMediumColor"], TintOpacity = 0.7, TintTransitionDuration = new TimeSpan(0, 0, 2) };
            switch (password.StrengthTier)
            {
                case PasswordStrengthTier.Weak:
                    Background = backBrush;
                    tintColor = Windows.UI.Colors.Red;
                    ScoreTextBlock.Foreground = new AcrylicBrush() {BackgroundSource = AcrylicBackgroundSource.HostBackdrop, TintColor = Windows.UI.Colors.Red, TintOpacity = 0.7, FallbackColor = Windows.UI.Colors.Red };
                    StrengthTierTextBlock.Text = "WEAK";
                    break;
                case PasswordStrengthTier.Medium:
                    Background = backBrush;
                    tintColor = Windows.UI.Colors.Orange;
                    ScoreTextBlock.Foreground = new AcrylicBrush() { BackgroundSource = AcrylicBackgroundSource.HostBackdrop, TintColor = Windows.UI.Colors.Orange, TintOpacity = 0.7, FallbackColor = Windows.UI.Colors.Orange };
                    StrengthTierTextBlock.Text = "MEDIUM STRENGTH";
                    break;
                case PasswordStrengthTier.Strong:
                    Background = backBrush;
                    tintColor = Windows.UI.Colors.Green;
                    ScoreTextBlock.Foreground = new AcrylicBrush() { BackgroundSource = AcrylicBackgroundSource.HostBackdrop, TintColor = Windows.UI.Colors.Green, TintOpacity = 0.7, FallbackColor = Windows.UI.Colors.Green };
                    StrengthTierTextBlock.Text = "STRONG";
                    break;
            }

            DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0) };
            timer.Tick += (s, ev) => { backBrush.TintColor = tintColor; backBrush.FallbackColor = tintColor; timer.Stop(); };
            timer.Start();

        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = Frame.BackStack.First();
            Frame.BackStack.Clear();
            Frame.BackStack.Add(mainPage);
            Frame.GoBack();
            titleBar.ButtonForegroundColor = (Windows.UI.Color)App.Current.Resources["SystemBaseHighColor"];
        }

    }
}
