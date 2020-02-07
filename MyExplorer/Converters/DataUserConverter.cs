using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace MyExplorer.Converters
{
    class DataUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is  IEnumerable<Data.User> users)
            {
                List<string> result = new List<string>();
                foreach (var user in users)
                {
                    result.Add($"[{user.Type}] {user.Domain}/{user.Name}");
                }
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
