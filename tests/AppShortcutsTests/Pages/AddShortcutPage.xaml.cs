using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppShortcutsTests.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddShortcutPage : ContentPage
    {
        private Shortcut _shortcut;

        public AddShortcutPage()
        {
            InitializeComponent();
            _shortcut = new Shortcut();
        }

        public AddShortcutPage(string shortcutId)
        {
            InitializeComponent();
            LoadShortcut(shortcutId);
        }

        private async void LoadShortcut(string shortcutId)
        {
            var shortcuts = await CrossAppShortcuts.Current.GetShortcuts();
            _shortcut = shortcuts.FirstOrDefault(s => string.Equals(s.ID, shortcutId));

            if (_shortcut == null)
            {
                _shortcut = new Shortcut();
                return;
            }

            SubmitButton.Text = "Update shortcut";

            TitleEntry.Text = _shortcut.Label;
            SubtitleEntry.Text = _shortcut.Description;
            IconTypePicker.SelectedItem = _shortcut.Icon.ToString();
            CustomIconEntry.Text = _shortcut.CustomIconName;
        }

        private async void CreateNewShortcut(object sender, EventArgs args)
        {
            Enum.TryParse(IconTypePicker.SelectedItem.ToString(), out ShortcutIconType iconType);

            _shortcut.Label = TitleEntry.Text;
            _shortcut.Description = SubtitleEntry.Text;
            _shortcut.Icon = iconType;
            _shortcut.CustomIconName = CustomIconEntry.Text;
            _shortcut.Uri = $"stc://{nameof(AppShortcutsTests)}/{nameof(AddShortcutPage)}/{_shortcut.ID}";

            await CrossAppShortcuts.Current.AddShortcut(_shortcut);

            Navigation.PopAsync();
        }
    }
}