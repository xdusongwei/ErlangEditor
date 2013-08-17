using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.UnitTest
{
    public static class SolutionTest
    {
        public static void Test()
        {
            System.IO.File.Delete(System.Environment.CurrentDirectory + "\\testSln\\testSln.sln"); 
            //create sln
            SolutionUtil.CreateSolution("testSln", System.Environment.CurrentDirectory);
            Debug.Assert(SolutionUtil.Solution != null && SolutionUtil.Solution.Name == "testSln");
            //close sln
            SolutionUtil.CloseSolution();
            Debug.Assert(SolutionUtil.Solution == null);
            //load sln
            SolutionUtil.LoadSolution(System.Environment.CurrentDirectory + "\\testSln\\testSln.sln");
            Debug.Assert(SolutionUtil.Solution != null && SolutionUtil.Solution.Name == "testSln");
            //save sln
            SolutionUtil.SaveSolution();
        }
    }
}
