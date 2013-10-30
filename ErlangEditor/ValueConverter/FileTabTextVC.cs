using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ErlangEditor.ValueConverter
{
    public class FileTabTextVC :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var name = values[0] as string;
            var state = (bool)values[1];
            var tb = new TextBlock{ Foreground = new SolidColorBrush(Colors.Black) , FontSize= 12 ,Margin = new System.Windows.Thickness(1,0,1,0)};
            tb.Text = string.Format("{0}{1}", name, state ? "*" : string.Empty);
            return tb;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
