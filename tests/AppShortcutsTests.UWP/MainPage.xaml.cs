using Xamarin.Forms.Platform.UWP;

namespace AppShortcutsTests.UWP
{
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new AppShortcutsTests.App());
        }
    }
}
