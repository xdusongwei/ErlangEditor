using Microsoft.Win32;
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
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : UserControl
    {
        public Setting()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "设置";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("保存", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var compilerpath = tbCompiler.Text;
                    var shellpath = tbShell.Text;
                    var consolepath = tbConsole.Text;
                    if (!string.IsNullOrEmpty(compilerpath) && !string.IsNullOrEmpty(shellpath) && !string.IsNullOrEmpty(consolepath))
                    {
                        try
                        {
                            ErlangEditor.Core.ConfigUtil.Config.CompilerPath = compilerpath;
                            ErlangEditor.Core.ConfigUtil.Config.ShellPath = shellpath;
                            ErlangEditor.Core.ConfigUtil.Config.ConsolePath = consolepath;
                            ErlangEditor.Core.ConfigUtil.SaveConfig();
                            App.Navigation.GoBackward();
                        }
                        catch (Exception e)
                        {
                            App.Navigation.ShowMessageBox(e.Message, "保存设置");
                        }
                    }
                })
            });
            App.ToolBox.ShowButtomBar();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbCompiler.Text = ErlangEditor.Core.ConfigUtil.Config.CompilerPath;
            tbShell.Text = ErlangEditor.Core.ConfigUtil.Config.ShellPath;
            tbConsole.Text = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
        }

        private void SreachCompilerClick(object sender, RoutedEventArgs e)
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

        private void SearchShellClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择shell";
            fileDialog.Filter = "Erlang shell(werl.exe)|werl.exe";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                tbShell.Text = file;
            }
        }

        private void SearchConsoleClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择shell";
            fileDialog.Filter = "Erlang shell(erl.exe)|erl.exe";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                tbConsole.Text = file;
            }
        }

        private void RefreshCache(object sender, RoutedEventArgs e)
        {
            App.MainViewModel.AutoCompleteCache.CacheLib();
            App.Navigation.ShowMessageBox("Erlang库代码的自动提示已经缓存完毕。","刷新缓存");
        }
    }
}
