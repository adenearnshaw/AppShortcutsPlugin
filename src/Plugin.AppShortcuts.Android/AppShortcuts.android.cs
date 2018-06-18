using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Plugin.AppShortcuts.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AUri = Android.Net.Uri;
using Path = System.IO.Path;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private readonly string NOT_SUPPORTED_ERROR_MESSAGE = $"Operation not supported on Android API 24 or below. Use {nameof(CrossAppShortcuts)}.{nameof(CrossAppShortcuts.IsSupported)} to check if the current device supports this feature.";

        private readonly ShortcutManager _manager;
        private readonly bool _isShortcutsSupported;

        private Type _drawableClass;

        public AppShortcutsImplementation()
        {
            _manager = (ShortcutManager)Application.Context.GetSystemService(Context.ShortcutService);

            _isShortcutsSupported = Build.VERSION.SdkInt >= BuildVersionCodes.NMr1;

            Init();
        }

        public void Init()
        {
            _drawableClass = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == "Drawable" || x.Name == "Resource_Drawable");
        }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            await Task.Run(() =>
            {
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
            });
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            return await Task.Run(() =>
            {
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

        public async Task RemoveShortcut(string shortcutId)
        {
            await Task.Run(() =>
            {
                if (!_isShortcutsSupported)
                    throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

                _manager.RemoveDynamicShortcuts(new List<string> { shortcutId });
            });
        }

        private Icon CreateIcon(ShortcutIconType iconType, string iconName)
        {
            try
            {
                if (iconType == ShortcutIconType.Custom)
                    return CreateIconFromCustomImage(iconName);

                return CreateIconFromDefaultSet(iconType);
            }
            catch
            {
                return null;
            }
        }

        private Icon CreateIconFromDefaultSet(ShortcutIconType iconType)
        {
            var iconTypeString = iconType.ToString().ToLower();
            var iconName = $"ic_sc_{iconTypeString}";
            var resourceId = (int)(typeof(Plugin.AppShortcuts.Resource.Drawable).GetField(iconName)?.GetValue(null) ?? 0);
            var icon = Icon.CreateWithResource(Application.Context, resourceId);
            return icon;
        }

        private Icon CreateIconFromCustomImage(string iconName)
        {
            if (File.Exists(iconName))
            {
                var bitmap = BitmapFactory.DecodeFile(iconName);

                if (bitmap != null)
                    return null;

                return Icon.CreateWithBitmap(bitmap);
            }
            else
            {
                var resourceId = IdFromTitle(iconName);
                var ic = Icon.CreateWithResource(Application.Context, resourceId);
                return ic;
            }
        }

        private int IdFromTitle(string title)
        {
            string name = Path.GetFileNameWithoutExtension(title);
            int id = GetId(name);
            return id;
        }

        private int GetId(string memberName)
        {
            var type = _drawableClass;
            object value = type.GetFields().FirstOrDefault(p => p.Name == memberName)?.GetValue(type)
                ?? type.GetProperties().FirstOrDefault(p => p.Name == memberName)?.GetValue(type);
            if (value is int)
                return (int)value;
            return 0;
        }
    }
}
