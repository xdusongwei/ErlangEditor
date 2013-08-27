using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ErlangEditor.Core.UnitTest
{
    public static class NodeTest
    {
        public static void Test()
        {
            //create node
            NodeUtil.AddNode("anode@localhost", true);
            Debug.Assert(SolutionUtil.Solution.Nodes.Any(i => i.NodeName == "anode@localhost" && i.ShowShell == true));
            //inject app
            NodeUtil.InjectionApp("anode@localhost", "hello");
            Debug.Assert(SolutionUtil.Solution.Apps.First(i => i.Name == "hello").NodeNames.Contains("anode@localhost"));
            Debug.Assert(SolutionUtil.Solution.Nodes.First(i => i.NodeName == "anode@localhost").Apps.Contains("hello"));
            //sep app
            NodeUtil.SeparateApp("anode@localhost", "hello");
            Debug.Assert(!SolutionUtil.Solution.Apps.First(i => i.Name == "hello").NodeNames.Contains("anode@localhost"));
            Debug.Assert(!SolutionUtil.Solution.Nodes.First(i => i.NodeName == "anode@localhost").Apps.Contains("hello"));
            //startup node
            NodeUtil.StartupNode("anode@localhost");
            //shutdown node
            NodeUtil.StopNode("anode@localhost");
            //remove node
            NodeUtil.DeleteNode("anode@localhost");
            Debug.Assert(!SolutionUtil.Solution.Nodes.Any(i => i.NodeName == "anode@localhost"));

            SolutionUtil.SaveSolution();
        }
    }
}
