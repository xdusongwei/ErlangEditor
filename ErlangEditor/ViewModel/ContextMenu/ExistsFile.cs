using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Telerik.Windows;
using System.Windows;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class ExistsFile
    {
        public static void Click(object sender, RadRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                foreach (var i in fileDialog.FileNames)
                {

                }
            }
        }
    }
}
