using AppShortcutsSample.Data;
using AppShortcutsSample.Models;
using System.Collections.ObjectModel;

namespace AppShortcutsSample.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Monkeys = new ObservableCollection<Monkey>(MonkeyStore.Instance.Monkeys);
        }

        public ObservableCollection<Monkey> Monkeys { get; set; }
    }
}
