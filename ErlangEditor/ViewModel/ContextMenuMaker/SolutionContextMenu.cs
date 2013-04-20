using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace ErlangEditor.ViewModel.ContextMenuMaker
{
    public static class SolutionContextMenu
    {
        public static void MakeMenu(ObservableCollection<RadMenuItem> aContextMenuContainer)
        {
            aContextMenuContainer.Clear();
            aContextMenuContainer.Add(new RadMenuItem { Header = "添加新项目" });
            aContextMenuContainer.Add(new RadMenuItem { Header = "排除" });
        }
    }
}
