using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class ApplicationEntity
    {
        public ApplicationEntity()
        {
            Name = string.Empty;
            Folders = new List<FolderEntity>();
            Files = new List<FileEntity>();
            StartupMoudle = StartupFunction = StartupArgs = string.Empty;
        }

        public string Name
        {
            get;
            set;
        }

        public List<FolderEntity> Folders
        {
            get;
            set;
        }

        public List<FileEntity> Files
        {
            get;
            set;
        }

        public string StartupMoudle
        {
            get;
            set;
        }

        public string StartupFunction
        {
            get;
            set;
        }

        public string StartupArgs
        {
            get;
            set;
        }

        public bool AutoConfigAppFile
        {
            get;
            set;
        }
    }
}
