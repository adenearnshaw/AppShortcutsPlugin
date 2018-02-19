using AppShortcutsTests.Pages;
using Xamarin.Forms;

namespace AppShortcutsTests
{
    public partial class App : Application
    {
        public static string DeepLinkUriBase => $"stc://{nameof(AppShortcutsTests)}/{nameof(AddShortcutPage)}/";

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnAppLinkRequestReceived(System.Uri uri)
        {
            var shortcutId = uri.ToString().Replace(DeepLinkUriBase, "");
            MainPage.Navigation.PushAsync(new AddShortcutPage(shortcutId));
            //base.OnAppLinkRequestReceived(uri);
        }
    }
}
