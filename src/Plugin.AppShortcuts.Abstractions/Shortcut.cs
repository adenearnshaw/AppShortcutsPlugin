using System;
using System.ComponentModel;

namespace Plugin.AppShortcuts.Abstractions
{
    public class Shortcut
    {
        public Shortcut()
        {
            ID = Guid.NewGuid().ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Shortcut(string shortcutId)
        {
            ID = shortcutId;
        }

        public string ID { get; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Uri { get; set; }
    }
}
