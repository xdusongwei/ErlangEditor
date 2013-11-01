using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class ConfigEntity
    {
        public ConfigEntity()
        {
            CompilerPath = @"";
            ShellPath = @"";
            ConsolePath = @"";
            RecentProject = new List<RecentProjectEntity>();
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

        public string ConsolePath
        {
            get;
            set;
        }

        public string AutoCompleteCache
        {
            get;
            set;
        }

        public double FontSize
        {
            get;
            set;
        }

        public string FontName
        {
            get;
            set;
        }

        public List<RecentProjectEntity> RecentProject
        {
            get;
            set;
        }
    }
}
