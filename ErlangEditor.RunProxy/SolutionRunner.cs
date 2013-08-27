using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ErlangEditor.RunProxy
{
    public class SolutionRunner
    {
        private Process prc_ = null;

        public event EventHandler<ShellClosedEventArgs> Closed;

        public void Run(ErlangEditor.Core.Entity.NodeEntity aEntity)
        {
            var prc = new Process();
            prc.StartInfo = new ProcessStartInfo();
            prc.StartInfo.CreateNoWindow = !aEntity.ShowShell;
            prc.StartInfo.FileName = aEntity.ShowShell ? ErlangEditor.Core.ConfigUtil.Config.ShellPath : ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
            var sln = ErlangEditor.Core.SolutionUtil.Solution;
            var pathSB = new StringBuilder(1024);
            var startupSB = new StringBuilder(1024);
            foreach (var i in aEntity.Apps)
            {
                if (sln.Apps.Any(j => j.Name == i))
                {
                    var app = sln.Apps.First(j => j.Name == i);
                    var path = string.Empty;
                    if (app.AppMode)
                    {
                        path = ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(app);
                    }
                    if (app.CodeMode)
                    {
                        path = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(app), "ebin");
                    }
                    if(!string.IsNullOrWhiteSpace(path))
                        pathSB.AppendFormat("-pa \"{0}\" ", path);
                    if (app.StartupAsMFA && !string.IsNullOrWhiteSpace(app.StartupMFA))
                    {
                        startupSB.AppendFormat("-s {0} ", app.StartupMFA);
                    }
                }
            }
            prc.StartInfo.Arguments = pathSB.ToString() + startupSB.ToString();
            prc.StartInfo.UseShellExecute = false;
            //prc.StartInfo.RedirectStandardInput = 
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.WindowStyle = aEntity.ShowShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
            prc.EnableRaisingEvents = true;
            prc.Exited += (a, b) => { var evt = Closed; if (evt != null)  evt(this, new ShellClosedEventArgs()); };
            prc.OutputDataReceived += (a, b) => { Debug.WriteLine(b.Data); };
            prc_ = prc;
            prc.Start();
            prc.BeginOutputReadLine();
        }

        public void Stop()
        {
            if (prc_ != null)
            {
                prc_.Kill();
                prc_.Dispose();
                prc_ = null;
                var evt = Closed; 
                if (evt != null) 
                    evt(this, new ShellClosedEventArgs());
            }
        }
    }
}
