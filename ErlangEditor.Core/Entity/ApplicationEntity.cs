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

        public bool DefaultStartupMFA
        {
            get;
            set;
        }

        public string StartupMFA
        {
            get;
            set;
        }

        public List<string> NodeNames
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool IsRunning
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
