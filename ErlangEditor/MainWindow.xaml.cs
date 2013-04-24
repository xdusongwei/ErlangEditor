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
        }

        private void ExitApplication(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

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
                App.ViewModel.OpenFile(vm);
                var rp = new RadPane();
                rp.SetBinding(RadPane.HeaderProperty, new Binding("Name") { Source = vm });
                var editor = new TextEditor();
                rp.Content = editor;
                rpContent.Items.Add(rp);
            }
        }
    }
}
