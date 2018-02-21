using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Abstractions;
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
                    await AddShortcut(ShortcutIconType.Add);
                    await AddShortcut(ShortcutIconType.Alarm);
                    await AddShortcut(ShortcutIconType.Audio);
                    await AddShortcut(ShortcutIconType.Bookmark);
                    break;
                case 2:
                    await AddShortcut(ShortcutIconType.CapturePhoto);
                    await AddShortcut(ShortcutIconType.CaptureVideo);
                    await AddShortcut(ShortcutIconType.Cloud);
                    await AddShortcut(ShortcutIconType.Compose);

                    break;
                case 3:
                    await AddShortcut(ShortcutIconType.Confirmation);
                    await AddShortcut(ShortcutIconType.Contact);
                    await AddShortcut(ShortcutIconType.Date);
                    await AddShortcut(ShortcutIconType.Favorite);

                    break;
                case 4:
                    await AddShortcut(ShortcutIconType.Home);
                    await AddShortcut(ShortcutIconType.Invitation);
                    await AddShortcut(ShortcutIconType.Location);
                    await AddShortcut(ShortcutIconType.Love);
                    break;
                case 5:
                    await AddShortcut(ShortcutIconType.Mail);
                    await AddShortcut(ShortcutIconType.MarkLocation);
                    await AddShortcut(ShortcutIconType.Message);
                    await AddShortcut(ShortcutIconType.Pause);
                    break;
                case 6:
                    await AddShortcut(ShortcutIconType.Play);
                    await AddShortcut(ShortcutIconType.Prohibit);
                    await AddShortcut(ShortcutIconType.Search);
                    await AddShortcut(ShortcutIconType.Share);
                    break;
                case 7:
                    await AddShortcut(ShortcutIconType.Shuffle);
                    await AddShortcut(ShortcutIconType.Task);
                    await AddShortcut(ShortcutIconType.TaskCompleted);
                    await AddShortcut(ShortcutIconType.Time);
                    break;
                case 8:
                    await AddShortcut(ShortcutIconType.Update);
                    await AddShortcut(ShortcutIconType.Default);
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
                await CrossAppShortcuts.Current.RemoveShortcut(sc.ID);
            }
        }

        private async Task AddShortcut(ShortcutIconType iconType)
        {
            var sc = new Shortcut
            {
                Icon = iconType,
                Label = $"{iconType}_L",
                Description = $"{iconType}_D",
                Uri = $"stc://AppShortcutTests/Tests/{iconType}"
            };
            await CrossAppShortcuts.Current.AddShortcut(sc);
        }

        private async Task AddCustomShortcut(string iconName)
        {
            var sc = new Shortcut
            {
                Icon = ShortcutIconType.Custom,
                Label = $"{iconName}_L",
                Description = $"{iconName}_D",
                Uri = $"stc://AppShortcutTests/Tests/{iconName}",
                CustomIconName = iconName
            };
            await CrossAppShortcuts.Current.AddShortcut(sc);
        }
    }
}
