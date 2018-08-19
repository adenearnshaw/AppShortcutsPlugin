using Android.App;
using Android.Graphics.Drawables;

namespace Plugin.AppShortcuts.Android
{
    public class IconProvider
    {
        private Icon CreateIconFromDefaultSet(ShortcutIconType iconType)
        {
            var iconTypeString = iconType.ToString().ToLower();

            var iconName = $"ic_sc_{iconTypeString}";

            var resourceId = (int)(typeof(Plugin.AppShortcuts.Resource.Drawable).GetField(iconName)?.GetValue(null) ?? 0);

            var icon = Icon.CreateWithResource(Application.Context, resourceId);

            return icon;

        }
    }
}