using Xamarin.Forms;

namespace AppShortcutsTests
{
    public partial class App : Application
    {
        public static string DeepLinkMessageName => "DeepLink";

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnAppLinkRequestReceived(System.Uri uri)
        {
            MessagingCenter.Send<App, string>(this, DeepLinkMessageName, uri.ToString());

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
