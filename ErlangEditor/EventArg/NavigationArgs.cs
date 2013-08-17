using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.EventArg
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
