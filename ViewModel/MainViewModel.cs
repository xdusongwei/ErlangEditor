using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core;

namespace ErlangEditor.ViewModel
{
    public class MainViewModel
    {
        public bool Loaded
        {
            get;
            private set;
        }

        public void Load()
        {
            Solution = new Solution();
            Loaded = true;
        }

        public void CreateSolution(string aName, string aCompilerPath, string aShellPath, string aBasePath)
        {

        }

        public Solution Solution
        {
            get;
            set;
        }
    }
}
