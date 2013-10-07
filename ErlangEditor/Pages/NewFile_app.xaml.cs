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
    /// NewFile_app.xaml 的交互逻辑
    /// </summary>
    public partial class NewFile_app : UserControl
    {
        public NewFile_app(string aName, ViewModel.PrjTreeItemVM aVM)
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
                return "配置app文件";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("创建", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var appname = tbAppName.Text.Trim();
                    var vsn = tbVsn.Text.Trim();
                    var desc = tbDesc.Text.Trim();
                    var mods = tbMods.Text.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                    var reg = tbReg.Text.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                    var apps = tbApps.Text.Split(',').Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                    var mod = tbMod.Text.Trim();
                    var arg = tbArg.Text.Trim();
                    if (string.IsNullOrWhiteSpace(appname) || vm_ == null) return;
                    try
                    {
                        var fld = App.Entity.FindFolderName(vm_.Entity);
                        var app = App.Entity.FindAppName(vm_.Entity);
                        var content = ErlangEditor.Template.TemplateUtil.MakeAppFile(vsn, appname, desc, mods, reg, apps, mod, arg);
                        var entity = new ErlangEditor.Core.Entity.FileEntity { Name = appname + ".app.src", DisplayName = appname + ".app", IsAppFile = true };
                        ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                        SortChildren(vm_);
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

        private void SortChildren(ViewModel.PrjTreeItemVM node)
        {
            var comparer = new Tools.Reverser<ViewModel.PrjTreeItemVM>(new ViewModel.PrjTreeItemVM().GetType(), "DisplayText", Tools.ReverserInfo.Direction.ASC);
            var lst = node.Children.ToList();
            lst.Sort(comparer);
            node.Children.Clear();
            foreach (var i in lst)
                node.Children.Add(i);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tbAppName.Text = name_;
        }
    }
}
