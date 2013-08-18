using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ErlangEditor.Core.UnitTest
{
    public static class ApplicationTest
    {
        public static void Test()
        {
            //add app
            ApplicationUtil.AddApplication(new Entity.ApplicationEntity { Name = "hello" });
            Debug.Assert(SolutionUtil.Solution != null && SolutionUtil.Solution.Apps.Any(x => x.Name == "hello"));
            Debug.Assert(System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello")); 
            //sep app
            ApplicationUtil.SeparateApplication("hello");
            Debug.Assert(SolutionUtil.Solution != null && !SolutionUtil.Solution.Apps.Any(x => x.Name == "hello"));
            //re add app
            System.IO.Directory.Delete(System.Environment.CurrentDirectory + "\\testSln\\hello", true);
            ApplicationUtil.AddApplication(new Entity.ApplicationEntity { Name = "hello" });
            //remove app
            ApplicationUtil.DeleteApplication("hello");
            Debug.Assert(SolutionUtil.Solution != null && !SolutionUtil.Solution.Apps.Any(x => x.Name == "hello"));
            Debug.Assert(!System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\testSln\\hello")); 
            //add app
            ApplicationUtil.AddApplication(new Entity.ApplicationEntity { Name = "hello" });
        }
    }
}
