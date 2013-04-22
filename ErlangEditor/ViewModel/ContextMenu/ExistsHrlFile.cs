using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows;
using Microsoft.Win32;

namespace ErlangEditor.ViewModel.ContextMenu
{
    public static class ExistsHrlFile
    {
        public static void Click(object sender, RadRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "Erlang头文件(*.hrl)|*.hrl";
            if (fileDialog.ShowDialog() == true)
            {
                foreach (var i in fileDialog.FileNames)
                {

                }
            }
        }
    }
}
