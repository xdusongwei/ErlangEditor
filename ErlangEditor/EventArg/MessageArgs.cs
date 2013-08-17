using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.EventArg
{
    public class MessageArgs : EventArgs
    {
        public string Message
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }
    }
}
