using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MainFrame.ViewModel;

namespace MainFrame.EventArg
{
    public class ToolBoxButtonClickEventArgs : RoutedEventArgs
    {
        public ToolBoxButtonClickEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }

        public ToolBoxButtonVM ViewModel
        {
            get;
            set;
        }
    }
}
