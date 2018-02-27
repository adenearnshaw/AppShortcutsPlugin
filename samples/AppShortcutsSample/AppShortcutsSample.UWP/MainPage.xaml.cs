using System;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms.Platform.UWP;

namespace AppShortcutsSample.UWP
{
    public sealed partial class MainPage : WindowsPage
    {
        App _formsApp;

        public MainPage()
        {
            this.InitializeComponent();

            _formsApp = new App();

            LoadApplication(_formsApp);
        }

        public void OnLaunchedEvent(string arguments)
        {
            if (!string.IsNullOrEmpty(arguments))
            {
                var parts = arguments.Split("||", StringSplitOptions.RemoveEmptyEntries);
                _formsApp.SendOnAppLinkRequestReceived(new Uri(parts[1]));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            OnLaunchedEvent(e.Parameter.ToString());
        }
    }
}
