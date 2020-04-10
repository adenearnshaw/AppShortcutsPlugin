using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Plugin.AppShortcuts.Android;
using Plugin.AppShortcuts.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AUri = Android.Net.Uri;

namespace Plugin.AppShortcuts
{
    [Preserve(AllMembers = true)]
    internal class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string TagKey = "tag";

        private readonly string NOT_SUPPORTED_ERROR_MESSAGE = $"Operation not supported on Android API 24 or below. Use {nameof(CrossAppShortcuts)}.{nameof(CrossAppShortcuts.IsSupported)} to check if the current device supports this feature.";
        
        private readonly IIconProvider _embeddedIconProvider;
        private readonly IIconProvider _customIconProvider;
        private readonly ShortcutManager _manager;
        private readonly bool _isShortcutsSupported;

        private Type _drawableClass;

        public AppShortcutsImplementation()
        {
            _embeddedIconProvider = new EmbeddedIconProvider();
            _customIconProvider = new CustomIconProvider();
            _manager = (ShortcutManager)Application.Context.GetSystemService(Context.ShortcutService);

            _isShortcutsSupported = Build.VERSION.SdkInt >= BuildVersionCodes.NMr1;

            Init();
        }

        public void Init()
        {
            _drawableClass = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(x => x.Name == "Drawable" || x.Name == "Resource_Drawable");
            (_customIconProvider as CustomIconProvider)?.Init(_drawableClass);
        }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            
            var context = Application.Context;
            var builder = new ShortcutInfo.Builder(context, shortcut.ShortcutId);

            var uri = AUri.Parse(shortcut.Uri);

            builder.SetIntent(new Intent(Intent.ActionView, uri));
            builder.SetShortLabel(shortcut.Label);
            builder.SetLongLabel(shortcut.Description);

            var extrasBundle = new PersistableBundle();
            extrasBundle.PutString(TagKey, shortcut.Tag);
            builder.SetExtras(extrasBundle);

            var icon = await CreateIcon(shortcut.Icon);

            if (icon != null)
                builder.SetIcon(icon);

            var scut = builder.Build();

            if (_manager.DynamicShortcuts == null || !_manager.DynamicShortcuts.Any())
                _manager.SetDynamicShortcuts(new List<ShortcutInfo> { scut });
            else
                _manager.AddDynamicShortcuts(new List<ShortcutInfo> { scut });
            
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
                    Uri = s.Intent.ToUri(IntentUriType.AllowUnsafe),
                    Tag = s.Extras.GetString(TagKey)
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

        private async Task<Icon> CreateIcon(IShortcutIcon icon)
        {
            try
            {
                if (icon is CustomIcon)
                    return await _customIconProvider.CreatePlatformIcon(icon) as Icon;

                return await _embeddedIconProvider.CreatePlatformIcon(icon) as Icon;
            }
            catch (Exception ex)
            {
                Log.Error(nameof(AppShortcutsImplementation), ex.Message, ex);
                return null;
            }
        }
    }
}
