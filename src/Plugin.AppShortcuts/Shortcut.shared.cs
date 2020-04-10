using System;
using Plugin.AppShortcuts.Icons;

namespace Plugin.AppShortcuts
{
    public class Shortcut
    {
        public Shortcut()
        {
            ShortcutId = Guid.NewGuid().ToString();
        }

        internal Shortcut(string shortcutId)
        {
            ShortcutId = shortcutId;
        }

        public string ShortcutId { get; }
        public string Label { get; set; }
        public string Description { get; set; }
        public IShortcutIcon Icon { get; set; }
        public string Uri { get; set; }
        public string Tag { get; set; }

        internal bool IsEmbeddedIcon => Icon is EmbeddedIcon;
    }
}
