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
        public SolutionEntity CreateSolution(
            string aName, 
            string aPath, 
            string aCompilerPath, 
            string aShellPath,
            Dictionary<string, string> aMacro,
            string aTemplatePath
            )
        {
            var sln = new SolutionEntity
            {
                Name = aName,
                ShellPath = aShellPath,
                CompilerPath = aCompilerPath,
                StartupProjectName = string.Empty,
                SolutionPath = aPath + "\\" + aName + "\\"
            };
            var prj = new ProjectEntity
            {
                Name = aName,
                ProjectPath = aName + "\\"
            };
            var file = new FileEntity
            {
                Name = aName + EditorConstant.ErlangCodeFileExtentFileType,
                Path = aName + EditorConstant.ErlangCodeFileExtentFileType
            };
            CreateCodeFile(file, aMacro, aTemplatePath);
            sln.StartupProjectName = prj.Name;
            sln.Children.Add(prj);
            prj.StartupFile = file.ID;
            prj.Children.Add(file);
            UpdateSolution(sln);
            SaveSolution(sln);
            return sln;
        }

        public void CreateCodeFile(FileEntity aEntity, Dictionary<string, string> aMacro , string aTemplatePath)
        {
            if (File.Exists(GetFullPath(aEntity)))
                throw new Exception("文件已经存在");
            using (StreamReader sr = new StreamReader(aTemplatePath))
            {
                string codeTemplate = sr.ReadToEnd();
                foreach (var i in aMacro)
                {
                    codeTemplate = codeTemplate.Replace(i.Key, i.Value);
                }
                if (dictCode_.ContainsKey(aEntity))
                    dictCode_.Remove(aEntity);
                aEntity.Modified = true;
                dictCode_.Add(aEntity, new CodeEntity { Content = codeTemplate.Trim() });
            }
        }

        public SolutionEntity LoadSolution(string aPath)
        {
            using (StreamReader sr = new StreamReader(aPath))
            {
                var strSln = sr.ReadToEnd();
                var slnEntity = JsonConvert.DeserializeObject<SolutionEntity>(strSln);
                dictCode_.Clear();
                UpdateSolution(slnEntity);
                int lastChar = aPath.LastIndexOf('\\');
                slnEntity.SolutionPath = aPath.Substring(0, lastChar + 1);
                return slnEntity;
            }
        }

        public void SaveSolution(SolutionEntity aEntity)
        {
            if (!Directory.Exists(aEntity.SolutionPath))
                Directory.CreateDirectory(aEntity.SolutionPath);
            SaveSolutionFile(aEntity);
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
                    SaveFile(i.Key);
                }
            }
        }

        public void SaveSolutionFile(SolutionEntity aEntity)
        {
            using (StreamWriter ws = new StreamWriter(aEntity.SolutionPath + aEntity.Name + ".sln"))
            {
                var strData = JsonConvert.SerializeObject(aEntity);
                ws.Write(strData);
                ws.Flush();
            }
        }

        public void SaveFile(FileEntity aFile)
        {
            var path = GetFullPath(aFile);
            aFile.Modified = false;
            Debug.WriteLine("Save file path " + path);
            using (StreamWriter sw = new StreamWriter(path))
            {
                if(dictCode_.ContainsKey(aFile))
                    sw.Write(dictCode_[aFile].Content);
            }
        }

        public string GetFullPath(object aNode)
        {
            return GetFullPath(string.Empty, aNode);
        }

        private string GetFullPath(string aPath, object aNode)
        {
            dynamic node = aNode;
            if (node is SolutionEntity)
            {
                return node.SolutionPath + aPath;
            }
            else if(node is ProjectEntity)
            {
                return GetFullPath(node.ProjectPath + aPath, node.Parent);
            }
            else if(node is FileEntity)
            {
                return GetFullPath(node.Path + aPath, node.Parent);
            }
            return string.Empty;
        }

        private void UpdateSolution(object aEntity)
        {
            dynamic node = aEntity;
            foreach (dynamic i in node.Children)
            {
                i.Parent = node;
                UpdateSolution(i);
            }
        }

        private Dictionary<FileEntity, CodeEntity> dictCode_ = new Dictionary<FileEntity, CodeEntity>();
    }
}
