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
        }
    }
}