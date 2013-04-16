using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ErlangEditor.Core.Entity
{
    public class ProjectEntity
    {
        public ProjectEntity()
        {
            Children = new List<FileEntity>();
        }

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

        public List<FileEntity> Children
        {
            get;
            set;
        }

        public Guid StartupFile
        {
            get;
            set;
        }

        [JsonIgnore]
        public SolutionEntity Parent
        {
            get;
            set;
        }
    }
}
