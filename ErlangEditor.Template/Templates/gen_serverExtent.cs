using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template.Templates
{
    public partial class gen_server
    {
        public gen_server()
            : this("no_nanme")
        {

        }

        public gen_server(string aModuleName)
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
