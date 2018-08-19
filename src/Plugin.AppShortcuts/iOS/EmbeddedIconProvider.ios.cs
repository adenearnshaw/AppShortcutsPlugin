using System;
using System.Threading.Tasks;
using Foundation;
using Plugin.AppShortcuts.Icons;
using UIKit;

namespace Plugin.AppShortcuts.iOS
{
    internal class EmbeddedIconProvider : IIconProvider
    {
        public async Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon)
        {
            var isParseSuccessful = Enum.TryParse(shortcutIcon.IconName, out UIApplicationShortcutIconType type);

            if (!isParseSuccessful)
                type = UIApplicationShortcutIconType.Favorite;

            UIApplicationShortcutIcon icon = null;
            new NSObject().BeginInvokeOnMainThread(() =>
            {
                icon = UIApplicationShortcutIcon.FromType(type);
            });

            await Task.Delay(200);

            return icon;
        }
    }
}