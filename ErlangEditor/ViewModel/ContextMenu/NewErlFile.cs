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
                    var name = string.Format("{0}{1}.erl", result.IsModule ? "m" : "s", result.Name);
                    var entity = new FileEntity
                    {
                        Name = name,
                        Compilable = true,
                        IsFolder = false,
                        Modified = true,
                        Path = name,
                        Parent = (App.ViewModel.SelectVMItem as dynamic).Entity
                    };
                    if (!IsValidFileName(result.Name))
                        throw new Exception("文件名不合法");
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
                    var macro = result.IsModule ?
                        new StdProcessTemplate(result.Name, result.ExportAll, string.Format("-import({0}).", result.Import), "-export([start/0]).", result.IsModule).Macro :
                        new StdCodeTemplate(result.Name.ToLower()).Macro;
                    App.ViewModel.Solution.CreateCodeFile(entity, macro, result.IsModule ? TemplateConstant.StdModuleTemplateFilePath : TemplateConstant.StdCodeTemplateFilePath);
                    (App.ViewModel.SelectVMItem as dynamic).Entity.Children.Add(entity);
                    (App.ViewModel.SelectVMItem as dynamic).Children.Add(vm);
                    SortChildItem();
                    App.ViewModel.SelectItem.ExpandAll();
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


        private static bool IsValidFileName(string fileName)
        {
            bool isValid = true;
            string errChar = "\\/:*?\"<>|"; 
            if (string.IsNullOrEmpty(fileName))
            {
                isValid = false;
            }
            else
            {
                for (int i = 0; i < errChar.Length; i++)
                {
                    if (fileName.Contains(errChar[i].ToString()))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }
    }
}
