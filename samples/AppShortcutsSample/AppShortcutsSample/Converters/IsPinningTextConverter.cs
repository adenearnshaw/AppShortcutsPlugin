using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShortcutsSample.Converters
{
    public class IsPinningTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPinned = System.Convert.ToBoolean(value);

            if (isPinned)
                return "Removing pin";
            else
                return "Adding pin";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
