using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppShortcutsSample.Converters
{
    public class IsPinnedIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isPinned = System.Convert.ToBoolean(value);

            if (isPinned)
                return ImageSource.FromFile("UnpinIcon.png");
            else
                return ImageSource.FromFile("PinIcon.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
