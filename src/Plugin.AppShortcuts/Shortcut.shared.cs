using System;

namespace Plugin.AppShortcuts
{
    public class Shortcut
    {
        public Shortcut()
        {
            ID = Guid.NewGuid().ToString();
        }

        internal Shortcut(string shortcutId)
        {
            ID = shortcutId;
        }

        public string ID { get; }
        public string Label { get; set; }
        public string Description { get; set; }
        public ShortcutIconType Icon { get; set; }
        public string CustomIconName { get; set; }
        public string Uri { get; set; }
    }
}
