using Newtonsoft.Json;
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
            NodeNames = new List<string>();
            StartupMFA = string.Empty;
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

        public bool AppMode
        {
            get;
            set;
        }

        public bool CodeMode
        {
            get;
            set;
        }

        public bool StartupAsMFA
        {
            get;
            set;
        }

        public string StartupMFA
        {
            get;
            set;
        }

        public bool NoStartup
        {
            get;
            set;
        }

        public List<string> NodeNames
        {
            get;
            set;
        }
    }
}
