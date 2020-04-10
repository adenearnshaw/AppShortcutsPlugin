using Plugin.AppShortcuts;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShortcutsTests.Converters
{
    public class ShortcutToDetailsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Shortcut shortcut)
            {
                return $"{shortcut.Label} - {shortcut.Description} - {shortcut.Tag} - {shortcut.Icon?.IconName} - {shortcut.Uri}";
            }

            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
