using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace inspection
{
    public class ColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string TypeInt = value.ToString();
            switch (TypeInt)
            {
                case "1":
                    return new SolidColorBrush(Colors.Green);
                case "2":
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            throw new NotImplementedException();

        }
    }
}
