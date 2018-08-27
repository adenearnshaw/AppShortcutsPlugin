namespace Plugin.AppShortcuts.Icons
{
    public abstract class EmbeddedIcon : IShortcutIcon
    {
        internal abstract ShortcutIconType IconType { get; }

        public string IconName => IconType.ToString();
    }
}
