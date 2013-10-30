using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core
{
    public static class FileUtil
    {
        public static void AddFile(string aAppName, string aFolderName, Entity.FileEntity aNewFile)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Apps.Any(x => x.Name == aAppName))
            {
                var app = SolutionUtil.Solution.Apps.First(x => x.Name == aAppName);
                if (string.IsNullOrWhiteSpace(aFolderName) && !app.Files.Any(x => x.Name == aNewFile.Name))
                {
                    app.Files.Add(aNewFile);
                    Helper.EntityTreeUtil.UpdateDict();
                }
                else if (app.Folders.Any(x => x.Name == aFolderName))
                {
                    var fld = app.Folders.First(x => x.Name == aFolderName);
                    if (!fld.Files.Any(x => x.Name == aNewFile.Name))
                    {
                        fld.Files.Add(aNewFile);
                        Helper.EntityTreeUtil.UpdateDict();
                    }
                }
            }
            else
            {
                throw new Exception("文件层次结构未知。");
            }
        }

        public static void AddFile(string aAppName, string aFolderName, Entity.FileEntity aNewFile, string aContent)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Apps.Any(x => x.Name == aAppName))
            {
                var app = SolutionUtil.Solution.Apps.First(x => x.Name == aAppName);
                if (string.IsNullOrWhiteSpace(aFolderName) && !app.Files.Any(x => x.Name == aNewFile.Name))
                {
                    app.Files.Add(aNewFile);
                    Helper.EntityTreeUtil.UpdateDict();
                    var path = Helper.EntityTreeUtil.GetPath(aNewFile);
                    if (System.IO.File.Exists(path))
                    {
                        app.Files.Remove(aNewFile);
                        Helper.EntityTreeUtil.UpdateDict();
                        throw new Exception("文件已经存在");
                    }
                    using (var ws = System.IO.File.CreateText(Helper.EntityTreeUtil.GetPath(aNewFile)))
                        ws.Write(aContent);
                }
                else if (string.IsNullOrWhiteSpace(aFolderName) && app.Files.Any(x => x.Name == aNewFile.Name))
                {
                    throw new Exception("文件已经存在");
                }
                else if (app.Folders.Any(x => x.Name == aFolderName))
                {
                    var fld = app.Folders.First(x => x.Name == aFolderName);
                    if (!fld.Files.Any(x => x.Name == aNewFile.Name))
                    {
                        fld.Files.Add(aNewFile);
                        Helper.EntityTreeUtil.UpdateDict();
                        var path = Helper.EntityTreeUtil.GetPath(aNewFile);
                        if (System.IO.File.Exists(path))
                        {
                            fld.Files.Remove(aNewFile);
                            Helper.EntityTreeUtil.UpdateDict();
                            throw new Exception("文件已经存在");
                        }
                        using (var ws = System.IO.File.CreateText(Helper.EntityTreeUtil.GetPath(aNewFile)))
                            ws.Write(aContent);
                    }
                    else
                    {
                        throw new Exception("文件已经存在");
                    }
                }
            }
            else
            {
                throw new Exception("文件层次结构未知。");
            }
        }

        public static void RemoveFile(Entity.FileEntity aFile)
        {
            if (SolutionUtil.Solution == null) return;
            var parent = Helper.EntityTreeUtil.GetParent(aFile);
            if (parent == null) return;
            if (parent is Entity.ApplicationEntity)
            {
                var path = Helper.EntityTreeUtil.GetPath(aFile);
                var app = parent as Entity.ApplicationEntity;
                app.Files.Remove(aFile);
                Helper.EntityTreeUtil.UpdateDict();
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                catch { }
            }
            if (parent is Entity.FolderEntity)
            {
                var path = Helper.EntityTreeUtil.GetPath(aFile);
                var fld = parent as Entity.FolderEntity;
                fld.Files.Remove(aFile);
                Helper.EntityTreeUtil.UpdateDict();
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                catch { }
            }
        }

        public static void SeparateFile(Entity.FileEntity aFile)
        {
            if (SolutionUtil.Solution == null) return;
            var parent = Helper.EntityTreeUtil.GetParent(aFile);
            if (parent == null) return;
            if (parent is Entity.ApplicationEntity)
            {
                var app = parent as Entity.ApplicationEntity;
                app.Files.Remove(aFile);
                Helper.EntityTreeUtil.UpdateDict();
            }
            if(parent is Entity.FolderEntity)
            {
                var fld = parent as Entity.FolderEntity;
                fld.Files.Remove(aFile);
                Helper.EntityTreeUtil.UpdateDict();
            }
        }

        public static void SaveFile(Entity.FileEntity aFile, string aContent)
        {
            if (SolutionUtil.Solution == null) return;
            var path = Helper.EntityTreeUtil.GetPath(aFile);
            System.IO.File.WriteAllText(path, aContent);
        }

        public static string LoadFile(Entity.FileEntity aFile)
        {
            var path = Helper.EntityTreeUtil.GetPath(aFile);
            var content = System.IO.File.ReadAllText(path);
            return content;
        }
    }
}
