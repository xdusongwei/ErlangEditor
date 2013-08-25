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
            prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ShellPath;
            //prc.StartInfo.Arguments = string.Format("-I {0} -o ebin src\\{1}", "include", fullPath);
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.RedirectStandardInput = prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.WindowStyle = aEntity.ShowShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
            prc.EnableRaisingEvents = true;
            prc.Exited += (a, b) => { var evt = Closed; if (evt != null)  evt(this, new ShellClosedEventArgs()); };
            prc_ = prc;
            prc.Start();
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
