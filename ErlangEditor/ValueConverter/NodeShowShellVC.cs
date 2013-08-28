using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ErlangEditor.ValueConverter
{
    public class NodeShowShellVC : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (bool)value;
            return state ?
                new BitmapImage(new Uri("/Images/appbar.upload.rest.png", UriKind.RelativeOrAbsolute)) :
                new BitmapImage(new Uri("/Images/appbar.download.rest.png", UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
