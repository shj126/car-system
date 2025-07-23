using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace inspection
{
    public class DataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string TypeInt = value.ToString();
            string TypeStr = string.Empty;
            switch (TypeInt)
            {
                case "1":
                    TypeStr = "故障";
                    break;
                case "2":
                    TypeStr = "虚接";
                    break;

                default:
                    TypeStr = "正常";
                    break;
            }

            return TypeStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string TypeStr = value.ToString();
            int TypeInt = 0;
            switch (TypeStr)
            {
                case "故障":
                    TypeInt = 1;
                    break;
                case "虚接":
                    TypeInt = 2;
                    break;
                default:
                    TypeInt = 0;
                    break;
            }

            return TypeInt;
        }
    }
}
