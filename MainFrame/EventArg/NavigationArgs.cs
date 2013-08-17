using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainFrame.EventArg
{
    public class NavigationArgs : EventArgs
    {
        public Type UserControlType
        {
            get;
            set;
        }
    }
}
