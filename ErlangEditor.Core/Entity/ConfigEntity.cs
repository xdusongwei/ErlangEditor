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
    }
}
