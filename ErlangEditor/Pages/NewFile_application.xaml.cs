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
    /// NeewFile_application.xaml 的交互逻辑
    /// </summary>
    public partial class NewFile_application : UserControl
    {
        public NewFile_application(string aName, ViewModel.PrjTreeItemVM aVM)
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
                return "配置application行为参数";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("创建", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var mfa = tbMFA.Text.Trim();
                    if (string.IsNullOrWhiteSpace(mfa) || vm_ == null) return;
                    try
                    {
                        var fld = App.Entity.FindFolderName(vm_.Entity);
                        var app = App.Entity.FindAppName(vm_.Entity);
                        var content = ErlangEditor.Template.TemplateUtil.Make_application(name_, mfa);
                        var entity = new ErlangEditor.Core.Entity.FileEntity { Name = name_ + ".erl", DisplayName = name_ };
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
    }
}
