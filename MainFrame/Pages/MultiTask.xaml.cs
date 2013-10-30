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

namespace MainFrame.Pages
{
    /// <summary>
    /// MultiTask.xaml 的交互逻辑
    /// </summary>
    [TaskFrame(IsSingleFrame = false, CanClose = true)]
    public partial class MultiTask : UserControl
    {
        public MultiTask()
        {
            InitializeComponent();
        }
    }
}
