using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.AppShortcuts;
using Xamarin.Forms;

namespace AppShortcutsTests.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Shortcuts = new ObservableCollection<Shortcut>();

            ShortcutsListView.ItemsSource = Shortcuts;
            ShortcutsListView.RefreshCommand = new Command(async () => await RefreshShortcutsList());
        }

        private ObservableCollection<Shortcut> _shortcuts;
        public ObservableCollection<Shortcut> Shortcuts
        {
            get => _shortcuts;
            set => _shortcuts = value;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsSupportedLabel.Text = $"App shortcuts supported: {(CrossAppShortcuts.IsSupported ? "Yes" : "No")}";

            RefreshShortcutsList();
        }

        public async void AddNewShortcut(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new AddShortcutPage());
        }

        public async void TestShortcuts(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new TestShortcutsPage());
        }

        public async void DeleteShortcuts(object sender, EventArgs args)
        {
            foreach (var sc in Shortcuts)
            {
                await CrossAppShortcuts.Current.RemoveShortcut(sc.ShortcutId);
            }
            await RefreshShortcutsList();
        }

        public async void DeleteShortcut(object sender, EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var sc = (Shortcut)menuItem.CommandParameter;
            await CrossAppShortcuts.Current.RemoveShortcut(sc.ShortcutId);
            await RefreshShortcutsList();
        }

        private async Task RefreshShortcutsList()
        {
            var shortcuts = await CrossAppShortcuts.Current.GetShortcuts();

            Device.BeginInvokeOnMainThread(() =>
            {
                Shortcuts.Clear();
                foreach (var sc in shortcuts)
                {
                    Shortcuts.Add(sc);
                }
            });
            ShortcutsListView.IsRefreshing = false;
        }

        private void ProcessDeeplink(object sender, string uri)
        {
            DisplayAlert("Navigation from Deep Link", uri, "OK");
        }
    }
}
