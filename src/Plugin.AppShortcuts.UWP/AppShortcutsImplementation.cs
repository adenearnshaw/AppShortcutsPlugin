using Plugin.AppShortcuts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.StartScreen;

namespace Plugin.AppShortcuts.UWP
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string DarkIconUriFormat = "ms-appx:///Assets/icon_{0}_white.png";
        private const string LightIconUriFormat = "ms-appx:///Assets/icon_{0}_black.png";

        private bool _isSupported;
        private WindowsTheme _windowsTheme;

        public AppShortcutsImplementation()
        {
            _isSupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2) && JumpList.IsSupported();

            var uisettings = new Windows.UI.ViewManagement.UISettings();
            var color = uisettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);
            _windowsTheme = color == Colors.Black ? WindowsTheme.Dark : WindowsTheme.Light;
        }

        public void Init()
        {
        }

        public bool IsSupportedByCurrentPlatformVersion => _isSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            var jumplistItem = JumpListItem.CreateWithArguments($"{shortcut.ID}||{shortcut.Uri}", shortcut.Label);
            jumplistItem.Description = shortcut.Description;
            jumplistItem.Logo = GetIconUri(shortcut.Icon);

            var jumplist = await JumpList.LoadCurrentAsync();
            jumplist.Items.Add(jumplistItem);

            await jumplist.SaveAsync();
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            var jumplist = await JumpList.LoadCurrentAsync();

            var shortcuts = jumplist.Items.Select(i =>
            {
                var args = i.Arguments.Split(new[] { "||" }, StringSplitOptions.None);
                var sc = new Shortcut(args[0])
                {
                    Label = i.DisplayName,
                    Description = i.Description,
                    Icon = ShortcutIconType.Default,
                    Uri = args[1]
                };
                return sc;
            }).ToList();

            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
            var jumplist = await JumpList.LoadCurrentAsync();
            var toDelete = jumplist.Items.FirstOrDefault(i => i.Arguments.Contains(shortcutId));

            if (toDelete == null)
                return;

            jumplist.Items.Remove(toDelete);
            await jumplist.SaveAsync();
        }

        private Uri GetIconUri(ShortcutIconType iconType)
        {
            var iconName = iconType.ToString().ToLower();
            string uri;

            if (_windowsTheme == WindowsTheme.Dark)
                uri = string.Format(DarkIconUriFormat, iconName);
            else
                uri = string.Format(LightIconUriFormat, iconName);

            return new Uri(uri);
        }


        enum WindowsTheme
        {
            Dark,
            Light
        }
    }
}
