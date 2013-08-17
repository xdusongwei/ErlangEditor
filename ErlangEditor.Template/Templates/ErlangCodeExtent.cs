using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template.Templates
{
    public partial class ErlangCode
    {
        public ErlangCode()
            : this("no_nanme")
        {

        }

        public ErlangCode(string aModuleName)
        {
            ModuleName = aModuleName;
        }

        public string ModuleName
        {
            get;
            set;
        }
    }
}
