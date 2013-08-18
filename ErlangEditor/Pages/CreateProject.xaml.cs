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
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProject : UserControl
    {
        public CreateProject()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return "创建新的项目"; }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("创建", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(()=>
                    {
                        var name = tbName.Text.Trim();
                        var fld = tbFolder.Text.Trim();
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(fld))
                        {
                            try
                            {
                                ErlangEditor.Core.SolutionUtil.CreateSolution(name, fld);
                                App.Entity.UpdateProjectTree();
                                App.MainViewModel.WorkingPage = new WorkingPage();
                                App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                            }
                            catch(Exception e)
                            {
                                App.Navigation.ShowMessageBox(e.Message, "创建项目");
                            }
                        }
                    })
            });
            App.ToolBox.ShowButtomBar();
        }

        private void SetPath(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = "选择项目的保存路径";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string foldPath = dialog.SelectedPath;
                    tbFolder.Text = foldPath;
                }
            }
        }
    }
}
