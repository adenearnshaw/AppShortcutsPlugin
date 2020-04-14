using AppShortcutsSample.Data;
using AppShortcutsSample.Models;
using Plugin.AppShortcuts;
using System.Linq;
using System.Threading.Tasks;
using Plugin.AppShortcuts.Icons;

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
            var monkey = MonkeyStore.Instance.Monkeys.FirstOrDefault(m => m.Id.Equals(monkeyId));

            if (monkey == null)
                return false;

            var shorcuts = await CrossAppShortcuts.Current.GetShortcuts();

            return shorcuts.Any(s => s.Tag?.Equals(monkey.Id) ?? false);
        }

        public async Task PinMonkey(Monkey monkey)
        {
            if (await MaxNumberOfPinsReached())
                throw new MaxPinnedItemsException();

            if (await IsMonkeyPinned(monkey.Id))
                return;

            var shortcut = new Shortcut()
            {
                Label = monkey.Name,
                Description = monkey.Name,
                Icon = new FavoriteIcon(),
                Uri = $"{App.AppShortcutUriBase}{monkey.Id}",
                Tag = monkey.Id
            };

            await CrossAppShortcuts.Current.AddShortcut(shortcut);
        }

        public async Task UnpinMonkey(string monkeyId)
        {
            var shortcut = (await CrossAppShortcuts.Current.GetShortcuts()).FirstOrDefault(s => s.Tag.Equals(monkeyId));

            await CrossAppShortcuts.Current.RemoveShortcut(shortcut.ShortcutId);
        }

        private async Task<bool> MaxNumberOfPinsReached()
        {
            var shorcuts = await CrossAppShortcuts.Current.GetShortcuts();
            return shorcuts.Count >= MaxPinsThreshold;
        }
    }
}
