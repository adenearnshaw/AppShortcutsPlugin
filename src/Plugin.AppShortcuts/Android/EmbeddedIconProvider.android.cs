using System.Threading.Tasks;
using Android.App;
using Android.Graphics.Drawables;
using Plugin.AppShortcuts.Icons;

namespace Plugin.AppShortcuts.Android
{
    internal class EmbeddedIconProvider : IIconProvider
    {
        public async Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon)
        {
            var iconTypeString = shortcutIcon.IconName.ToLower();
            var iconName = $"ic_plugin_sc_{iconTypeString}";
            var resourceId = (int)(typeof(Plugin.AppShortcuts.Resource.Drawable).GetField(iconName)?.GetValue(null) ?? 0);
            var icon = Icon.CreateWithResource(Application.Context, resourceId);
            return icon;
        }
    }
}
