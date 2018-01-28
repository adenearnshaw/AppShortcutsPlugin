using Xamarin.Forms;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Abstractions;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppShortcutsTests
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Shortcuts = new ObservableCollection<Shortcut>();
            ShortcutsListView.ItemsSource = Shortcuts;
            ShortcutsListView.RefreshCommand = new Command(async () => await RefreshShortcutsList());
            MessagingCenter.Subscribe<App, string>(this, App.DeepLinkMessageName, ProcessDeeplink);
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

            RefreshShortcutsList();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<App, string>(this, App.DeepLinkMessageName);

            base.OnDisappearing();
        }

        public async void AddNewShortcut()
        {
            var shortcut = new Shortcut
            {
                Label = $"Shortcut {Shortcuts.Count + 1}",
                Description = $"Added {DateTime.Now}"
            };
            shortcut.Uri = $"stc://AppShortcutsTests/MainPage/{shortcut.ID}";

            await CrossAppShortcuts.Current.AddShortcut(shortcut);
            await RefreshShortcutsList();
        }

        public async void DeleteShortcut(object sender, EventArgs e)
        {
            var sc = (Shortcut)sender;
            await CrossAppShortcuts.Current.RemoveShortcut(sc.ID);
            await RefreshShortcutsList();
        }

        private async Task RefreshShortcutsList()
        {
            var shortcuts = await CrossAppShortcuts.Current.GetShortcuts();

            Shortcuts.Clear();
            foreach (var sc in shortcuts)
            {
                Shortcuts.Add(sc);
            }
        }

        private void ProcessDeeplink(object sender, string uri)
        {
            DisplayAlert("Navigation from Deep Link", uri, "OK");
        }
    }
}
