using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ErlangEditor.Core.Entity
{
    public class SolutionEntity
    {
        public SolutionEntity()
        {
            Children = new List<ProjectEntity>();
            MakeFolder = string.Empty;
        }

        public string Name
        {
            get;
            set;
        }

        public List<ProjectEntity> Children
        {
            get;
            set;
        }

        public string StartupProjectName
        {
            get;
            set;
        }

        public string CompilerPath
        {
            get;
            set;
        }

        public string ShellPath
        {
            get;
            set;
        }

        public string MakeFolder
        {
            get;
            set;
        }

        [JsonIgnore]
        public string SolutionPath
        {
            get;
            set;
        }
    }
}
