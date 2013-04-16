using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core;
using ErlangEditor.Template;

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

        public void CreateSolution(
            string aName,
            string aPath,
            string aCompilerPath,
            string aShellPath
            )
        {
            var macro = new StdProcessTemplate(aName, true, string.Empty, string.Empty).Macro;
            CurrentSolution = new SolutionVM(Solution.CreateSolution(aName, aPath, aCompilerPath, aShellPath, macro, "Template\\module.erl"));
        }

        public void SaveSolution()
        {
            if(CurrentSolution != null)
                Solution.SaveSolution(CurrentSolution.Entity);
        }

        public void LoadSolution(string aPath)
        {
            CurrentSolution = new SolutionVM(Solution.LoadSolution(aPath));
        }

        public void CloseSolution()
        {
            CurrentSolution = null;
        }

        public SolutionVM CurrentSolution
        {
            get;
            set;
        }

        private Solution Solution
        {
            get;
            set;
        }
    }
}
