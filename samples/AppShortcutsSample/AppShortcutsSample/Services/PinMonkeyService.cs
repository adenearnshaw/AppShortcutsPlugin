using AppShortcutsSample.Data;
using AppShortcutsSample.Models;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace AppShortcutsSample.Services
{
    public class PinMonkeyService
    {
        private static PinMonkeyService _instance;
        public static PinMonkeyService Instance => _instance ?? (_instance = new PinMonkeyService());

        private const int MaxPinsThreshold = 4;

        private PinMonkeyService()
        {

        }

        public bool CanPinMonkeys => CrossAppShortcuts.IsSupported;

        public async Task<bool> IsMonkeyPinned(string monkeyId)
        {
            var shorcuts = await CrossAppShortcuts.Current.GetShortcuts();
            var monkeyShortcut = shorcuts.FirstOrDefault(s => s.Uri.Contains(monkeyId));

            return monkeyShortcut != null;
        }

        public async Task PinMonkey(Monkey monkey)
        {
            if (await MaxNumberOfPinsReached())
                throw new MaxPinnedItemsException();

            var shortcut = new Shortcut()
            {
                Label = monkey.Name,
                Description = monkey.Name,
                Icon = ShortcutIconType.Favorite,
                Uri = $"{App.AppShortcutUriBase}{monkey.Id}"
            };

            await CrossAppShortcuts.Current.AddShortcut(shortcut);
            MonkeyStore.Instance.UpdateMonkeyShortcutId(monkey.Id, shortcut.ID);
        }

        public async Task UnpinMonkey(string monkeyId)
        {
            var monkey = MonkeyStore.Instance.Monkeys.FirstOrDefault(m => m.Id.Equals(monkeyId));

            if (monkey == null)
                return;

            await CrossAppShortcuts.Current.RemoveShortcut(monkey.ShortcutId);
        }

        private async Task<bool> MaxNumberOfPinsReached()
        {
            var shorcuts = await CrossAppShortcuts.Current.GetShortcuts();
            return shorcuts.Count >= MaxPinsThreshold;
        }
    }
}
