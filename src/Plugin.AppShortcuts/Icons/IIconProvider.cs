using System.Threading.Tasks;

namespace Plugin.AppShortcuts.Icons
{
    public interface IIconProvider
    {
        Task<object> CreatePlatformIcon(IShortcutIcon shortcutIcon);
    }
}