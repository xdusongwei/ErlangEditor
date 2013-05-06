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
using ErlangEditor.Windows;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using System.Diagnostics;
using ErlangEditor.ViewModel;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit;

namespace ErlangEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!App.ViewModel.Loaded)
                App.ViewModel.Load();
            DataContext = App.ViewModel;
        }

        private void CreateNewSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            NewSolution ns = new NewSolution();
            ns.ShowDialog();
        }

        private void OpenSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择解决方案";
            fileDialog.Filter = "Erlang solution(*.sln)|*sln";
            if (fileDialog.ShowDialog() == true)
            {
                string file = fileDialog.FileName;
                App.ViewModel.LoadSolution(file);
            }
        }

        private void SaveSolution(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            App.ViewModel.SaveSolution();
            foreach (dynamic i in rpContent.Items)
                i.Tag.Entity.Modified = i.Tag.Entity.Modified;
        }

        private void ExitApplication(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void RadContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            RadTreeViewItem clickedItemContainer = radContextMenu.GetClickedElement<RadTreeViewItem>();
            App.ViewModel.UpdateContextOperationMenu(clickedItemContainer);
            if (clickedItemContainer == null)
                radContextMenu.Visibility = System.Windows.Visibility.Collapsed;
            else
                radContextMenu.Visibility = System.Windows.Visibility.Visible;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if((sender as FrameworkElement).Visibility == System.Windows.Visibility.Visible)
                CommitItemChange(sender);
        }

        private void CommitItemChange(object sender)
        {
            try
            {
                App.ViewModel.CommitItemAddOrUpdate((sender as FrameworkElement).Tag, (sender as TextBox).Text);
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitItemChange(sender);
            }
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
            (sender as TextBox).Focus();
        }

        private void TextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as TextBox).Visibility == System.Windows.Visibility.Visible)
            {
                (sender as TextBox).SelectAll();
                (sender as TextBox).Focus();
            }
        }

        private void RadTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = (sender as RadTreeView).SelectedItem as ItemVM;
            if (vm == null) return;
            if (!vm.IsFolder)
            {
                foreach(dynamic i in rpContent.Items)
                    if (i.Tag.Entity == vm.Entity)
                    {
                        rpContent.SelectedItem = i;
                        return;
                    }
                var openFileVM = App.ViewModel.OpenFile(vm);
                var rp = new RadPane() { Tag = openFileVM };
                rp.SetBinding(RadPane.TitleProperty, new Binding("BarText") { Source = openFileVM ,Mode= BindingMode.OneWay});
                var editor = new TextEditor();
                editor.Tag = openFileVM;
                editor.TextChanged += new EventHandler(editor_TextChanged);
                editor.Text = openFileVM.Code;
                rp.Content = editor;
                rpContent.Items.Add(rp);
            }
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            var vm = (sender as TextEditor).Tag as OpenedFileVM;
            vm.Modified = (sender as TextEditor).IsModified;
        }

        private void Make(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            foreach (var i in App.ViewModel.OpenedFiles)
                if (i.Modified)
                    foreach (dynamic j in rpContent.Items)
                        if (j.Content.Tag == i)
                        {
                            i.Code = j.Content.Text;
                            break;
                        }
            App.ViewModel.MakeSolution();
        }
    }
}
