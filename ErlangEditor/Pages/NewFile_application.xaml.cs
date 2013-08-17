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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErlangEditor.Pages
{
    /// <summary>
    /// NeewFile_application.xaml 的交互逻辑
    /// </summary>
    public partial class NewFile_application : UserControl
    {
        public NewFile_application()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "配置模板";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
        }
    }
}
