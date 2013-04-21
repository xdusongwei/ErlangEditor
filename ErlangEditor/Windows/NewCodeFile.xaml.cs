using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Dynamic;

namespace ErlangEditor.Windows
{
    /// <summary>
    /// NewCodeFile.xaml 的交互逻辑
    /// </summary>
    public partial class NewCodeFile : Window
    {
        public NewCodeFile()
        {
            InitializeComponent();
            dynamic bindingSource = new ExpandoObject();
            bindingSource.Name = "myModule.erl";
            bindingSource.ExportAll = false;
            bindingSource.Import = "[]";
            DataContext = bindingSource;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            dynamic context = DataContext;
            if (!string.IsNullOrWhiteSpace(context.Name))
            {
                context.Name = context.Name.Trim();
                DialogResult = true;
            }
        }
    }
}
