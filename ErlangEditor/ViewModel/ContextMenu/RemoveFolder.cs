using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using System.Windows;
using Microsoft.VisualBasic.FileIO;
using Telerik.Windows.Controls;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class RemoveFolder
    {
        public static void Click(object sender, RadRoutedEventArgs e)
        {
            if (App.ViewModel.SelectVMItem is ItemVM && (App.ViewModel.SelectVMItem as ItemVM).IsFolder)
            {
                var vm = App.ViewModel.SelectVMItem as ItemVM;
                dynamic vmParent = (App.ViewModel.SelectItem.ParentItem as RadTreeViewItem).Item;
                dynamic entityParent = vm.Entity.Parent;
                entityParent.Children.Remove(vm.Entity);
                vmParent.Children.Remove(vm);
                App.ViewModel.SaveSolutionFile();
            }
        }
    }
}
