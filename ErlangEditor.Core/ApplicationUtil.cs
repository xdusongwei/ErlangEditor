﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core
{
    public static class ApplicationUtil
    {
        public static void AddApplication(Entity.ApplicationEntity aApplication)
        {
            if (SolutionUtil.Solution == null) return;
            var app = aApplication;
            if (System.IO.Directory.Exists(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name)))
            {
                throw new Exception("文件目录中已经存在该应用的目录，不能添加。");
            }
            if (aApplication.Name.ToLower() == ErlangEditor.Core.SolutionUtil.Solution.Name.ToLower())
            {
                throw new Exception("因为发布系统的原因，不能创建同项目相同名称的应用。");
            }
            if (app != null)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath , app.Name));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name, "ebin"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name, "src"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name, "include"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name, "doc"));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, app.Name, "priv"));
                SolutionUtil.Solution.Apps.Add(app);
                app.Folders.Add(new Entity.FolderEntity { Name = "ebin" });
                app.Folders.Add(new Entity.FolderEntity { Name = "src" });
                app.Folders.Add(new Entity.FolderEntity { Name = "include" });
                app.Folders.Add(new Entity.FolderEntity { Name = "doc" });
                app.Folders.Add(new Entity.FolderEntity { Name = "priv" });
                Helper.EntityTreeUtil.UpdateDict();
            }
        }

        public static void SeparateApplication(string aAppName)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Apps.Any(x => x.Name == aAppName))
            {
                SolutionUtil.Solution.Apps.Remove(SolutionUtil.Solution.Apps.First(x => x.Name == aAppName));
                Helper.EntityTreeUtil.UpdateDict();
            }
        }

        public static void DeleteApplication(string aAppName)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Apps.Any(x => x.Name == aAppName))
            {
                try
                {
                    System.IO.Directory.Delete(System.IO.Path.Combine(Helper.EntityTreeUtil.GetBasePath, aAppName), true);
                }
                catch { }
                SolutionUtil.Solution.Apps.Remove(SolutionUtil.Solution.Apps.First(x => x.Name == aAppName));
                Helper.EntityTreeUtil.UpdateDict();
            }
        }
    }
}

