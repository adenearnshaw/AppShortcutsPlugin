using System;
using Foundation;
using UIKit;

namespace Plugin.AppShortcuts.iOS
{
    [Preserve(AllMembers = true)]
    public static class ArgumentsHelper
    {
        internal static string ShortcutUriKey = "ShortcutUri";

        public static Uri GetUriFromApplicationShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            if (shortcutItem.UserInfo.ContainsKey(new NSString(ShortcutUriKey)))
            {
                var shortcutUri = shortcutItem.UserInfo[ShortcutUriKey].ToString();
                return new Uri(shortcutUri);
            }

            return null;
        }
    }
}
