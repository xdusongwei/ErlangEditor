using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using ErlangEditor.ViewModel.ContextMenu;

namespace ErlangEditor.ViewModel.ContextMenuMaker
{
    public static class ProjectContextMenu
    {
        public static void MakeMenu(ObservableCollection<RadMenuItem> aContextMenuContainer)
        {
            aContextMenuContainer.Clear();
            var newItem = new RadMenuItem { Header = "添加新Erlang代码文件" };
            newItem.Click += NewErlFile.Click;
            var newItem2 = new RadMenuItem { Header = "添加新Hrl代码文件" };
            newItem2.Click += NewHrlFile.Click;
            var newItem3 = new RadMenuItem { Header = "添加新的其他文件" };
            newItem3.Click += NewFile.Click;
            var sep = new RadMenuItem { IsSeparator = true };
            var existItem = new RadMenuItem { Header = "添加现有Erlang代码文件" };
            existItem.Click += ExistsErlFile.Click;
            var existItem2 = new RadMenuItem { Header = "添加现有Hrl代码文件" };
            existItem2.Click += ExistsHrlFile.Click;
            var existItem3 = new RadMenuItem { Header = "添加现有文件" };
            existItem3.Click += ExistsFile.Click;
            var sep2 = new RadMenuItem { IsSeparator = true };
            var folderItem = new RadMenuItem { Header = "新建文件夹" };
            folderItem.Click += NewFolder.Click;
            var addChildren = new RadMenuItem[] { newItem, newItem2, newItem3, sep, existItem, existItem2, existItem3, sep2, folderItem };
            aContextMenuContainer.Add(new RadMenuItem { Header = "添加", ItemsSource = new ObservableCollection<RadMenuItem>(addChildren) });
            aContextMenuContainer.Add(new RadMenuItem { Header = "重命名" });
            aContextMenuContainer.Add(new RadMenuItem { Header = "删除" });
        }
    }
}
