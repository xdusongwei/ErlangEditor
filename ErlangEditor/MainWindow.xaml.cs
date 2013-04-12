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
using ErlangEditor.Windows;

namespace ErlangEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!App.ViewModel.Loaded)
                App.ViewModel.Load();
        }

        private void createNewSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            NewSolution ns = new NewSolution();
            ns.ShowDialog();
        }
    }
}
