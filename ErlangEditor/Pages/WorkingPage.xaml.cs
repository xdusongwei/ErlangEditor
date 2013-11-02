using ErlangEditor.Entity;
using ErlangEditor.ViewModel;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
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
            LoadTreeItemToolBar();
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
                    SaveProject();
                })
            });
            
        }

        private static void SaveProject()
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
        }

        private void TreeCtrl_ItemChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTreeItemToolBar();
            foreach (var i in e.RemovedItems)
            {
                var vm = i as ViewModel.PrjTreeItemVM;
                vm.RemoveToolbarVisibility = System.Windows.Visibility.Collapsed;
                vm.PropToolbarVisibility = System.Windows.Visibility.Collapsed;
                vm.AddToolbarVisibility = System.Windows.Visibility.Collapsed;
                vm.CompileToolbarVisibility = System.Windows.Visibility.Collapsed;
                vm.SaveToolbarVisibility = System.Windows.Visibility.Collapsed;
            }
            foreach (var i in e.AddedItems)
            {
                var vm = i as ViewModel.PrjTreeItemVM;
                if (vm.Entity is ErlangEditor.Core.Entity.SolutionEntity)
                    vm.SaveToolbarVisibility = System.Windows.Visibility.Visible;
                if (!(vm.Entity is ErlangEditor.Core.Entity.SolutionEntity) && !(vm.Entity is ErlangEditor.Core.Entity.FolderEntity))
                    vm.RemoveToolbarVisibility = System.Windows.Visibility.Visible;
                if (!(vm.Entity is ErlangEditor.Core.Entity.FileEntity))
                    vm.AddToolbarVisibility = System.Windows.Visibility.Visible;
                if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
                    vm.PropToolbarVisibility = System.Windows.Visibility.Visible;
                if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity || vm.Entity is ErlangEditor.Core.Entity.SolutionEntity)
                    vm.CompileToolbarVisibility = System.Windows.Visibility.Visible;
            }
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
                    var fileVM = new ViewModel.FileVM()
                    {
                        FileName = entity.Name,
                        Pane = rp,
                        Editor = editor,
                        FileEntity = entity,
                        ItemVM = vm
                    };
                    editor.Text = ErlangEditor.Core.FileUtil.LoadFile(entity);
                    editor.TextChanged += (a, b) => { fileVM.Changed = true; };
                    editor.BorderBrush = new SolidColorBrush(Colors.White);
                    editor.FontSize = 16;
                    editor.ShowLineNumbers = true;
                    editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("Erl");
                    editor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
                    editor.TextArea.TextEntered += textEditor_TextArea_TextEntered;
                    editor.FontSize = 12;
                    rp.Content = editor;
                    rp.Tag = fileVM;
                    rp.Unloaded += (a, b) => { Debug.WriteLine(string.Format("unloaded,{0}", a)); var panel = a as RadPane; App.MainViewModel.OpenedFiles.Remove(panel.Tag as FileVM); };
                    rp.Loaded += (a, b) => 
                    { 
                        Debug.WriteLine(string.Format("loaded,{0}", a)); 
                        var panel = a as RadPane;
                        var fvm = panel.Tag as FileVM;
                        if (!App.MainViewModel.OpenedFiles.Contains(fvm))
                            App.MainViewModel.OpenedFiles.Add(fvm);
                    };
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

        CompletionWindow completionWindow;

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            var textArea = sender as TextArea;
            if(e.Text == "\'")
            {
                completionWindow = new CompletionWindow(textArea);
                completionWindow.Width = 256;
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                var mods = App.MainViewModel.AutoCompleteCache.GetModules;
                foreach (var i in mods)
                {
                    data.Add(new CompletionData(true) { Text = i + "\':", Content = i , Description = App.MainViewModel.AutoCompleteCache.GetModuleDesc(i)});
                }
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void AddNode(object sender, RoutedEventArgs e)
        {
            App.Navigation.GoFroward(new NodeProp());
        }

        private void ItemBoot(object sender, RoutedEventArgs e)
        {
            try
            {
                var vm = (sender as FrameworkElement).Tag as ViewModel.NodeVM;
                if (vm.Entity.IsRunning) return;
                ErlangEditor.Core.NodeUtil.StartupNode(vm.Name);
                vm.Proxy.Run(vm.Entity);
                vm.State = true;
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "运行出错提示");
                try
                {
                    var vm = (sender as FrameworkElement).Tag as ViewModel.NodeVM;
                    ErlangEditor.Core.NodeUtil.StopNode(vm.Name);
                    vm.Proxy.Stop();
                }
                catch { }
            }
        }

        private void ItemShutdown(object sender, RoutedEventArgs e)
        {
            try
            {
                var vm = (sender as FrameworkElement).Tag as ViewModel.NodeVM;
                vm.Proxy.Stop();
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "出错");
            }
        }

        private void ItemMouseMove(object sender, MouseEventArgs e)
        {
            var panel = sender as FrameworkElement;
            var vm = panel.Tag as ViewModel.PrjTreeItemVM;
            //vm.ToolbarVisibility = System.Windows.Visibility.Visible;
            if (vm != null && e.LeftButton == MouseButtonState.Pressed && vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                dragVM_ = vm;
                DragDrop.DoDragDrop(panel, vm, DragDropEffects.Copy);
            }
        }

        private ViewModel.PrjTreeItemVM dragVM_;
        private void NodeDragEnter(object sender, DragEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = new SolidColorBrush(Colors.Yellow);
        }

        private void NodeDragLeave(object sender, DragEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void NodeDrop(object sender, DragEventArgs e)
        {
            var panel = sender as StackPanel;
            panel.Background = new SolidColorBrush(Colors.Transparent);
            var vm = dragVM_;
            if (vm != null)
            {
                var entity = vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity;
                if (entity != null)
                {
                    try
                    {
                        var dest = panel.Tag as ViewModel.NodeVM;
                        if (dest.Entity.Apps.Contains((vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity).Name))
                        {
                            ErlangEditor.Core.NodeUtil.SeparateApp(dest.Name, (vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity).Name);
                            var apps = new System.Collections.ObjectModel.ObservableCollection<string>(dest.Entity.Apps);
                            dest.AppNames = apps;
                            ErlangEditor.Core.SolutionUtil.SaveSolution();
                        }
                        else
                        {
                            ErlangEditor.Core.NodeUtil.InjectionApp(dest.Name, (vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity).Name);
                            var apps = new System.Collections.ObjectModel.ObservableCollection<string>(dest.Entity.Apps);
                            dest.AppNames = apps;
                            ErlangEditor.Core.SolutionUtil.SaveSolution();
                        }
                    }
                    catch(Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                }
            }
        }

        private void ItemSetting(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as PrjTreeItemVM;
            if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                App.Navigation.GoFroward(new AppProp(vm));
            }
        }

        private void ItemAdd(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as PrjTreeItemVM;
            if (vm.Entity is ErlangEditor.Core.Entity.SolutionEntity)
            {
                App.Navigation.GoFroward(new NewApp(vm)); 
            }
            else
            {
                App.Navigation.GoFroward(new NewFile(vm));
            }
        }

        private void ItemSep(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as PrjTreeItemVM;
            if (vm.Entity is ErlangEditor.Core.Entity.FileEntity)
            {
                App.Navigation.ShowYesNoBox(string.Format("确认将文件 {0} 分离吗?", vm.DisplayText), "确认操作");
                if (YesNoFrame.Result)
                {
                    try
                    {
                        ErlangEditor.Core.FileUtil.SeparateFile(vm.Entity as ErlangEditor.Core.Entity.FileEntity);
                        (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(vm);
                        if (System.IO.Path.GetExtension((vm.Entity as ErlangEditor.Core.Entity.FileEntity).Name).ToLower() == ".erl")
                            App.MainViewModel.AutoCompleteCache.DropModule((vm.Entity as ErlangEditor.Core.Entity.FileEntity).DisplayName);
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                }
            }
            if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                App.Navigation.ShowYesNoBox(string.Format("确认将应用 {0} 分离吗?", vm.DisplayText), "确认操作");
                if (YesNoFrame.Result)
                {
                    try
                    {
                        ErlangEditor.Core.ApplicationUtil.SeparateApplication(App.Entity.FindAppName(vm.Entity));
                        (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(vm);
                        App.MainViewModel.AutoCompleteCache.DropApplication(vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity);
                        foreach (var i in App.MainViewModel.Nodes)
                        {
                            if (i.AppNames.Contains(vm.DisplayText))
                            {
                                ErlangEditor.Core.NodeUtil.SeparateApp(i.Name, vm.DisplayText);
                                var apps = new System.Collections.ObjectModel.ObservableCollection<string>(i.Entity.Apps);
                                i.AppNames = apps;
                            }
                        }
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                }
            }
        }

        private void ItemSave(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }

        private void ItemCompile(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as PrjTreeItemVM;
            SaveProject();
            try
            {
                if (vm.Entity is ErlangEditor.Core.Entity.SolutionEntity)
                {
                    App.Compile.MakeSolution();
                }
                if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
                {
                    App.Compile.MakeApp(vm);
                }
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "编译出错提示");
            }
        }

        private void ItemDeleteNode(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as NodeVM;
            App.Navigation.ShowYesNoBox(string.Format("确认要将{0}节点删除吗?", vm.Name), "删除节点");
            if (YesNoFrame.Result)
            {
                try
                {
                    ErlangEditor.Core.NodeUtil.DeleteNode(vm.Name);
                    ErlangEditor.Core.SolutionUtil.SaveSolution();
                    App.MainViewModel.Nodes.Remove(vm);
                }
                catch (Exception ecp)
                {
                    App.Navigation.ShowMessageBox(ecp.Message, "出错");
                }
            }
        }

        private void ItemVisibilityChange(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as ViewModel.NodeVM;
            vm.ShowShell = !vm.ShowShell;
            try
            {
                ErlangEditor.Core.SolutionUtil.SaveSolution();
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "出错");
            }
        }

        private void ItemRemove(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).Tag as PrjTreeItemVM;
            if (vm.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                App.Navigation.ShowYesNoBox(string.Format("确认将应用 {0} 删除吗?", vm.DisplayText), "确认操作");
                if (YesNoFrame.Result)
                {
                    try
                    {
                        ErlangEditor.Core.ApplicationUtil.DeleteApplication(App.Entity.FindAppName(vm.Entity));
                        (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(vm);
                        foreach (var i in App.MainViewModel.Nodes)
                        {
                            if (i.AppNames.Contains(vm.DisplayText))
                            {
                                ErlangEditor.Core.NodeUtil.SeparateApp(i.Name, vm.DisplayText);
                                var apps = new System.Collections.ObjectModel.ObservableCollection<string>(i.Entity.Apps);
                                i.AppNames = apps;
                            }
                        }
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        App.MainViewModel.AutoCompleteCache.DropApplication(vm.Entity as ErlangEditor.Core.Entity.ApplicationEntity);
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                }
            }
            if (vm.Entity is ErlangEditor.Core.Entity.FileEntity)
            {
                App.Navigation.ShowYesNoBox(string.Format("确认将文件 {0} 删除吗?", vm.DisplayText), "确认操作");
                if (YesNoFrame.Result)
                {
                    try
                    {
                        ErlangEditor.Core.FileUtil.RemoveFile(vm.Entity as ErlangEditor.Core.Entity.FileEntity);
                        (rtvSolution.SelectedContainer.ParentItem.Item as ViewModel.PrjTreeItemVM).Children.Remove(vm);
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        if (System.IO.Path.GetExtension((vm.Entity as ErlangEditor.Core.Entity.FileEntity).Name).ToLower() == ".erl")
                            App.MainViewModel.AutoCompleteCache.DropModule((vm.Entity as ErlangEditor.Core.Entity.FileEntity).DisplayName);
                    }
                    catch (Exception ecp)
                    {
                        App.Navigation.ShowMessageBox(ecp.Message, "出错");
                    }
                }
            }
        }
    }
}
