using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace ErlangEditor.ValueConverter
{
    public class NodeAppsVC : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coll = value as ObservableCollection<string>;
            if (coll.Count == 0)
                return "没有应用的空节点。";
            return string.Format("{0}加入该节点。", string.Join(", ", coll));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
