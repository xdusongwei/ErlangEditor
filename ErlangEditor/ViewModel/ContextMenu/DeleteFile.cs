using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using System.Windows;
using Telerik.Windows.Controls;
using Microsoft.VisualBasic.FileIO;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class DeleteFile
    {
        public static void Click(object sender, RadRoutedEventArgs e)
        {
            if (App.ViewModel.SelectVMItem is ItemVM && !(App.ViewModel.SelectVMItem as ItemVM).IsFolder)
            {
                var vm = App.ViewModel.SelectVMItem as ItemVM;
                if (MessageBox.Show(string.Format("确认删除 {0} ?", vm.Name), "删除", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        FileSystem.DeleteFile(App.ViewModel.GetVMFilePath(vm), UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    catch { }
                    dynamic vmParent = (App.ViewModel.SelectItem.ParentItem as RadTreeViewItem).Item;
                    dynamic entityParent = vm.Entity.Parent;
                    entityParent.Children.Remove(vm.Entity);
                    vmParent.Children.Remove(vm);
                    App.ViewModel.SaveSolutionFile();
                }
            }
        }
    }
}
