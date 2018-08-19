namespace Plugin.AppShortcuts.Icons
{
    public class CustomIcon : IShortcutIcon
    {
        public CustomIcon(string iconFileName)
        {
            IconName = iconFileName;
        }

        public string IconName { get; }
    }
}