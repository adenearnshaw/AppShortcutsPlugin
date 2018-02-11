using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Plugin.AppShortcuts.Abstractions;
using AUri = Android.Net.Uri;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private readonly ShortcutManager _manager;
        private readonly bool _isShortcutsSupported;

        public AppShortcutsImplementation()
        {
            _manager = (ShortcutManager)Application.Context.GetSystemService(Context.ShortcutService);

            _isShortcutsSupported = Build.VERSION.SdkInt >= BuildVersionCodes.N;
        }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!_isShortcutsSupported)
                return;

            var context = Application.Context;
            var builder = new ShortcutInfo.Builder(context, shortcut.ID);

            var uri = AUri.Parse(shortcut.Uri);

            builder.SetIntent(new Intent(Intent.ActionView, uri));
            builder.SetShortLabel(shortcut.Label);
            builder.SetLongLabel(shortcut.Description);

            var icon = CreateIcon(shortcut.Icon, shortcut.CustomIconName);
            if (icon != null)
                builder.SetIcon(icon);

            var scut = builder.Build();

            if (_manager.DynamicShortcuts == null || !_manager.DynamicShortcuts.Any())
                _manager.SetDynamicShortcuts(new List<ShortcutInfo> { scut });
            else
                _manager.AddDynamicShortcuts(new List<ShortcutInfo> { scut });
        }

        public Task<List<Shortcut>> GetShortcuts()
        {
            return Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    return new List<Shortcut>();

                var dynamicShortcuts = _manager.DynamicShortcuts;
                var shortcuts = dynamicShortcuts.Select(s => new Shortcut(s.Id)
                {
                    Label = s.ShortLabel,
                    Description = s.LongLabel,
                    Uri = s.Intent.ToUri(IntentUriType.AllowUnsafe)
                }).ToList();
                return shortcuts;
            });
        }

        public Task RemoveShortcut(string shortcutId)
        {
            return Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    return;

                _manager.RemoveDynamicShortcuts(new List<string> { shortcutId });
            });
        }

        private Icon CreateIcon(ShortcutIconType iconType, string iconName)
        {
            try
            {
                switch (iconType)
                {
                    case ShortcutIconType.Default:
                        return null;
                    case ShortcutIconType.Custom:
                        return Icon.CreateWithBitmap(BuildBitmapFromName(iconName));
                    default:
                        {
                            var iconTypeString = iconType.ToString().ToLower();
                            iconName = $"ic_sc_{iconTypeString}";
                            var resourceId = (int)(typeof(Plugin.AppShortcuts.Resource.Drawable).GetField(iconName)?.GetValue(null) ?? 0);
                            var icon = Icon.CreateWithResource(Application.Context, resourceId);
                            return icon;
                        }
                }
            }
            catch
            {
                return null;
            }
        }

        private Bitmap BuildBitmapFromName(string iconName)
        {
            iconName = iconName.Replace(".png", "")
                               .Replace(".svg", "")
                               .Replace(".jpg", "")
                               .Replace(".jpeg", "")
                               .Replace(".gif", "");
            var resourceId = (int)(typeof(Resource.Drawable).GetField(iconName)?.GetValue(null) ?? 0);
            var vd = Application.Context.GetDrawable(resourceId);
            var bitmap = Bitmap.CreateBitmap(vd.IntrinsicWidth, vd.IntrinsicHeight, Bitmap.Config.Argb8888);
            var canvas = new Canvas(bitmap);
            vd.SetBounds(0, 0, canvas.Width, canvas.Height);
            vd.Draw(canvas);
            return bitmap;
        }
    }
}
