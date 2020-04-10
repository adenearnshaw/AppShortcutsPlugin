using System;
using Foundation;
using UIKit;

namespace Plugin.AppShortcuts.iOS
{
    [Preserve(AllMembers = true)]
    public static class ArgumentsHelper
    {
        internal static string ShortcutUriKey = "ShortcutUri";
        internal static string ShortcutTagKey = "Tag";

        public static Uri GetUriFromApplicationShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            if (shortcutItem.UserInfo.ContainsKey(new NSString(ShortcutUriKey)))
            {
                var shortcutUri = shortcutItem.UserInfo[ShortcutUriKey].ToString();
                return new Uri(shortcutUri);
            }

            return null;
        }

        public static string GetTagFromApplicationShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            if (shortcutItem.UserInfo.ContainsKey(new NSString(ShortcutTagKey)))
            {
                var shortcutTag = shortcutItem.UserInfo[ShortcutTagKey].ToString();
                return shortcutTag;
            }

            return null;
        }
    }
}
