using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShortcutsSample.Converters
{
    public class IsPinnedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPinned = System.Convert.ToBoolean(value);

            if (isPinned)
                return "Unpin";
            else
                return "Pin";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
