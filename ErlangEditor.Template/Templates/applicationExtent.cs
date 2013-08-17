using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template.Templates
{
    public partial class application
    {
        public application()
            :this("no_name" , "NoMFA")
        {

        }

        public application(string aModuleName, string aStartupMFA)
        {
            StartupMFA = aStartupMFA;
            ModuleName = aModuleName;
        }

        public string StartupMFA
        {
            get;
            set;
        }

        public string ModuleName
        {
            get;
            set;
        }
    }
}
