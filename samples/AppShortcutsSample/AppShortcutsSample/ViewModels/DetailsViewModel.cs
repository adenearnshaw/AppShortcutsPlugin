using AppShortcutsSample.Models;
using AppShortcutsSample.Services;
using MvvmHelpers;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppShortcutsSample.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public DetailsViewModel(Monkey monkey)
        {
            Monkey = monkey;
            PinMonkeyCommand = new Command(PinMonkey, () => CanPinMonkeys);
            CheckIfPinned();
        }

        public bool CanPinMonkeys => PinMonkeyService.Instance.CanPinMonkeys;
        public Monkey Monkey { get; private set; }

        private bool _isMonkeyPinned;
        public bool IsMonkeyPinned
        {
            get => _isMonkeyPinned;
            private set => SetProperty(ref _isMonkeyPinned, value);
        }

        public ICommand PinMonkeyCommand { get; private set; }


        private async void CheckIfPinned()
        {
            IsMonkeyPinned = await PinMonkeyService.Instance.IsMonkeyPinned(Monkey.Id);
        }

        private async void PinMonkey()
        {
            try
            {
                if (!IsMonkeyPinned)
                    await PinMonkeyService.Instance.PinMonkey(Monkey);
                else
                    await PinMonkeyService.Instance.UnpinMonkey(Monkey.Id);

                CheckIfPinned();
            }
            catch (System.Exception)
            {
                MessagingCenter.Send(this, "ErrorDialog", "Sorry, cannot pin more than 4 monkeys.");
            }
        }
    }
}
