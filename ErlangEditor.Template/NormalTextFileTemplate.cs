using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template
{
    public class NormalTextFileTemplate
    {
        private Dictionary<string, string> macro_ = new Dictionary<string, string>();

        public Dictionary<string, string> Macro
        {
            get
            { return macro_; }
        }
    }
}
