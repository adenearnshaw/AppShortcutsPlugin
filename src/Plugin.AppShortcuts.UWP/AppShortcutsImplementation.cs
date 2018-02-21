using Plugin.AppShortcuts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.StartScreen;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string DarkIconUriFormat = "icon_{0}_white.png";
        private const string LightIconUriFormat = "icon_{0}_black.png";

        private readonly EmbeddedImageHelper _embeddedImageHelper;

        private bool _isSupported;
        private WindowsTheme _windowsTheme = WindowsTheme.Dark;


        public AppShortcutsImplementation()
        {
            _isSupported = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2) && JumpList.IsSupported();

            _embeddedImageHelper = new EmbeddedImageHelper();

            //var uisettings = new Windows.UI.ViewManagement.UISettings();
            //var color = uisettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Foreground);
            //_windowsTheme = color == Colors.Black ? WindowsTheme.Dark : WindowsTheme.Light;
        }

        public void Init()
        {
        }

        public bool IsSupportedByCurrentPlatformVersion => _isSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            var jumplistItem = JumpListItem.CreateWithArguments($"{shortcut.ID}||{shortcut.Uri}", shortcut.Label);
            jumplistItem.Description = shortcut.Description;
            jumplistItem.Logo = await GetIconUri(shortcut.Icon, shortcut.CustomIconName);

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

        private async Task<Uri> GetIconUri(ShortcutIconType iconType, string fileName = "")
        {
            if (iconType == ShortcutIconType.Custom)
                return GetCustomIconUri(fileName);

            return await GetEmbeddedIconUri(iconType);
        }

        private Uri GetCustomIconUri(string fileName)
        {
            var uri = $"ms-appx:///{fileName}";
            return new Uri(uri);
        }

        private async Task<Uri> GetEmbeddedIconUri(ShortcutIconType iconType)
        {
            var iconName = iconType.ToString().ToLower();
            string iconFileName;

            if (_windowsTheme == WindowsTheme.Dark)
                iconFileName = string.Format(DarkIconUriFormat, iconName);
            else
                iconFileName = string.Format(LightIconUriFormat, iconName);

            var uri = await _embeddedImageHelper.CopyEmbeddedImageToAppData(iconFileName);
            return uri;
        }


        enum WindowsTheme
        {
            Dark,
            Light
        }
    }
}
