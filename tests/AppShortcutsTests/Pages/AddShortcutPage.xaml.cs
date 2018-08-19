using System;
using System.Linq;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;
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
            _shortcut = shortcuts.FirstOrDefault(s => string.Equals(s.ShortcutId, shortcutId));

            if (_shortcut == null)
            {
                _shortcut = new Shortcut();
                return;
            }

            SubmitButton.Text = "Update shortcut";

            TitleEntry.Text = _shortcut.Label;
            SubtitleEntry.Text = _shortcut.Description;
            IconTypePicker.SelectedItem = _shortcut.Icon.ToString();
            CustomIconEntry.Text = _shortcut.Icon.IconName;
        }

        private async void CreateNewShortcut(object sender, EventArgs args)
        {
            var icon = ResolveEmbeddedIcon(IconTypePicker.SelectedItem.ToString());

            if (!string.IsNullOrWhiteSpace(CustomIconEntry.Text))
            {
                icon = new CustomIcon(CustomIconEntry.Text);
            }

            _shortcut.Label = TitleEntry.Text;
            _shortcut.Description = SubtitleEntry.Text;
            _shortcut.Icon = icon;
            _shortcut.Uri = $"stc://{nameof(AppShortcutsTests)}/{nameof(AddShortcutPage)}/{_shortcut.ShortcutId}";

            await CrossAppShortcuts.Current.AddShortcut(_shortcut);

            Navigation.PopAsync();
        }

        private static Func<string, IShortcutIcon> ResolveEmbeddedIcon = iconType =>
        {
            switch (iconType)
            {
                case "Add":
                    return new AddIcon();
                case "Alarm":
                    return new AlarmIcon();
                case "Audio":
                    return new AudioIcon();
                case "Bookmark":
                    return new BookmarkIcon();
                case "CapturePhoto":
                    return new CapturePhotoIcon();
                case "CaptureVideo":
                    return new CaptureVideoIcon();
                case "Cloud":
                    return new CloudIcon();
                case "Compose":
                    return new ComposeIcon();
                case "Confirmation":
                    return new ConfirmationIcon();
                case "Contact":
                    return new ContactIcon();
                case "Date":
                    return new DateIcon();
                case "Favorite":
                    return new FavoriteIcon();
                case "Home":
                    return new HomeIcon();
                case "Invitation":
                    return new InvitationIcon();
                case "Location":
                    return new LocationIcon();
                case "Love":
                    return new LoveIcon();
                case "Mail":
                    return new MailIcon();
                case "MarkLocation":
                    return new MarkLocationIcon();
                case "Message":
                    return new MessageIcon();
                case "Pause":
                    return new PauseIcon();
                case "Play":
                    return new PlayIcon();
                case "Prohibit":
                    return new ProhibitIcon();
                case "Search":
                    return new SearchIcon();
                case "Share":
                    return new ShareIcon();
                case "Shuffle":
                    return new ShuffleIcon();
                case "Task":
                    return new TaskIcon();
                case "TaskCompleted":
                    return new TaskCompletedIcon();
                case "Time":
                    return new TimeIcon();
                case "Update":
                    return new UpdateIcon();
                case "Default":
                default:
                    return new DefaultIcon();
            }
        };
    }
}