using ErlangEditor.ViewModel;
using ICSharpCode.AvalonEdit;
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
using Telerik.Windows.Controls;

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
                        App.Navigation.GoFroward(new NewApp(item)); 
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
                        App.Navigation.ShowYesNoBox(string.Format("确认将应用 {0} 分离吗?", item.DisplayText), "确认操作");
                        if (YesNoFrame.Result)
                        {
                            try
                            {
                                ErlangEditor.Core.ApplicationUtil.SeparateApplication(App.Entity.FindAppName(item.Entity));
                                (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(item);
                            }
                            catch (Exception ecp)
                            {
                                App.Navigation.ShowMessageBox(ecp.Message, "出错");
                            }
                        }
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "删除",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.delete.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => 
                    {
                        App.Navigation.ShowYesNoBox(string.Format("确认将应用 {0} 删除吗?", item.DisplayText), "确认操作");
                        if (YesNoFrame.Result)
                        {
                            try
                            {
                                ErlangEditor.Core.ApplicationUtil.DeleteApplication(App.Entity.FindAppName(item.Entity));
                                (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(item);
                            }
                            catch (Exception ecp)
                            {
                                App.Navigation.ShowMessageBox(ecp.Message, "出错");
                            }
                        }
                    })
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
                    ClickedAction = new Action(() => 
                    {
                        
                        App.Navigation.ShowYesNoBox(string.Format("确认将文件 {0} 分离吗?", item.DisplayText), "确认操作");
                        if (YesNoFrame.Result)
                        {
                            try
                            {
                                ErlangEditor.Core.FileUtil.SeparateFile(item.Entity as ErlangEditor.Core.Entity.FileEntity);
                                (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(item);
                            }
                            catch (Exception ecp)
                            {
                                App.Navigation.ShowMessageBox(ecp.Message, "出错");
                            }
                        }
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "删除",
                    ImageSource = new BitmapImage(new Uri("/Images/appbar.delete.rest.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() => 
                    {
                        App.Navigation.ShowYesNoBox(string.Format("确认将应用 {0} 删除吗?", item.DisplayText), "确认操作");
                        if (YesNoFrame.Result)
                        {
                            try
                            {
                                ErlangEditor.Core.FileUtil.RemoveFile(item.Entity as ErlangEditor.Core.Entity.FileEntity);
                                (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(item);
                            }
                            catch (Exception ecp)
                            {
                                App.Navigation.ShowMessageBox(ecp.Message, "出错");
                            }
                        }
                    })
                });
            }
            
        }

        private void TreeCtrl_ItemChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTreeItemToolBar();
        }

        private void rtvSolution_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = rtvSolution.SelectedItem as ViewModel.PrjTreeItemVM;
            if (vm == null) return;
            if (vm.Entity!=null && vm.Entity is ErlangEditor.Core.Entity.FileEntity)
            {
                try
                {
                    var entity = vm.Entity as ErlangEditor.Core.Entity.FileEntity;
                    var editor = new TextEditor();
                    //editor.Tag = openFileVM;
                    //editor.TextChanged += new EventHandler(editor_TextChanged);
                    var rp = new RadPane() { Title = entity.Name, Background = new SolidColorBrush(Colors.White) };
                    editor.Text = ErlangEditor.Core.FileUtil.LoadFile(entity);
                    editor.BorderBrush = new SolidColorBrush(Colors.White);
                    editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("Erl");
                    
                    rp.Content = editor;
                    rpContent.Items.Add(rp);
                }
                catch (Exception ecp)
                {
                    App.Navigation.ShowMessageBox(ecp.Message, "出错");
                }
            }
        }

        private void RadPane_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
