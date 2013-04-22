using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using ErlangEditor.Core.Entity;
using System.IO;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class RenameFile
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
            App.ViewModel.ContextOperation = MainViewModel.ContextOperationTypeEnum.Rename;
            if (App.ViewModel.SelectVMItem is ItemVM)
            {
                var itm = App.ViewModel.SelectVMItem as ItemVM;
                itm.TextBlockVisibility = System.Windows.Visibility.Collapsed;
                itm.TextBoxVisibility = System.Windows.Visibility.Visible;
            }
        }

        public static void Commit(object aVM, string aNewFileName)
        {
            if (VM == null) return;
            if (aVM is ItemVM) 
            {
                var itm = aVM as ItemVM;
                if (itm.Entity.Parent is ProjectEntity)
                {
                    if ((itm.Entity.Parent as ProjectEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFileName))
                    {
                        itm.Name = Name;
                        VM = null;
                        throw new Exception("文件已经存在");
                    }
                }
                else if (itm.Entity.Parent is FileEntity)
                {
                    if ((itm.Entity.Parent as FileEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFileName))
                    {
                        itm.Name = Name;
                        VM = null;
                        throw new Exception("文件已经存在");
                    }
                }

                itm.TextBoxVisibility = System.Windows.Visibility.Collapsed;
                itm.TextBlockVisibility = System.Windows.Visibility.Visible;
                var oldPath = App.ViewModel.GetVMFilePath(itm);
                itm.Name = aNewFileName;
                itm.Entity.Path = aNewFileName;
                var dirPath = App.ViewModel.GetVMFilePath(itm);
                try
                {
                    if (!File.Exists(dirPath))
                    {
                        File.Move(oldPath, dirPath);
                    }
                    else
                    {
                        throw new Exception("文件已经存在");
                    }
                    App.ViewModel.Solution.SaveFile(itm.Entity);
                }
                catch (Exception ep)
                {
                    itm.Name = Name;
                    itm.Entity.Path = Name;
                    App.ViewModel.Solution.SaveFile(itm.Entity);
                    VM = null;
                    throw ep;
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
