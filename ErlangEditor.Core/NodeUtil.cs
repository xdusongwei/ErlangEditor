using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core
{
    public static class NodeUtil
    {
        public static void AddNode(string aName , bool aShowShell)
        {
            if(string.IsNullOrWhiteSpace(aName)) return;
            aName = aName.Trim();
            if (SolutionUtil.Solution == null) return;
            var sln = SolutionUtil.Solution;
            if(sln.Nodes.Any(i=>i.NodeName == aName)) return;
            var node = new Entity.NodeEntity { NodeName = aName, IsRunning = false, ShowShell = aShowShell };
            sln.Nodes.Add(node);
        }

        public static void DeleteNode(string aName)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Nodes.Any(i => i.NodeName == aName))
            {
                var node = SolutionUtil.Solution.Nodes.First(i => i.NodeName == aName);
                if (node.IsRunning)
                    throw new Exception("不可以在节点运行时将其删除。");
                SolutionUtil.Solution.Nodes.Remove(node);
                foreach (var i in SolutionUtil.Solution.Apps)
                {
                    i.NodeNames.RemoveAll(j => j == aName);
                }
            }
        }

        public static void StartupNode(string aName)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Nodes.Any(i => i.NodeName == aName))
            {
                var node = SolutionUtil.Solution.Nodes.First(i => i.NodeName == aName);
                if (node.IsRunning)
                    throw new Exception("节点正在运行。");
                node.IsRunning = true;
            }
        }

        public static void StopNode(string aName)
        {
            if (SolutionUtil.Solution == null) return;
            if (SolutionUtil.Solution.Nodes.Any(i => i.NodeName == aName))
            {
                var node = SolutionUtil.Solution.Nodes.First(i => i.NodeName == aName);
                node.IsRunning = false;
            }
        }

        public static void InjectionApp(string aNode, string aApp)
        {
            if (SolutionUtil.Solution == null) return;
            if (string.IsNullOrWhiteSpace(aNode) || string.IsNullOrWhiteSpace(aApp)) return;
            aNode = aNode.Trim();
            aApp = aApp.Trim();
            var sln = SolutionUtil.Solution;
            if (sln.Nodes.Any(i => i.NodeName == aNode))
            {
                if (sln.Apps.Any(i => i.Name == aApp))
                {
                    var app = sln.Apps.First(i=>i.Name == aApp);
                    if (!app.NodeNames.Contains(aNode))
                        app.NodeNames.Add(aNode);
                }
                if(sln.Nodes.Any(i=>i.NodeName== aNode))
                {
                    var node = sln.Nodes.First(i=>i.NodeName == aNode);
                    if(!node.Apps.Contains(aApp))
                        node.Apps.Add(aApp);
                }
            }
        }

        public static void SeparateApp(string aNode, string aApp)
        {
            if (SolutionUtil.Solution == null) return;
            if (string.IsNullOrWhiteSpace(aNode) || string.IsNullOrWhiteSpace(aApp)) return;
            aNode = aNode.Trim();
            aApp = aApp.Trim();
            var sln = SolutionUtil.Solution;
            if (sln.Nodes.Any(i => i.NodeName == aNode))
            {
                if (sln.Apps.Any(i => i.Name == aApp))
                {
                    var app = sln.Apps.First(i => i.Name == aApp);
                    if (app.NodeNames.Contains(aNode))
                        app.NodeNames.Remove(aNode);
                }
                if (sln.Nodes.Any(i => i.NodeName == aNode))
                {
                    var node = sln.Nodes.First(i => i.NodeName == aNode);
                    if (node.Apps.Contains(aApp))
                        node.Apps.Remove(aApp);
                }
            }
        }
    }
}
