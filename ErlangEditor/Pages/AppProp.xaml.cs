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
    /// AppProp.xaml 的交互逻辑
    /// </summary>
    public partial class AppProp : UserControl
    {
        public AppProp(ViewModel.PrjTreeItemVM aVM)
        {
            InitializeComponent();
            vm_ = aVM;
        }

        public string Title
        {
            get
            {
                return "应用信息";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("保存", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var entity = vm_.Entity as ErlangEditor.Core.Entity.ApplicationEntity;
                    entity.AppMode = rbAppConfig.IsChecked ?? false;
                    entity.CodeMode = rbNormal.IsChecked ?? false;
                    entity.NoStartup = rbNostartup.IsChecked ?? false;
                    entity.StartupAsMFA = rbMFA.IsChecked ?? false;
                    entity.StartupMFA = tbMFA.Text.Trim();
                    entity.DebugInfo = rbDebug.IsChecked ?? false;
                    entity.CompileNative = rbNative.IsChecked ?? false;
                    var pa = tbPa.Text.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.Trim());
                    entity.IncludePath.Clear();
                    entity.IncludePath.AddRange(pa);
                    try
                    {
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                    }
                    catch (Exception e)
                    {
                        App.Navigation.ShowMessageBox(e.Message, "应用设置");
                    }
                })
            });
            App.ToolBox.ShowButtomBar();
        }

        private ViewModel.PrjTreeItemVM vm_;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbTitle.Text = vm_.DisplayText;
            var entity = vm_.Entity as ErlangEditor.Core.Entity.ApplicationEntity;
            tbName.Text = vm_.DisplayText;
            tbMFA.Text = entity.StartupMFA;
            rbAppConfig.IsChecked = entity.AppMode;
            rbNormal.IsChecked = entity.CodeMode;
            rbNostartup.IsChecked = entity.NoStartup;
            rbMFA.IsChecked = entity.StartupAsMFA;
            tbPa.Text = string.Join(";", entity.IncludePath);
            rbDebug.IsChecked = entity.DebugInfo;
            rbNoDebug.IsChecked = !entity.DebugInfo && !entity.CompileNative;
            rbNative.IsChecked = entity.CompileNative;
        }
    }
}
