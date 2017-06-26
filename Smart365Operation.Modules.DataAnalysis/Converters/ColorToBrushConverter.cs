using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Smart365Operation.Modules.DataAnalysis.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        //private static readonly IDictionary<Color, SolidColorBrush> _cachedBrushes = new Dictionary<Color, SolidColorBrush>();

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            SolidColorBrush brush;

            // No more caching: causes memory leak when the Legend is used
            // 
            //if (_cachedBrushes.TryGetValue(color, out brush))
            //{
            //    return brush;
            //}
            brush = new SolidColorBrush(color);
            //_cachedBrushes.Add(color, brush);

            return brush;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
