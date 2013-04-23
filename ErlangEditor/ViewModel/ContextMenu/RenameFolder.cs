using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using ErlangEditor.Core.Entity;
using System.IO;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class RenameFolder
    {
        private static object VM
        {
            get;
            set;
        }

        private static string Name
        {
            get;
            set;
        }

        public static void Click(object sender, RadRoutedEventArgs e)
        {
            VM = App.ViewModel.SelectVMItem;
            Name = (App.ViewModel.SelectVMItem as dynamic).Name;
            if (App.ViewModel.SelectVMItem is ItemVM)
            {
                var itm = App.ViewModel.SelectVMItem as ItemVM;
                itm.TextBlockVisibility = System.Windows.Visibility.Collapsed;
                itm.TextBoxVisibility = System.Windows.Visibility.Visible;
            }
            App.ViewModel.CommitItemNameAction = new Action<object, string>(Commit);
        }

        public static void Commit(object aVM, string aNewFolderName)
        {
            if (VM == null) return;
            if (aVM is ItemVM)
            {
                var itm = aVM as ItemVM;
                itm.TextBoxVisibility = System.Windows.Visibility.Collapsed;
                itm.TextBlockVisibility = System.Windows.Visibility.Visible;
                if (itm.Name == aNewFolderName) return;
                if (itm.Entity.Parent is ProjectEntity)
                {
                    if ((itm.Entity.Parent as ProjectEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFolderName))
                    {
                        itm.Name = Name;
                        VM = null;
                        throw new Exception("文件夹已经存在");
                    }
                }
                else if (itm.Entity.Parent is FileEntity)
                {
                    if ((itm.Entity.Parent as FileEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFolderName))
                    {
                        itm.Name = Name;
                        VM = null;
                        throw new Exception("文件夹已经存在");
                    }
                }
                
                
                var oldPath = App.ViewModel.GetVMFilePath(itm);
                itm.Name = aNewFolderName;
                itm.Entity.Path = aNewFolderName + "\\";
                var dirPath = App.ViewModel.GetVMFilePath(itm);
                if (!Directory.Exists(dirPath))
                {
                    Directory.Move(oldPath , dirPath);
                }
                App.ViewModel.SaveSolutionFile();
                SortChildItem();
                VM = null;
            }
        }

        private static void SortChildItem()
        {
            dynamic parent = App.ViewModel.SelectItem.ParentItem.Item;
            var sorted = new List<ItemVM>(parent.Children);
            sorted.Sort();
            parent.Children.Clear();
            foreach (var i in sorted)
                parent.Children.Add(i);
        }
    }
}
