using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template.Templates
{
    public partial class supervisor
    {
        public supervisor(string aModuleName, string aRS , string aMax , string aWithin , string aID , string aStartMFA,string aRestart, string aShutdown,
            string aNodeType, string aMods)
        {
            ModuleName = aModuleName.Trim();
            RS = aRS.Trim();
            Max = aMax.Trim();
            Within = aWithin.Trim();
            ID = aID.Trim();
            StartupMFA = aStartMFA.Trim();
            Restart = aRestart.Trim();
            Shutdown = aShutdown.Trim();
            NodeType = aNodeType.Trim();
            Mods = aMods.Trim();
        }

        public string ModuleName
        {
            get;
            set;
        }

        public string RS
        {
            get;
            set;
        }

        public string Max
        {
            get;
            set;
        }

        public string Within
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public string StartupMFA
        {
            get;
            set;
        }

        public string Restart
        {
            get;
            set;
        }

        public string Shutdown
        {
            get;
            set;
        }

        public string NodeType
        {
            get;
            set;
        }

        public string Mods
        {
            get;
            set;
        }
    }
}
