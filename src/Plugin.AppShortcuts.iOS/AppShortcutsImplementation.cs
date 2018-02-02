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

        public Task AddShortcut(Shortcut shortcut)
        {
            return Task.Run(() =>
            {
                var type = shortcut.ID.ToString();
                var icon = CreateIcon(shortcut.Icon);
                var userInfo = CreateUserInfo(shortcut.Uri);

                var scut = new UIMutableApplicationShortcutItem(type,
                                                                shortcut.Label,
                                                                shortcut.Description,
                                                                icon,
                                                                userInfo);

                if (UIApplication.SharedApplication.ShortcutItems == null)
                    UIApplication.SharedApplication.ShortcutItems = new UIApplicationShortcutItem[0];

                UIApplication.SharedApplication.ShortcutItems.Append(scut);
            });
        }

        public Task<List<Shortcut>> GetShortcuts()
        {
            return Task.Run(() =>
            {
                var dynamicShortcuts = UIApplication.SharedApplication.ShortcutItems ?? new UIApplicationShortcutItem[0];
                var shortcuts = dynamicShortcuts.Select(ds => new Shortcut
                {
                    Label = ds.LocalizedTitle,
                    Description = ds.Description,
                    Uri = ds.UserInfo[SHORTCUT_URI_KEY].ToString(),
                    Icon = ds.Icon.ToString()
                });
                return shortcuts.ToList();
            });
        }

        public Task RemoveShortcut(Guid shortcutId)
        {
            return Task.Run(() =>
            {
                var shortcutIdString = shortcutId.ToString();
                var shortcutItem =
                    UIApplication.SharedApplication.ShortcutItems.FirstOrDefault(si => si.Type.Equals(shortcutIdString));

                if (shortcutItem != null)
                    UIApplication.SharedApplication.ShortcutItems.ToList().Remove(shortcutItem);
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
            var icon = UIApplicationShortcutIcon.FromTemplateImageName(assetName);
            return icon;
        }
    }
}
