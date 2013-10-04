using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.AutoComplete
{
    public class AcEntity
    {
        public string ModuleName
        {
            get;
            set;
        }

        public string FunctionName
        {
            get;
            set;
        }

        public string Arity
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }
    }
}
