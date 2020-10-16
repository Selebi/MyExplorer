using MyExplorer.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MyExplorer.Converters
{
    public class MessageImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType type)
            {
                switch (type)
                {
                    case MessageType.Error:
                        return "/Sintek Explorer;component/Resources/Error.png";
                    case MessageType.Warning:
                        return "/Sintek Explorer;component/Resources/Warning.png";
                    case MessageType.Info:
                        return "/Sintek Explorer;component/Resources/Info.png";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
