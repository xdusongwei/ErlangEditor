using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ErlangEditor.ReleaseProxy
{
    public class ReleaseCore
    {
        public void MakeTar(string aLibPath)
        {
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var prc = new Process();
            prc.StartInfo = new ProcessStartInfo();
            prc.StartInfo.CreateNoWindow = true;
            prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
            prc.StartInfo.Arguments = string.Format("-noshell  -eval \"" +
                "application:start(sasl)," +
                "systools:make_tar(\\\"{0}\\\",[])," +
                "io:format(\\\"%OK%\\\")," +
                "init:stop().\"", ErlangEditor.Core.SolutionUtil.Solution.Name );
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.WorkingDirectory = aLibPath;
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.EnableRaisingEvents = true;
            prc.Start();
            var result = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit(30000);
            if (!result.Contains("%OK%"))
            {
                throw new Exception("打包应用时遇到了问题。");
            }
        }

        public void MakeScript(IEnumerable< ErlangEditor.Core.Entity.ApplicationEntity> aApps, bool aLocal)
        {
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var basePath = ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath;
            var pas = aApps.Select(x => string.Format("-pa {0}" ,  System.IO.Path.Combine(basePath, ErlangEditor.Core.SolutionUtil.Solution.Name, x.Name, "ebin")));
            var arg = string.Join(" ", pas);
            var prc = new Process();
            prc.StartInfo = new ProcessStartInfo();
            prc.StartInfo.CreateNoWindow = true;
            prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
            prc.StartInfo.Arguments = string.Format("-noshell {0} -eval \"" +
                "application:start(sasl)," +
                "systools:make_script(\\\"{1}\\\",[{2}])," +
                "io:format(\\\"%OK%\\\")," +
                "init:stop().\"", arg, ErlangEditor.Core.SolutionUtil.Solution.Name, aLocal ? "local" : string.Empty);
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.WorkingDirectory = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.EnableRaisingEvents = true;
            prc.Start();
            var result = prc.StandardOutput.ReadToEnd();
            prc.WaitForExit(30000);
            if (!result.Contains("%OK%"))
            {
                throw new Exception("对应用创建脚本时遇到了问题。");
            }
        }
    }
}
