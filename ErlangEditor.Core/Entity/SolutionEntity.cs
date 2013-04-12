using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class SolutionEntity
    {
        public string Name
        {
            get;
            set;
        }

        public List<ProjectEntity> Projects
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
    }
}
