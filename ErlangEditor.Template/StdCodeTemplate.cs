using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template
{
    public class StdCodeTemplate
    {
        private Dictionary<string, string> macro_ = new Dictionary<string, string>();
        private static readonly string constantFileNameMacro = "%_FILENAME_%";

        public Dictionary<string, string> Macro
        {
            get
            { return macro_; }
        }

        public StdCodeTemplate(string aFileName)
        {
            macro_.Add(constantFileNameMacro, aFileName);
        }
    }
}
