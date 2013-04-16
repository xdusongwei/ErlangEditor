using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ErlangEditor.Core.Entity
{
    public class FileEntity
    {
        public FileEntity()
        {
            Children = new List<FileEntity>();
            ID = Guid.NewGuid();
        }

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

        [JsonIgnore]
        public bool Modified
        {
            get;
            set;
        }

        public bool IsFolder
        {
            get;
            set;
        }

        public List<FileEntity> Children
        {
            get;
            set;
        }

        [JsonIgnore]
        public object Parent
        {
            get;
            set;
        }
    }
}
