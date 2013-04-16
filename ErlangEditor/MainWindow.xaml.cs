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
using Microsoft.Win32;

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

        private void openSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择解决方案";
            fileDialog.Filter = "Erlang solution(*.sln)|*sln";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                App.ViewModel.LoadSolution(file);
            }
        }

        private void saveSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            App.ViewModel.SaveSolution();
        }
    }
}
