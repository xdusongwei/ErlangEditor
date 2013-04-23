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
                        Name = result.Name + ".erl" ,
                        Compilable = true,
                        IsFolder = false,
                        Modified = true,
                        Path = result.Name + ".erl",
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
