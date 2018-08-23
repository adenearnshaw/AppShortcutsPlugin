using System.Threading.Tasks;

namespace Plugin.AppShortcuts.Icons
{
    internal interface IIconProvider
    {
        Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon);
    }
}