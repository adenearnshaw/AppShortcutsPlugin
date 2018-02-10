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

        public AppShortcutsImplementation()
        {
            IsSupportedByCurrentPlatformVersion = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        }

        public bool IsSupportedByCurrentPlatformVersion { get; }

        public async Task AddShortcut(Shortcut shortcut)
        {
            await Task.Run(() =>
            {
                var type = shortcut.ID;
                var icon = CreateIcon(shortcut.Icon);
                var userInfo = CreateUserInfo(shortcut.Uri);

                new NSObject().BeginInvokeOnMainThread(() =>
                {
                    var scut = new UIMutableApplicationShortcutItem(type,
                                                                    shortcut.Label,
                                                                    shortcut.Description,
                                                                    icon,
                                                                    userInfo);

                    var scuts = UIApplication.SharedApplication.ShortcutItems.ToList();
                    scuts.Add(scut);

                    UIApplication.SharedApplication.ShortcutItems = scuts.ToArray();
                });
            });
        }

        public async Task<List<Shortcut>> GetShortcuts()
        {
            var dynamicShortcuts = UIApplication.SharedApplication.ShortcutItems.ToList();
            var shortcuts = dynamicShortcuts.Select(ds => new Shortcut(ds.Type)
            {
                Label = ds.LocalizedTitle,
                Description = ds.LocalizedSubtitle,
                Uri = ds?.UserInfo[SHORTCUT_URI_KEY]?.ToString() ?? string.Empty,
                Icon = ds?.Icon?.ToString() ?? string.Empty
            }).ToList();
            return shortcuts;
        }

        public async Task RemoveShortcut(string shortcutId)
        {
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

        private NSDictionary<NSString, NSObject> CreateUserInfo(string uri)
        {
            var userInfo = new NSDictionary<NSString, NSObject>(new NSString(SHORTCUT_URI_KEY), new NSString(uri));
            return userInfo;
        }

        private UIApplicationShortcutIcon CreateIcon(UIApplicationShortcutIconType type)
        {
            return UIApplicationShortcutIcon.FromType(type);
        }

        private UIApplicationShortcutIcon CreateIcon(string assetName)
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
    }
}
