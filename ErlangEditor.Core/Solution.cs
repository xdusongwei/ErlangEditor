using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.Core
{
    public class Solution
    {
        public SolutionEntity CreateSolution(string aName, string aPath, string aCompilerPath, string aShellPath)
        {
            var sln = new SolutionEntity { Name = aName, ShellPath = aShellPath, CompilerPath = aCompilerPath, StartupProjectName = string.Empty };
            var prj = new ProjectEntity { Name = aName, ProjectPath = aName };
            var file = new FileEntity { Name = aName + ".erl", Path = aName + ".erl" };
            sln.StartupProjectName = prj.Name;
            prj.StartupFile = file.ID;
            SaveSolution(sln);
            return sln;
        }

        public void SaveSolution(SolutionEntity aEntity)
        {

            foreach(var i in aEntity.Children )
                foreach (var j in i.Children)
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
