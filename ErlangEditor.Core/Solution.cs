using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ErlangEditor.Core.Entity;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ErlangEditor.Core
{
    public class Solution
    {
        public SolutionEntity CreateSolution(string aName, string aPath, string aCompilerPath, string aShellPath)
        {
            var sln = new SolutionEntity
            {
                Name = aName,
                ShellPath = aShellPath,
                CompilerPath = aCompilerPath,
                StartupProjectName = string.Empty,
                SolutionPath = aPath + "\\"
            };
            var prj = new ProjectEntity
            {
                Name = aName,
                ProjectPath = aName
            };
            var file = new FileEntity
            {
                Name = aName + ".erl",
                Path = aName + ".erl"
            };
            sln.StartupProjectName = prj.Name;
            sln.Children.Add(prj);
            prj.StartupFile = file.ID;
            prj.Children.Add(file);
            SaveSolution(sln);
            file.Modified = true;
            dictCode_.Add(file, new CodeEntity { Content = string.Empty, Key = file });
            return sln;
        }

        public void SaveSolution(SolutionEntity aEntity)
        {
            using (StreamWriter ws = new StreamWriter(aEntity.SolutionPath + aEntity.Name + ".sln"))
            {
                var strData = JsonConvert.SerializeObject(aEntity);
                ws.Write(strData);
                ws.Flush();
            }
            foreach (var i in aEntity.Children)
            {
                if (!Directory.Exists(aEntity.SolutionPath + i.ProjectPath))
                {
                    Directory.CreateDirectory(aEntity.SolutionPath + i.ProjectPath);
                }
            }
            foreach (var i in dictCode_)
            {
                if (i.Key.Modified)
                {
                    i.Key.Modified = false;
                }
            }
            //foreach (var i in aEntity.Children)
            //{
            //    string prjPath = aEntity.SolutionPath + i.ProjectPath;
            //    foreach (var j in i.Children)
            //    {
            //        if (j.Modified)
            //        {
            //            j.Modified = false;
            //            //SaveFile(j);
            //        }
            //    }
            //}
        }

        public void SaveFile(SolutionEntity aSln, FileEntity aFile , string aPath)
        {
            Debug.WriteLine("Save file path{0}{1}", aPath, aFile.Name);
        }

        private Dictionary<FileEntity, CodeEntity> dictCode_ = new Dictionary<FileEntity, CodeEntity>();
    }
}
