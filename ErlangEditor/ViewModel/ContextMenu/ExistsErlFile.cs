using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using Microsoft.Win32;
using System.IO;
using ErlangEditor.Core.Entity;
using System.Windows;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class ExistsErlFile
    {
        public static void Click(object sender, RadRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "Erlang代码文件(*.erl)|*.erl";
            if (fileDialog.ShowDialog() == true)
            {
                for (var i = 0 ; i < fileDialog.FileNames.Length ; i++)
                {
                    try
                    {
                        var folderPath = App.ViewModel.GetVMFilePath(App.ViewModel.SelectVMItem);
                        var targetPath = Path.Combine(new string[] { folderPath, fileDialog.SafeFileNames[i] });
                        var sourcePath = fileDialog.FileNames[i];
                        if (sourcePath.ToLower() != targetPath.ToLower())
                        {
                            if (File.Exists(targetPath))
                            {
                                throw new Exception("项目目录中已经存在该文件");
                            }
                            else
                            {
                                File.Copy(sourcePath, targetPath);
                            }
                        }
                        var parentVM = App.ViewModel.SelectVMItem as dynamic;
                        var entity = new FileEntity
                        {
                            Name = fileDialog.SafeFileNames[i],
                            Compilable = true,
                            Modified = false,
                            Parent = parentVM.Entity
                        };
                        var vm = new ItemVM(entity);
                        parentVM.Entity.Children.Add(entity);
                        parentVM.Children.Add(vm);
                        SortChildItem();
                        App.ViewModel.SaveSolutionFile();
                    }
                    catch (Exception ep)
                    {
                        MessageBox.Show(ep.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
