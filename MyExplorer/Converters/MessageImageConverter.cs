using MyExplorer.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyExplorer.Converters
{
    public class MessageImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is MessageType type)
            {
                switch (type)
                {
                    case MessageType.Error:
                        return "/AssemblyName;component/Images/Error.png";
                    case MessageType.Warning:
                        return "/AssemblyName;component/Images/Warning.png";
                    case MessageType.Info:
                        return "/AssemblyName;component/Images/Info.png";
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
