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
using Microsoft.Win32;

namespace ErlangEditor.Windows
{
    /// <summary>
    /// NewSolution.xaml 的交互逻辑
    /// </summary>
    public partial class NewSolution : Window
    {
        public NewSolution()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void SetCompiler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择编译器";
            fileDialog.Filter = "Erlang compiler(erlc.exe)|erlc.exe";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                tbCompiler.Text = file;
            }
        }

        private void SetShell(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择shell";
            fileDialog.Filter = "Erlang shell(erl.exe)|erl.exe";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                tbShell.Text = file;
            }
        }

        private void SetPath(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "选择解决方案路径";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string foldPath = dialog.SelectedPath;
                    tbBasePath.Text = foldPath;
                }
            }
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSlnName.Text) ||
                string.IsNullOrWhiteSpace(tbCompiler.Text) ||
                string.IsNullOrWhiteSpace(tbShell.Text) ||
                string.IsNullOrWhiteSpace(tbBasePath.Text))
            {
                return;
            }

        }
    }
}
