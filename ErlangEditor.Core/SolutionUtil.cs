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
            if (File.Exists(Path.Combine(aBasePath, aName + "\\" + aName + ".sln")))
                throw new Exception("文件已经存在");
            var sln = new Entity.SolutionEntity { BasePath = Path.Combine(aBasePath, aName), Name = aName };
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
            //write .sln file
            using (StreamWriter ws = new StreamWriter(Helper.EntityTreeUtil.GetBasePath + "\\" + Solution.Name + ".sln"))
            {
                var strData = JsonConvert.SerializeObject(Solution);
                ws.Write(strData);
                ws.Flush();
            }
            //write any edited files
        }

        public static void CloseSolution()
        {
            Solution = null;
            Helper.EntityTreeUtil.GetBasePath = string.Empty;
            Helper.EntityTreeUtil.UpdateDict();
        }

        public static Entity.SolutionEntity Solution
        {
            get;
            private set;
        }
    }
}
