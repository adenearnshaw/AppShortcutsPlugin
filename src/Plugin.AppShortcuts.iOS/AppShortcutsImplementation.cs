using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Plugin.AppShortcuts.Abstractions;
using UIKit;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string SHORTCUT_URI_KEY = "ShortcutUri";
        private readonly string NOT_SUPPORTED_ERROR_MESSAGE = $"Operation not supported on iOS 8 or below. Use {nameof(CrossAppShortcuts)}.{nameof(CrossAppShortcuts.IsSupported)} to check if the current device supports this feature.";

        private readonly bool _isShortcutsSupported;

        public AppShortcutsImplementation()
        {
            _isShortcutsSupported = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        }

        public void Init()
        { }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            await Task.Run(() =>
            {
                var type = shortcut.ID;
                var icon = shortcut.Icon == ShortcutIconType.Custom
                            ? CreateCustomIcon(shortcut.CustomIconName)
                            : CreateIcon(shortcut.Icon);
                var metadata = CreateUriMetadata(shortcut.Uri);

                new NSObject().BeginInvokeOnMainThread(() =>
                {
                    var scut = new UIMutableApplicationShortcutItem(type,
                                                                    shortcut.Label,
                                                                    shortcut.Description,
                                                                    icon,
                                                                    metadata);

                    var scuts = UIApplication.SharedApplication.ShortcutItems.ToList();
                    scuts.Add(scut);

                    UIApplication.SharedApplication.ShortcutItems = scuts.ToArray();
                });
            });
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            var dynamicShortcuts = UIApplication.SharedApplication.ShortcutItems.ToList();
            var shortcuts = dynamicShortcuts.Select(ds => new Shortcut(ds.Type)
            {
                Label = ds.LocalizedTitle,
                Description = ds.LocalizedSubtitle,
                Uri = ds?.UserInfo[SHORTCUT_URI_KEY]?.ToString() ?? string.Empty,
                Icon = ResolveShortcutIconType(ds?.Icon?.ToString() ?? string.Empty)
            }).ToList();
            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
            if (!_isShortcutsSupported)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            await Task.Run(() =>
            {
                new NSObject().BeginInvokeOnMainThread(() =>
                {
                    var shortcutItem =
                        UIApplication.SharedApplication.ShortcutItems.FirstOrDefault(si => si.Type.Equals(shortcutId));

                    if (shortcutItem == null)
                        return;

                    var updatedItems = UIApplication.SharedApplication.ShortcutItems.ToList();
                    updatedItems.Remove(shortcutItem);
                    UIApplication.SharedApplication.ShortcutItems = updatedItems.ToArray();
                });
            });
        }

        private NSDictionary<NSString, NSObject> CreateUriMetadata(string uri)
        {
            var metadata = new NSDictionary<NSString, NSObject>(new NSString(SHORTCUT_URI_KEY), new NSString(uri));
            return metadata;
        }

        private UIApplicationShortcutIcon CreateIcon(ShortcutIconType iconType)
        {
            var isParseSuccessful = Enum.TryParse(iconType.ToString(), out UIApplicationShortcutIconType type);

            if (!isParseSuccessful)
                type = UIApplicationShortcutIconType.Favorite;

            UIApplicationShortcutIcon icon = null;
            new NSObject().BeginInvokeOnMainThread(() =>
            {
                icon = UIApplicationShortcutIcon.FromType(type);
            });
            return icon;
        }

        private UIApplicationShortcutIcon CreateCustomIcon(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName))
                return null;

            UIApplicationShortcutIcon icon = null;
            new NSObject().BeginInvokeOnMainThread(() =>
            {
                icon = UIApplicationShortcutIcon.FromTemplateImageName(assetName);
            });
            return icon;
        }

        private Func<string, ShortcutIconType> ResolveShortcutIconType = iconName =>
        {
            ShortcutIconType type;
            var isParseSuccessful = Enum.TryParse(iconName, out type);

            if (!isParseSuccessful)
                type = ShortcutIconType.Default;

            return type;
        };
    }
}
