using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Plugin.AppShortcuts.Abstractions;
using UIKit;

namespace Plugin.AppShortcuts
{
    public class AppShortcutsImplementation : IAppShortcuts, IPlatformSupport
    {
        private const string SHORTCUT_URI_KEY = "ShortcutUri";

        private readonly NSString ApplicationShortcutUserInfoIconKey = (NSString)"applicationShortcutUserInfoIconKey";
        private readonly bool _isShortcutsSupported;

        public AppShortcutsImplementation()
        {
            _isShortcutsSupported = UIDevice.CurrentDevice.CheckSystemVersion(9, 0);
        }

        public bool IsSupportedByCurrentPlatformVersion => _isShortcutsSupported;

        public Task AddShortcut(Shortcut shortcut)
        {
            return Task.Run(() =>
            {
                string type = ShortcutIdentifierType.Third.GetTypeName();
                var icon = CreateIcon(UIApplicationShortcutIconType.Date);
                var userInfo = CreateUserInfo(UIApplicationShortcutIconType.Play);

                var scut = new UIMutableApplicationShortcutItem(type,
                                                                shortcut.Label,
                                                                shortcut.Description,
                                                                icon,
                                                                userInfo);
                scut.UserInfo = new NSDictionary<NSString, NSObject>(new NSString(SHORTCUT_URI_KEY), new NSString(shortcut.Uri));
                UIApplication.SharedApplication.ShortcutItems = new[] { scut };
            });
        }

        public Task<List<Shortcut>> GetShortcuts()
        {
            return Task.Run(() =>
            {
                //TODO
                return new List<Shortcut>();
            });
        }

        public Task RemoveShortcut(Guid shortcutId)
        {
            return Task.Run(() =>
            {
                //TODO
            });
        }

        private NSDictionary<NSString, NSObject> CreateUserInfo(UIApplicationShortcutIconType type)
        {
            int rawValue = Convert.ToInt32(type);
            return new NSDictionary<NSString, NSObject>(ApplicationShortcutUserInfoIconKey, new NSNumber(rawValue));
        }

        private UIApplicationShortcutIcon CreateIcon(UIApplicationShortcutIconType type)
        {
            return UIApplicationShortcutIcon.FromType(type);
        }
    }

    enum ShortcutIdentifierType
    {
        First,
        Second,
        Third,
        Fourth,
    }

    static class ShortcutIdentifierTypeExtensions
    {
        public static string GetTypeName(this ShortcutIdentifierType self)
        {
            return string.Format("{0} {1}", NSBundle.MainBundle.BundleIdentifier, self);
        }
    }
}
