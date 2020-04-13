namespace Plugin.AppShortcuts.Icons
{
    /// <summary>
    /// Icon with image from built-in set
    /// </summary>
    public abstract class EmbeddedIcon : IShortcutIcon
    {
        internal abstract ShortcutIconType IconType { get; }

        /// <summary>
        /// Icon name for associated embedded icon
        /// </summary>
        public string IconName => IconType.ToString();
    }
}
