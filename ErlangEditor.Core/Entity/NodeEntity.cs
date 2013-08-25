using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class NodeEntity
    {
        public string NodeName
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get;
            set;
        }

        public bool ShowShell
        {
            get;
            set;
        }
    }
}
