﻿using AppShortcutsSample.Models;
using AppShortcutsSample.Services;
using MvvmHelpers;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

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


        private async Task CheckIfPinned()
        {
            if (!CanPinMonkeys)
                return;

            IsMonkeyPinned = await PinMonkeyService.Instance.IsMonkeyPinned(Monkey.Id);
        }

        private async void PinMonkey()
        {
            IsBusy = true;

            if (!CanPinMonkeys)
                return;

            try
            {
                if (!IsMonkeyPinned)
                    await PinMonkeyService.Instance.PinMonkey(Monkey);
                else
                    await PinMonkeyService.Instance.UnpinMonkey(Monkey.Id);

                if (Device.RuntimePlatform == Device.iOS)
                    await Task.Delay(10);

                await CheckIfPinned();
            }
            catch (System.Exception)
            {
                MessagingCenter.Send(this, "ErrorDialog", "Sorry, cannot pin more than 4 monkeys.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
