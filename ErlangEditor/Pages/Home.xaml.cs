using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            App.Navigation.GoFroward(new ErlangEditor.Pages.CreateProject(this));
        }

        private void OpenPrj(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择项目文件";
            fileDialog.Filter = "Erlang solution(*.sln)|*sln";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                OpenProject(file);
            }
        }

        private void OpenProject(string file)
        {
            try
            {
                ErlangEditor.Core.SolutionUtil.LoadSolution(file);
                App.MainViewModel.WorkingPage = new WorkingPage();
                App.Entity.UpdateProjectTree();
                App.MainViewModel.AutoCompleteCache.ScanAllBin(ErlangEditor.Core.SolutionUtil.Solution);
                InsertRecentProject(new ViewModel.RecentProjectVM(new Core.Entity.RecentProjectEntity
                {
                    Title = ErlangEditor.Core.SolutionUtil.Solution.Name,
                    Path = file
                }));
                App.Navigation.GoFroward(App.MainViewModel.WorkingPage);
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "打开项目");
            }
        }

        private void RecentItemClick(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as ViewModel.RecentProjectVM;
            OpenProject(vm.Path);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.ToolBox.HideButtomBar();
            RefreshRecentList();
        }

        private void RefreshRecentList()
        {
            var lst = new ObservableCollection<ViewModel.RecentProjectVM>();
            foreach (var i in ErlangEditor.Core.ConfigUtil.Config.RecentProject)
            {
                lst.Add(new ViewModel.RecentProjectVM(i));
            }
            lstRecent.ItemsSource = lst;
        }

        public void InsertRecentProject(ViewModel.RecentProjectVM aVM)
        {
            var entity = aVM.Entity;
            var cfg = ErlangEditor.Core.ConfigUtil.Config;
            if (entity != null && cfg.RecentProject.Any(x=>entity.Title == x.Title && entity.Path == x.Path))
            {
                cfg.RecentProject = new List<Core.Entity.RecentProjectEntity>(cfg.RecentProject.Where(x => entity.Title != x.Title || entity.Path != x.Path));
            }
            cfg.RecentProject.Insert(0, entity);
            cfg.RecentProject = new List<Core.Entity.RecentProjectEntity>(cfg.RecentProject.Take(10));
            RefreshRecentList();
            ErlangEditor.Core.ConfigUtil.SaveConfig();
        }

        private void DeleteRecentProject(ViewModel.RecentProjectVM aVM)
        {
            var entity = aVM.Entity;
            var cfg = ErlangEditor.Core.ConfigUtil.Config;
            if (entity != null && cfg.RecentProject.Contains(entity))
            {
                cfg.RecentProject.Remove(entity);
            }
            RefreshRecentList();
            ErlangEditor.Core.ConfigUtil.SaveConfig();
        }
    }
}
