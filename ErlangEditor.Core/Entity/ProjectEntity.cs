using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class ProjectEntity
    {
        public string Name
        {
            get;
            set;
        }

        public string ProjectPath
        {
            get;
            set;
        }

        public List<string> Folders
        {
            get;
            set;
        }

        public List<FileEntity> Files
        {
            get;
            set;
        }

        public Guid StartupFile
        {
            get;
            set;
        }
    }
}
