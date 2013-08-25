using ErlangEditor.ViewModel;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
            {
                Text = "保存",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0008_save.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() =>
                {
                    try
                    {
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        foreach (var i in App.MainViewModel.OpenedFiles.Where(i => i.Changed))
                        {
                            ErlangEditor.Core.FileUtil.SaveFile(i.FileEntity, i.Content);
                            i.Changed = false;
                        }
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                })
            });
            

            var item = rtvSolution.SelectedItem as ViewModel.PrjTreeItemVM;
            if (item == null || item.Entity == null) return;
            if (item.Entity is ErlangEditor.Core.Entity.SolutionEntity)
            {
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "全部编译",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0015_reload.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        UpdateTabCollection();
                        App.Compile.MakeSolution();
                    })
                });
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
                    Text = "全部编译",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0015_reload.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        UpdateTabCollection();
                        App.Compile.MakeSolution();
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "编译应用",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0013_APP-info.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        UpdateTabCollection();
                        App.Compile.MakeApp(item);
                    })
                });
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
                    Text = "全部编译",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0015_reload.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        UpdateTabCollection();
                        App.Compile.MakeSolution();
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "编译应用",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0013_APP-info.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        var vm = rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM;
                        UpdateTabCollection();
                        App.Compile.MakeApp(vm);
                    })
                });
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
                    Text = "全部编译",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0015_reload.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        UpdateTabCollection();
                        App.Compile.MakeSolution();
                    })
                });
                App.MainViewModel.ContextButtonsLeft.Add(new ToolBoxButtonVM
                {
                    Text = "编译应用",
                    ImageSource = new BitmapImage(new Uri("/Images/MB_0013_APP-info.png", UriKind.RelativeOrAbsolute)),
                    ClickedAction = new Action(() =>
                    {
                        var h = rtvSolution.SelectedContainer.ParentItem;
                        while (!((h.Item as ViewModel.PrjTreeItemVM).Entity is ErlangEditor.Core.Entity.ApplicationEntity))
                        {
                            h = h.ParentItem;
                        }
                        UpdateTabCollection();
                        App.Compile.MakeApp(h.Item as ViewModel.PrjTreeItemVM);
                    })
                });
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
                    if (App.MainViewModel.OpenedFiles.Any(i => i.FileName == entity.Name))
                    {
                        rpContent.SelectedItem = App.MainViewModel.OpenedFiles.First(i => i.FileName == entity.Name).Pane;
                        return;
                    }
                    var editor = new TextEditor();
                    var rp = new RadPane();
                    var fileVM = new ViewModel.FileVM() { FileName = entity.Name, Pane = rp, Editor = editor, FileEntity = entity };
                    editor.Text = ErlangEditor.Core.FileUtil.LoadFile(entity);
                    editor.TextChanged += (a, b) => { fileVM.Changed = true; };
                    editor.BorderBrush = new SolidColorBrush(Colors.White);
                    editor.FontSize = 16;
                    editor.ShowLineNumbers = true;
                    editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("Erl");
                    rp.Content = editor;
                    rp.Tag = fileVM;
                    rpContent.Items.Add(rp);
                    var b1 = new Binding("FileName") { Source = fileVM, Mode = BindingMode.OneWay };
                    var b2 = new Binding("Changed") { Source = fileVM, Mode = BindingMode.OneWay };
                    var mb = new MultiBinding() { Mode = BindingMode.OneWay };
                    mb.Bindings.Add(b1);
                    mb.Bindings.Add(b2);
                    mb.Converter = new ValueConverter.FileTabTextVC();
                    rp.SetBinding(RadPane.HeaderProperty, mb);
                    App.MainViewModel.OpenedFiles.Add(fileVM);
                }
                catch (Exception ecp)
                {
                    App.Navigation.ShowMessageBox(ecp.Message, "出错");
                }
            }
        }

        private void UpdateTabCollection()
        {
            App.MainViewModel.OpenedFiles.Clear();
            foreach (var i in rpContent.Items)
            {
                var pane = i as RadPane;
                var fileVM = pane.Tag as ViewModel.FileVM;
                App.MainViewModel.OpenedFiles.Add(fileVM);
            }
        }

        private void AddNode(object sender, RoutedEventArgs e)
        {
            App.Navigation.GoFroward(new NodeProp());
        }
    }
}
