namespace Plugin.AppShortcuts.Icons
{
    /// <summary>
    /// Icon with a custom image as an icon
    /// </summary>
    public class CustomIcon : IShortcutIcon
    {
        /// <summary>
        /// Creates instance of CustomIcon
        /// </summary>
        /// <param name="iconFileName">Custom image name</param>
        public CustomIcon(string iconFileName)
        {
            IconName = iconFileName;
        }

        /// <summary>
        /// Gets the custom image name for the Icon
        /// </summary>
        public string IconName { get; }
    }
}
