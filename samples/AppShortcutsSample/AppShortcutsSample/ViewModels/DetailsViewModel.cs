using AppShortcutsSample.Models;
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppShortcutsSample.ViewModels
{
    public class DetailsViewModel
    {
        private readonly bool _isPinningSupported;

        public DetailsViewModel(Monkey monkey)
        {
            Monkey = monkey;
            _isPinningSupported = CrossAppShortcuts.IsSupported;

            PinMonkeyCommand = new Command(PinMonkey, () => _isPinningSupported);
        }

        public Monkey Monkey { get; private set; }
        public ICommand PinMonkeyCommand { get; private set; }


        private async void PinMonkey()
        {
            if (!CrossAppShortcuts.IsSupported)
                return;

            var shortcut = new Shortcut()
            {
                Label = Monkey.Name,
                Description = Monkey.Name,
                Icon = ShortcutIconType.Favorite,
                Uri = $"{App.AppShortcutUriBase}{Monkey.Id}"
            };

            await CrossAppShortcuts.Current.AddShortcut(shortcut);
        }
    }
}
