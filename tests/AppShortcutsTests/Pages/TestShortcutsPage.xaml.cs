using System;
using System.Threading.Tasks;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;
using Xamarin.Forms;

namespace AppShortcutsTests.Pages
{
    public partial class TestShortcutsPage : ContentPage
    {
        public TestShortcutsPage()
        {
            InitializeComponent();
        }

        private async void AddShortcutSet(object sender, System.EventArgs e)
        {
            var tag = Convert.ToInt32((sender as Button)?.CommandParameter);

            await RemoveCurrentShortcuts();

            switch (tag)
            {
                case 1:
                    await AddShortcut(new AddIcon());
                    await AddShortcut(new AlarmIcon());
                    await AddShortcut(new AudioIcon());
                    await AddShortcut(new BookmarkIcon());
                    break;
                case 2:
                    await AddShortcut(new CapturePhotoIcon());
                    await AddShortcut(new CaptureVideoIcon());
                    await AddShortcut(new CloudIcon());
                    await AddShortcut(new ComposeIcon());

                    break;
                case 3:
                    await AddShortcut(new ConfirmationIcon());
                    await AddShortcut(new ContactIcon());
                    await AddShortcut(new DateIcon());
                    await AddShortcut(new FavoriteIcon());

                    break;
                case 4:
                    await AddShortcut(new HomeIcon());
                    await AddShortcut(new InvitationIcon());
                    await AddShortcut(new LocationIcon());
                    await AddShortcut(new LoveIcon());
                    break;
                case 5:
                    await AddShortcut(new MailIcon());
                    await AddShortcut(new MarkLocationIcon());
                    await AddShortcut(new MessageIcon());
                    await AddShortcut(new PauseIcon());
                    break;
                case 6:
                    await AddShortcut(new PlayIcon());
                    await AddShortcut(new ProhibitIcon());
                    await AddShortcut(new SearchIcon());
                    await AddShortcut(new ShareIcon());
                    break;
                case 7:
                    await AddShortcut(new ShuffleIcon());
                    await AddShortcut(new TaskIcon());
                    await AddShortcut(new TaskCompletedIcon());
                    await AddShortcut(new TimeIcon());
                    break;
                case 8:
                    await AddShortcut(new UpdateIcon());
                    await AddShortcut(new DefaultIcon());
                    await AddCustomShortcut("ic_beach.png");
                    break;
            }

            await Navigation.PopAsync();
        }

        private void ClearShortcuts(object sender, EventArgs e)
        {
            RemoveCurrentShortcuts();
        }

        private async Task RemoveCurrentShortcuts()
        {
            var currentShortcuts = await CrossAppShortcuts.Current.GetShortcuts();
            foreach (var sc in currentShortcuts)
            {
                await CrossAppShortcuts.Current.RemoveShortcut(sc.ShortcutId);
            }
        }

        private async Task AddShortcut(IShortcutIcon icon)
        {
            var sc = new Shortcut
            {
                Icon = icon,
                Label = $"{icon.IconName}_L",
                Description = $"{icon.IconName}_D",
                Uri = $"stc://AppShortcutTests/Tests/{icon.IconName}"
            };
            await CrossAppShortcuts.Current.AddShortcut(sc);
        }

        private async Task AddCustomShortcut(string iconName)
        {
            var sc = new Shortcut
            {
                Icon = new CustomIcon(iconName),
                Label = $"{iconName}_L",
                Description = $"{iconName}_D",
                Uri = $"stc://AppShortcutTests/Tests/{iconName}"
            };
            await CrossAppShortcuts.Current.AddShortcut(sc);
        }
    }
}
