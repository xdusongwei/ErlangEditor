using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.CompilerProxy
{
    public class CodeFileErrorEventArgs :EventArgs
    {
        public string Log
        {
            get;
            set;
        }

        public FileEntity Entity
        {
            get;
            set;
        }

        public CodeFileErrorEventArgs(string aLog, FileEntity aEntity)
        {
            Log = aLog;
            Entity = aEntity;
        }
    }
}
