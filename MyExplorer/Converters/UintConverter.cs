using System;
using System.Globalization;
using System.Windows.Data;

namespace MyExplorer.Converters
{
    public class UintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is uint u)
            {
                return u.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (uint.TryParse((string)value, out uint result))
            {
                return result;
            }
            return 0;
        }
    }
}
