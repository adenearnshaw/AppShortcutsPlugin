using AppShortcutsSample.Models;
using AppShortcutsSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppShortcutsSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage(Monkey monkey)
        {
            InitializeComponent();
            BindingContext = new DetailsViewModel(monkey);
            MessagingCenter.Subscribe<DetailsViewModel, string>(this, "ErrorDialog", (sender, args) =>
            {
                DisplayAlert("Error", args, "OK");
            });
        }
    }
}