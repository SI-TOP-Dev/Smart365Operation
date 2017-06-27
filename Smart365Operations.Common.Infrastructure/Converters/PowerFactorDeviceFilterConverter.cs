using Smart365Operations.Common.Infrastructure.Models.TO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Smart365Operations.Common.Infrastructure.Converters
{
    public class PowerFactorDeviceFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<EquipmentDTO> allEquipments = value as ObservableCollection<EquipmentDTO>;
            if (allEquipments != null)
            {
                return new ObservableCollection<EquipmentDTO>(allEquipments.Where(e => e.isPowerFactor != 0));
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
