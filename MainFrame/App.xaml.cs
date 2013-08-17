using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MainFrame.ViewModel;

namespace MainFrame
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static NavigationHelper navigation_ = new NavigationHelper();
        public static NavigationHelper Navigation
        {
            get { return navigation_; }
        }

        private static ToolBoxHelper toolBox_ = new ToolBoxHelper();
        public static ToolBoxHelper ToolBox
        {
            get { return toolBox_; }
        }

        private static TasksHelper tasks_ = new TasksHelper();
        public static TasksHelper Tasks
        {
            get { return tasks_; }
        }

        private static readonly MainVM mainVM_ = new MainVM();
        public static MainVM MainViewModel
        {
            get { return mainVM_; }
        }
    }
}
