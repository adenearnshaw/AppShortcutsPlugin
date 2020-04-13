using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Plugin.AppShortcuts.iOS;
using Plugin.AppShortcuts.Icons;
using UIKit;
using AuthenticationServices;

namespace Plugin.AppShortcuts
{
    [Preserve(AllMembers = true)]
    internal class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private readonly string NOT_SUPPORTED_ERROR_MESSAGE
            = $"Operation not supported on iOS 8 or below. Use {nameof(CrossAppShortcuts)}.{nameof(CrossAppShortcuts.IsSupported)} to check if the current device supports this feature.";

        private readonly IIconProvider _embeddedIconProvider;
        private readonly IIconProvider _customIconProvider;

        public AppShortcutsImplementation()
        {
            _embeddedIconProvider = new EmbeddedIconProvider();
            _customIconProvider = new CustomIconProvider();
            IsSupportedByCurrentPlatformVersion = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        }

        public void Init()
        { }

        public bool IsSupportedByCurrentPlatformVersion { get; }

        public async Task AddShortcut(Shortcut shortcut)
        {
            if (!IsSupportedByCurrentPlatformVersion)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            var type = shortcut.ShortcutId;
            var icon = shortcut.IsEmbeddedIcon
                ? (UIApplicationShortcutIcon)(await _embeddedIconProvider.CreatePlatformIcon(shortcut.Icon))
                : (UIApplicationShortcutIcon)(await _customIconProvider.CreatePlatformIcon(shortcut.Icon));
            var metadata = CreateMetadata(shortcut);

            NSRunLoop.Main.BeginInvokeOnMainThread(() =>
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
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            if (!IsSupportedByCurrentPlatformVersion)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            var dynamicShortcuts = UIApplication.SharedApplication.ShortcutItems.ToList();
            var shortcuts = dynamicShortcuts.Select(ds => new Shortcut(ds.Type)
            {
                Label = ds.LocalizedTitle,
                Description = ds.LocalizedSubtitle,
                Tag = ds?.UserInfo[ArgumentsHelper.ShortcutTagKey]?.ToString() ?? string.Empty,
                Uri = ds?.UserInfo[ArgumentsHelper.ShortcutUriKey]?.ToString() ?? string.Empty,
                Icon = ResolveShortcutIconType(ds?.Icon?.ToString() ?? string.Empty)
            }).ToList();

            await Task.Delay(200);

            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
            if (!IsSupportedByCurrentPlatformVersion)
                throw new NotSupportedOnDeviceException(NOT_SUPPORTED_ERROR_MESSAGE);

            await Task.Run(() =>
            {
                NSRunLoop.Main.BeginInvokeOnMainThread(() =>
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

        private NSDictionary<NSString, NSObject> CreateMetadata(Shortcut shortcut)
        {
            var metadata = new NSDictionary<NSString, NSObject>(
                new[]
                {
                    new NSString(ArgumentsHelper.ShortcutTagKey),
                    new NSString(ArgumentsHelper.ShortcutUriKey)
                },
                new[]
                {
                    new NSString(shortcut.Tag ?? string.Empty),
                    new NSString(shortcut.Uri)
                });

            return metadata;
        }

        private static readonly Func<string, IShortcutIcon> ResolveShortcutIconType = iconName =>
        {
            var isParseSuccessful = Enum.TryParse(iconName, out ShortcutIconType type);

            if (isParseSuccessful)
                return ShortcutIconTypesHelper.ResolveEmbeddedIcon(type);

            return new CustomIcon(iconName);
        };
    }
}
