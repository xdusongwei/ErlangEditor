using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ErlangEditor.ViewModel;

namespace ErlangEditor
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

        private static readonly MainVM mainVM_ = new MainVM();
        public static MainVM MainViewModel
        {
            get { return mainVM_; }
        }

        private static EntityHelper entity_ = new EntityHelper();
        public static EntityHelper Entity
        {
            get { return entity_; }
        }

        private static Helper.CompileHelper compile_ = new Helper.CompileHelper();
        public static Helper.CompileHelper Compile
        {
            get { return compile_; }
        }
    }
}
