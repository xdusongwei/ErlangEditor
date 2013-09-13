using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template.Templates
{
    public partial class gen_event : gen_eventBase
    {
        public gen_event()
            : this("no_nanme")
        {

        }

        public gen_event(string aModuleName)
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
