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
            CompilerPath = @"E:\erl5.10.1\bin\erlc.exe";
            ShellPath = @"E:\erl5.10.1\bin\werl.exe";
            ConsolePath = @"E:\erl5.10.1\bin\erl.exe";
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
