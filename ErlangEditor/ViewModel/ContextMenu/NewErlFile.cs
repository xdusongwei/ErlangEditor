using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using ErlangEditor.Core.Entity;
using ErlangEditor.Template;
using ErlangEditor.Windows;
using System.Windows;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class NewErlFile
    {
        //private static object VM
        //{
        //    get;
        //    set;
        //}

        public static void Click(object sender, RadRoutedEventArgs e)
        {
            try
            {
                var dlg = new NewCodeFile();
                if (dlg.ShowDialog() == true)
                {
                    dynamic result = dlg.DataContext;
                    var entity = new FileEntity
                    {
                        Name = result.Name,
                        Compilable = true,
                        IsFolder = false,
                        Modified = true,
                        Path = result.Name,
                        Parent = (App.ViewModel.SelectVMItem as dynamic).Entity
                    };
                    object parentVM = App.ViewModel.SelectVMItem;
                    if (parentVM is ProjectVM)
                    {
                        if ((parentVM as ProjectVM).Children.Any(x => x.Name == result.Name))
                            throw new Exception("该项已经存在");
                    }
                    else if (parentVM is ItemVM)
                    {
                        if ((parentVM as ItemVM).Children.Any(x => x.Name == result.Name))
                            throw new Exception("该项已经存在");
                    }
                    var vm = new ItemVM(entity);
                    var path = App.ViewModel.GetVMFilePath(vm);
                    var macro = new StdProcessTemplate(entity.Name, result.ExportAll, string.Format("-import({0}).", result.Import), "-export([start/0]).", result.IsModule).Macro;
                    App.ViewModel.Solution.CreateCodeFile(entity, macro, TemplateConstant.StdModuleTemplateFilePath);
                    (App.ViewModel.SelectVMItem as dynamic).Entity.Children.Add(entity);
                    (App.ViewModel.SelectVMItem as dynamic).Children.Add(vm);
                    SortChildItem();
                    App.ViewModel.SaveSolutionFile();
                }
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //VM = App.ViewModel.SelectVMItem;
            //App.ViewModel.ContextOperation = MainViewModel.ContextOperationTypeEnum.Add;
            //if (App.ViewModel.SelectVMItem is ProjectVM)
            //{
            //    var prj = App.ViewModel.SelectVMItem as ProjectVM;
            //    var newEntity = new FileEntity { Compilable = true, Parent = prj.Entity, Name = "NewCodeFile.erl" };
            //    prj.Entity.Children.Add(newEntity);
            //    prj.Children.Insert(0, new ItemVM(newEntity) { TextBlockVisibility = System.Windows.Visibility.Hidden, TextBoxVisibility = System.Windows.Visibility.Visible });
            //    App.ViewModel.SelectItem.ExpandAll();
            //}
            //else if (App.ViewModel.SelectVMItem is ItemVM)
            //{
            //    var itm = App.ViewModel.SelectVMItem as ItemVM;
            //    var newEntity = new FileEntity { Compilable = true, Parent = itm.Entity, Name = "NewCodeFile.erl" };
            //    itm.Entity.Children.Add(newEntity);
            //    itm.Children.Insert(0, new ItemVM(newEntity) { TextBlockVisibility = System.Windows.Visibility.Hidden, TextBoxVisibility = System.Windows.Visibility.Visible });
            //    App.ViewModel.SelectItem.ExpandAll();
            //}
        }

        public static void Commit(object aVM, string aNewFileName)
        {
            //if (VM == null) return;
            //if (aVM is ItemVM)
            //{
            //    var itm = aVM as ItemVM;
            //    if (itm.IsFolder)
            //    {
            //        if (itm.Entity.Parent is ProjectEntity)
            //        {
            //            if ((itm.Entity.Parent as ProjectEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => !x.IsFolder && x.Name == aNewFileName))
            //            {
            //                if (VM is ProjectVM)
            //                {
            //                    (itm.Entity.Parent as ProjectEntity).Children.Remove(itm.Entity);
            //                    (VM as ProjectVM).Children.Remove(itm);
            //                }
            //                else if (VM is ItemVM)
            //                {
            //                    (itm.Entity.Parent as FileEntity).Children.Remove(itm.Entity);
            //                    (VM as ItemVM).Children.Remove(itm);
            //                }
            //                VM = null;
            //                throw new Exception("文件已经存在");
            //            }
            //        }
            //        else if (itm.Entity.Parent is FileEntity)
            //        {
            //            if ((itm.Entity.Parent as FileEntity).Children.Except(new FileEntity[] { itm.Entity }).Any(x => !x.IsFolder && x.Name == aNewFileName))
            //            {
            //                if (VM is ProjectVM)
            //                {
            //                    (itm.Entity.Parent as ProjectEntity).Children.Remove(itm.Entity);
            //                    (VM as ProjectVM).Children.Remove(itm);
            //                }
            //                else if (VM is ItemVM)
            //                {
            //                    (itm.Entity.Parent as FileEntity).Children.Remove(itm.Entity);
            //                    (VM as ItemVM).Children.Remove(itm);
            //                }
            //                VM = null;
            //                throw new Exception("文件已经存在");
            //            }
            //        }
            //        itm.TextBoxVisibility = System.Windows.Visibility.Collapsed;
            //        itm.TextBlockVisibility = System.Windows.Visibility.Visible;
            //        itm.Name = aNewFileName;
            //        itm.Entity.Path = aNewFileName;
            //        var dirPath = App.ViewModel.GetVMFilePath(itm);
            //        using (var sw = File.CreateText(dirPath))
            //        {
            //            var macro = new StdProcessTemplate(aName, true, string.Empty, string.Empty).Macro;
            //            App.ViewModel.Solution.CreateCodeFile(itm.Entity, macro, "Template\\module.erl");
            //        }
            //        App.ViewModel.SaveSolutionFile();
            //        SortChildItem();
            //        VM = null;
            //    }
            //}
        }

        private static void SortChildItem()
        {
            dynamic parent = App.ViewModel.SelectVMItem;
            var sorted = new List<ItemVM>(parent.Children);
            sorted.Sort();
            parent.Children.Clear();
            foreach (var i in sorted)
                parent.Children.Add(i);
        }
    }
}
