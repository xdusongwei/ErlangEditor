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
    /// NewFile_gen_supervisor.xaml 的交互逻辑
    /// </summary>
    public partial class NewFile_supervisor : UserControl
    {
        public NewFile_supervisor(string aName, ViewModel.PrjTreeItemVM aVM)
        {
            InitializeComponent();
            vm_ = aVM;
            name_ = aName;
        }

        private ViewModel.PrjTreeItemVM vm_;
        private string name_;

        public string Title
        {
            get
            {
                return "配置模板";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("创建", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var modName = tbModName.Text.Trim();
                    var rs = (cbRS.SelectedValue as ComboBoxItem).Tag as string;
                    var max = tbMax.Text.Trim();
                    var within = tbWhitin.Text.Trim();
                    var id = tbID.Text.Trim();
                    var startMFA = tbStartupMFA.Text.Trim();
                    var restart = (cbRestart.SelectedValue as ComboBoxItem).Tag as string;
                    var shutdown = cbiShutdownCountdown.IsSelected ? tbShutdownCountdown.Text.Trim() : (cbShutdown.SelectedValue as ComboBoxItem).Tag as string;
                    var nodeType = (cbType.SelectedValue as ComboBoxItem).Tag as string;
                    var mods = tbMods.Text.Trim();
                    if (string.IsNullOrWhiteSpace(modName) || vm_ == null) return;
                    try
                    {
                        var fld = App.Entity.FindFolderName(vm_.Entity);
                        var app = App.Entity.FindAppName(vm_.Entity);
                        var content = ErlangEditor.Template.TemplateUtil.Make_supervisor(modName, rs, max, within, id, startMFA, restart, shutdown, nodeType, mods);
                        var entity = new ErlangEditor.Core.Entity.FileEntity { Name = modName + ".erl", DisplayName = modName };
                        ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                        App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "创建文件");
                    }
                })
            });
            App.ToolBox.ShowButtomBar();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbModName.Text = name_;
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
