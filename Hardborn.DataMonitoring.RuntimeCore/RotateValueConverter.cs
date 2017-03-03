using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class RotateValueConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double num;
            if (((value != null) && (targetType == typeof(double))) && double.TryParse(value.ToString(), out num))
            {
                return num;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }


}
