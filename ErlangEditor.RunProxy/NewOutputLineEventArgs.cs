using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.RunProxy
{
    public class NewOutputLineEventArgs : EventArgs
    {
        public string NodeName
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }
    }
}
