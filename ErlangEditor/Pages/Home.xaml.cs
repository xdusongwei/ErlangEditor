using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "欢迎";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.ToolBox.HideButtomBar();
        }

        private void NewPrj(object sender, RoutedEventArgs e)
        {
            App.Navigation.GoFroward(new ErlangEditor.Pages.CreateProject());
        }

        private void OpenPrj(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择项目文件";
            fileDialog.Filter = "Erlang solution(*.sln)|*sln";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                try
                {
                    ErlangEditor.Core.SolutionUtil.LoadSolution(file);
                    App.MainViewModel.WorkingPage = new WorkingPage();
                    App.Entity.UpdateProjectTree();
                }
                catch (Exception ecp)
                {
                    App.Navigation.ShowMessageBox(ecp.Message, "打开项目");
                }
                //App.ViewModel.LoadSolution(file);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.ToolBox.HideButtomBar();
        }
    }
}
