using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ErlangEditor.RunProxy
{
    public class SolutionRunner
    {
        private volatile Process prc_ = null;

        public event EventHandler<ShellClosedEventArgs> Closed;

        public void Run(ErlangEditor.Core.Entity.NodeEntity aEntity)
        {
            if(prc_ != null)
            {
                try
                {
                    prc_.Kill();
                }
                catch { }
                prc_ = null;
            }
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var prc = new Process();
            prc.StartInfo = new ProcessStartInfo();
            prc.StartInfo.CreateNoWindow = !aEntity.ShowShell;
            prc.StartInfo.FileName = aEntity.ShowShell ? ErlangEditor.Core.ConfigUtil.Config.ShellPath : ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
            var sln = ErlangEditor.Core.SolutionUtil.Solution;
            var pathSB = new StringBuilder(1024);
            var startupSB = new StringBuilder(1024);
            var pas = string.Empty; //这个...比较矛盾
            foreach (var i in aEntity.Apps)
            {
                if (sln.Apps.Any(j => j.Name == i))
                {
                    var app = sln.Apps.First(j => j.Name == i);
                    var path = string.Empty;
                    path = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(app), "ebin");
                    //pas += string.Concat(app.IncludePath.Select(x => string.Format("-pa {0} ", x)));
                    if(!string.IsNullOrWhiteSpace(path))
                        pathSB.AppendFormat("-pz \"{0}\" ", path);
                    if (app.StartupAsMFA && !string.IsNullOrWhiteSpace(app.StartupMFA))
                    {
                        startupSB.AppendFormat("-s {0} ", app.StartupMFA);
                    }
                }
            }
            prc.StartInfo.Arguments = string.Format("-name {0} " , aEntity.NodeName) +  pathSB.ToString() + pas + startupSB.ToString();
            prc.StartInfo.UseShellExecute = false;
            prc.StartInfo.WorkingDirectory = ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath;
            //prc.StartInfo.RedirectStandardInput = 
            prc.StartInfo.RedirectStandardOutput = true;
            prc.StartInfo.WindowStyle = aEntity.ShowShell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;
            prc.EnableRaisingEvents = true;
            prc.Exited += (a, b) => { var evt = Closed; if (evt != null)  evt(this, new ShellClosedEventArgs()); prc_ = null; };
            prc.OutputDataReceived += (a, b) => 
                { 
                    var evt = NewOutput; 
                    if (evt != null) 
                        evt(this, new NewOutputLineEventArgs { NodeName = aEntity.NodeName, Data = b.Data }); 
                };
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

        public event EventHandler<NewOutputLineEventArgs> NewOutput;
    }
}
