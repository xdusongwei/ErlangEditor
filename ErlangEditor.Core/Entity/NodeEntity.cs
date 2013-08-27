using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class NodeEntity
    {
        public NodeEntity()
        {
            Apps = new List<string>();
        }

        public string NodeName
        {
            get;
            set;
        }

        public bool ShowShell
        {
            get;
            set;
        }

        public List<string> Apps
        {
            get;
            set;
        }

        [Newtonsoft.Json.JsonIgnore]
        public bool IsRunning
        {
            get;
            set;
        }
    }
}
