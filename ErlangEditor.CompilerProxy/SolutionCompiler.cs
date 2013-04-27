using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErlangEditor.Core.Entity;
using System.Diagnostics;

namespace ErlangEditor.CompilerProxy
{
    public class SolutionCompiler
    {
        public void Start(SolutionEntity aEntity, IEnumerable<CodeEntity> aEntities, string aExportPath)
        {
            var prc = new Process();
            prc.StartInfo = new ProcessStartInfo();
            prc.StartInfo.CreateNoWindow = false;
            prc.StartInfo.FileName = aEntity.CompilerPath;
            prc.StartInfo.Arguments = "";
        }
    }
}
