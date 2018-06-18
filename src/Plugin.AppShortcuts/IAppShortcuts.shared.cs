using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.AppShortcuts
{
    public interface IAppShortcuts
    {
        void Init();

        Task<List<Shortcut>> GetShortcuts();
        Task AddShortcut(Shortcut shortcut);
        Task RemoveShortcut(string shortcutId);
    }
}
