using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class FileEntity
    {
        public string Name
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public Guid ID
        {
            get;
            set;
        }

        public bool Compilable
        {
            get;
            set;
        }

        public bool Modified
        {
            get;
            set;
        }
    }
}
