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
    /// NodeProp.xaml 的交互逻辑
    /// </summary>
    public partial class NodeProp : UserControl
    {
        public NodeProp()
            : this(null)
        {

        }

        public NodeProp(ViewModel.NodeVM aVM)
        {
            node_ = aVM;
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return "节点属性";
            }
        }

        private ViewModel.NodeVM node_ = null;

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("确认", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var name = tbName.Text.Trim();
                    var show = rbShow.IsChecked ?? false;
                    if (string.IsNullOrWhiteSpace(name)) return;
                    try
                    {
                        ErlangEditor.Core.NodeUtil.AddNode(name, show);
                        App.MainViewModel.Nodes.Add(new ViewModel.NodeVM(ErlangEditor.Core.SolutionUtil.Solution.Nodes.First(i => i.NodeName == name)));
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                    }
                    catch (Exception e)
                    {
                        App.Navigation.ShowMessageBox(e.Message, "节点属性");
                    }
                })
            });
            App.ToolBox.ShowButtomBar();
        }
    }
}
