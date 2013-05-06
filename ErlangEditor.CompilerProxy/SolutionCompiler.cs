using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ErlangEditor.Core.Entity;
using ErlangEditor.Core;
using System.Diagnostics;

namespace ErlangEditor.CompilerProxy
{
    public class SolutionCompiler
    {
        public IEnumerable<string> Start(SolutionEntity aEntity, IEnumerable<CodeEntity> aEntities, string aExportPath)
        {
            LinkedList<string> lst = new LinkedList<string>();
            foreach (var i in aEntities)
            {
                using (var prc = new Process())
                {
                    prc.StartInfo = new ProcessStartInfo();
                    prc.StartInfo.CreateNoWindow = true;
                    prc.StartInfo.FileName = aEntity.CompilerPath;
                    prc.StartInfo.WorkingDirectory = Path.Combine(aEntity.SolutionPath, aEntity.MakeFolder);
                    prc.StartInfo.Arguments = Solution.GetFullPath(i.Entity);
                    prc.StartInfo.UseShellExecute = false;
                    prc.StartInfo.RedirectStandardInput = prc.StartInfo.RedirectStandardOutput = true;
                    prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    prc.Start();
                    prc.WaitForExit();
                    lst.AddLast(new LinkedListNode<string>(prc.StandardOutput.ReadToEnd()));
                }
            }
            return lst;
        }
    }
}
