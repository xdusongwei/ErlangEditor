using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.RunProxy
{
    public class PrerunArguments
    {
        public enum RunTypeEnum { Auto, MFA, Shell };
        public RunTypeEnum RunType
        {
            get;
            set;
        }

        public string NodeName
        {
            get;
            set;
        }

        public bool ShowShell
        {
            get;
            set;
        }
    }
}
