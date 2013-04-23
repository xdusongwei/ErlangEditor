using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using System.Diagnostics;
using Telerik.Windows.Controls;
using ErlangEditor.Core.Entity;
using System.Collections.ObjectModel;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class NewFolder
    {
        private static object VM
        {
            get;
            set;
        }

        public static void Click(object sender, RadRoutedEventArgs e)
        {
            VM = App.ViewModel.SelectVMItem;
            if (App.ViewModel.SelectVMItem is ProjectVM)
            {
                var prj = App.ViewModel.SelectVMItem as ProjectVM;
                var newEntity = new FileEntity { IsFolder = true, Parent = prj.Entity, Name = "NewFolder" };
                prj.Entity.Children.Add(newEntity);
                prj.Children.Insert(0, new ItemVM(newEntity) { TextBlockVisibility = System.Windows.Visibility.Hidden, TextBoxVisibility = System.Windows.Visibility.Visible });
                App.ViewModel.SelectItem.ExpandAll();
            }
            else if (App.ViewModel.SelectVMItem is ItemVM)
            {
                var itm = App.ViewModel.SelectVMItem as ItemVM;
                var newEntity = new FileEntity { IsFolder = true, Parent = itm.Entity, Name = "NewFolder" };
                itm.Entity.Children.Add(newEntity);
                itm.Children.Insert(0, new ItemVM(newEntity) { TextBlockVisibility = System.Windows.Visibility.Hidden, TextBoxVisibility = System.Windows.Visibility.Visible });
                App.ViewModel.SelectItem.ExpandAll();
            }
            App.ViewModel.CommitItemNameAction = new Action<object, string>(Commit);
        }

        public static void Commit(object aVM  ,string aNewFolderName)
        {
            if (VM == null) return;
            if (aVM is ItemVM)
            {
                var itm = aVM as ItemVM;
                if (itm.IsFolder)
                {
                    if (itm.Entity.Parent is ProjectEntity)
                    {
                        if ((itm.Entity.Parent as ProjectEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFolderName))
                        {
                            if (VM is ProjectVM)
                            {
                                (itm.Entity.Parent as ProjectEntity).Children.Remove(itm.Entity);
                                (VM as ProjectVM).Children.Remove(itm);
                            }
                            else if (VM is ItemVM)
                            {
                                (itm.Entity.Parent as FileEntity).Children.Remove(itm.Entity);
                                (VM as ItemVM).Children.Remove(itm);
                            }
                            VM = null;
                            throw new Exception("文件夹已经存在");
                        }
                    }
                    else if (itm.Entity.Parent is FileEntity)
                    {
                        if ((itm.Entity.Parent as FileEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => x.IsFolder && x.Name == aNewFolderName))
                        {
                            if (VM is ProjectVM)
                            {
                                (itm.Entity.Parent as ProjectEntity).Children.Remove(itm.Entity);
                                (VM as ProjectVM).Children.Remove(itm);
                            }
                            else if (VM is ItemVM)
                            {
                                (itm.Entity.Parent as FileEntity).Children.Remove(itm.Entity);
                                (VM as ItemVM).Children.Remove(itm);
                            }
                            VM = null;
                            throw new Exception("文件夹已经存在");
                        }
                    }
                    itm.TextBoxVisibility = System.Windows.Visibility.Collapsed;
                    itm.TextBlockVisibility = System.Windows.Visibility.Visible;
                    itm.Name = aNewFolderName;
                    itm.Entity.Path = aNewFolderName + "\\";
                    var dirPath = App.ViewModel.GetVMFilePath(itm);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    App.ViewModel.SaveSolutionFile();
                    SortChildItem();
                    VM = null;
                }
            }
        }

        private static void SortChildItem()
        {
            dynamic parent = VM;
            var sorted = new List<ItemVM>(parent.Children);
            sorted.Sort();
            parent.Children.Clear();
            foreach (var i in sorted)
                parent.Children.Add(i);
        }
    }
}
