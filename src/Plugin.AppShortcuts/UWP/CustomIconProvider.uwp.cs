using System;
using System.Threading.Tasks;
using Plugin.AppShortcuts.Icons;

namespace Plugin.AppShortcuts.UWP
{
    internal class CustomIconProvider : IIconProvider
    {
        public async Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon)
        {
            return await Task.Run(() =>
            {
                var uri = $"ms-appx:///{shortcutIcon.IconName}";
                return new Uri(uri);
            });
        }
    }
}
