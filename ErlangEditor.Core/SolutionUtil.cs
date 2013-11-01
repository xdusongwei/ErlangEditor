using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core
{
    public static class SolutionUtil
    {
        public static void CreateSolution(string aName , string aBasePath)
        {
            Solution = null;
            var filePath = Path.Combine(aBasePath, aName + "\\" + aName + ".sln");
            if (File.Exists(filePath))
                throw new Exception("文件已经存在");
            var sln = new Entity.SolutionEntity(aName);
            Helper.EntityTreeUtil.GetBasePath = Path.Combine(aBasePath, aName);
            Helper.EntityTreeUtil.UpdateDict();
            Solution = sln;
            SaveSolution();
        }

        public static void LoadSolution(string aSlnPath)
        {
            Solution = null;
            int lastChar = aSlnPath.LastIndexOf('\\');
            using (StreamReader sr = new StreamReader(aSlnPath))
            {
                var strSln = sr.ReadToEnd();
                var sln = JsonConvert.DeserializeObject<Entity.SolutionEntity>(strSln);
                Helper.EntityTreeUtil.GetBasePath = aSlnPath.Substring(0, lastChar);
                Helper.EntityTreeUtil.GetFilePath = aSlnPath;
                Solution = sln;
                Helper.EntityTreeUtil.UpdateDict();
            }
        }

        public static void SaveSolution()
        {
            if (Solution == null) return;
            //ensure solution base folder is exists
            if (!Directory.Exists(Helper.EntityTreeUtil.GetBasePath))
                Directory.CreateDirectory(Helper.EntityTreeUtil.GetBasePath);
            if (!Directory.Exists(Path.Combine(Helper.EntityTreeUtil.GetBasePath, "lib")))
                Directory.CreateDirectory(Path.Combine(Helper.EntityTreeUtil.GetBasePath, "lib"));
            //write .sln file
            using (StreamWriter ws = new StreamWriter(Helper.EntityTreeUtil.GetBasePath + "\\" + Solution.Name + ".sln"))
            {
                var strData = JsonConvert.SerializeObject(Solution);
                ws.Write(strData);
                ws.Flush();
            }
        }

        public static void CloseSolution()
        {
            Solution = null;
            Helper.EntityTreeUtil.GetBasePath = string.Empty;
            Helper.EntityTreeUtil.GetFilePath = string.Empty;
            Helper.EntityTreeUtil.UpdateDict();
        }

        public static Entity.SolutionEntity Solution
        {
            get;
            private set;
        }
    }
}
