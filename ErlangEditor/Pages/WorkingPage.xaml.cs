using ErlangEditor.ViewModel;
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
    /// WorkingPage.xaml 的交互逻辑
    /// </summary>
    public partial class WorkingPage : UserControl
    {
        public WorkingPage()
        {
            InitializeComponent();
            DataContext = App.MainViewModel;
        }

        public string Title
        {
            get { return "开发"; }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
        }

        private void TreeCtrl_GotFocus(object sender, RoutedEventArgs e)
        {
            //LoadTreeItemToolBar();
        }

        private void TreeCtrl_LostFocus(object sender, RoutedEventArgs e)
        {
            //App.MainViewModel.ContextButtonsLeft.Clear();
        }

        private void LoadTreeItemToolBar()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            var item = rtvSolution.SelectedItem as ViewModel.PrjTreeItemVM;
            if (item == null || item.Entity == null) return;
            if (item.Entity is ErlangEditor.Core.Entity.SolutionEntity)
            {
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "应用",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.add.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => 
                    { 
                        Dispatcher.Invoke(new Action(()=>App.Navigation.GoFroward(new NewApp(item))), null); 
                    })
                });
            }
            if (item.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "文件",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.add.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => { App.Navigation.GoFroward(new NewFile(rtvSolution.SelectedItem as ViewModel.PrjTreeItemVM)); })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "分离",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.close.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => 
                    {
                        
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "删除",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.delete.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => { })
                });
            }
            if (item.Entity is ErlangEditor.Core.Entity.FolderEntity)
            {
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "文件",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.add.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => { App.Navigation.GoFroward(new NewFile(rtvSolution.SelectedItem as ViewModel.PrjTreeItemVM)); })
                });
            }
            if (item.Entity is ErlangEditor.Core.Entity.FileEntity)
            {
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "分离",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.close.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => { })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "删除",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.delete.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => { })
                });
            }
            
        }

        private void TreeCtrl_ItemChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTreeItemToolBar();
        }
    }
}
