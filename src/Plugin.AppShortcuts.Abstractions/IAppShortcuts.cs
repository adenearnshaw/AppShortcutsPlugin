using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.AppShortcuts.Abstractions
{
    public interface IAppShortcuts
    {
        Task<List<Shortcut>> GetShortcuts();
        Task AddShortcut(Shortcut shortcut);
        Task RemoveShortcut(string shortcutId);
    }
}
