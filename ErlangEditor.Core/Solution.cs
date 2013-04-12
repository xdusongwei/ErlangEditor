using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.Core
{
    public class Solution
    {
        public static SolutionEntity Create(string aName, string aPath, string aCompilerPath, string aShellPath)
        {
            return new SolutionEntity { Name = aName, ShellPath = aShellPath, CompilerPath = aCompilerPath, StartupProjectName = string.Empty };
        }

        public void SaveSolution(SolutionEntity aEntity)
        {
            foreach(var i in aEntity.Projects )
                foreach (var j in i.Files)
                {
                    if (j.Modified)
                    {
                        j.Modified = false;
                        //SaveFile(j);
                    }
                }
        }

        public void SaveFile(SolutionEntity aSln, FileEntity aFile , string aCode)
        {

        }

        private Dictionary<FileEntity, CodeEntity> dictCode_ = new Dictionary<FileEntity, CodeEntity>();
    }
}
