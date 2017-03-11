using RestSharp;
using Smart365Operations.Common.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Smart365Operations.Common.Infrastructure.Converters
{
    public class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = value as string;
            DataServiceApi dataServcie = new DataServiceApi();
            var request = new RestRequest(uri);
            var data = dataServcie.Download(request);
            ImageSource imageSource;
            var source = new System.Windows.Media.Imaging.BitmapImage();

            source.BeginInit();
            source.StreamSource = new System.IO.MemoryStream(data.ToArray());
            source.EndInit();

            // use public setter
           return imageSource = source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
