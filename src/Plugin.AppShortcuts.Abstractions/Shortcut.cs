using System;
namespace Plugin.AppShortcuts.Abstractions
{
    public class Shortcut
    {
        public Guid ID { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
    }
}
