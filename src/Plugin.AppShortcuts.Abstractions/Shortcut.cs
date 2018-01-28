using System;
using System.ComponentModel;

namespace Plugin.AppShortcuts.Abstractions
{
    public class Shortcut
    {
        public Shortcut()
        {
            ID = Guid.NewGuid();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Shortcut(Guid id)
        {
            ID = id;
        }

        public Guid ID { get; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
    }
}
