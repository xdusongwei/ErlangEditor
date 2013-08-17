using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ErlangEditor.Core.UnitTest
{
    public static class FileTest
    {
        public static void Test()
        {
            //add file
            FileUtil.AddFile("hello", string.Empty, new Entity.FileEntity { Name = "hello.erl" },"");
            Debug.Assert(SolutionUtil.Solution.Apps.First().Files.Any(x=>x.Name =="hello.erl"));
            Debug.Assert(System.IO.File.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello\\hello.erl")); 
            //add file in src
            FileUtil.AddFile("hello", "src", new Entity.FileEntity { Name = "hello.erl" },"");
            Debug.Assert(SolutionUtil.Solution.Apps.First().Folders.First(x=>x.Name=="src").Files.Any(x => x.Name == "hello.erl"));
            Debug.Assert(System.IO.File.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello\\src\\hello.erl")); 
            //sep file
            FileUtil.SeparateFile(SolutionUtil.Solution.Apps.First().Files.First(x => x.Name == "hello.erl"));
            Debug.Assert(!SolutionUtil.Solution.Apps.First().Files.Any(x => x.Name == "hello.erl"));
            Debug.Assert(System.IO.File.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello\\hello.erl")); 
            //remove file
            FileUtil.RemoveFile(SolutionUtil.Solution.Apps.First().Folders.First(x => x.Name == "src").Files.First(x => x.Name == "hello.erl"));
            Debug.Assert(!SolutionUtil.Solution.Apps.First().Folders.First(x => x.Name == "src").Files.Any(x => x.Name == "hello.erl"));
            Debug.Assert(!System.IO.File.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello\\src\\hello.erl")); 
            //add file in include
            FileUtil.AddFile("hello", "include", new Entity.FileEntity { Name = "info.txt", DisplayName="info" },"");
            Debug.Assert(SolutionUtil.Solution.Apps.First().Folders.First(x => x.Name == "include").Files.Any(x => x.Name == "info.txt"));
            Debug.Assert(System.IO.File.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello\\include\\info.txt")); 
            //save file as "qwerty"
            FileUtil.SaveFile(SolutionUtil.Solution.Apps.First().Folders.First(x => x.Name == "include").Files.First(x => x.Name == "info.txt"), "qwerty");
            Debug.Assert(FileUtil.LoadFile(SolutionUtil.Solution.Apps.First().Folders.First(x => x.Name == "include").Files.First(x => x.Name == "info.txt")) == "qwerty");
            //save sln file
            SolutionUtil.SaveSolution();
        }
    }
}
