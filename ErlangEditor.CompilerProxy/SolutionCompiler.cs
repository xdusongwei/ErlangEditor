using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ErlangEditor.Core.Entity;
using ErlangEditor.Core;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;

namespace ErlangEditor.CompilerProxy
{
    public class SolutionCompiler : DispatcherObject
    {
        private static object locker_ = new object();
        private static volatile Thread thCompiler = null;
        public void Start(
            SolutionEntity aEntity, 
            IEnumerable<CodeEntity> aEntities, 
            string aExportPath, 
            Collection<string> aExportReport,
            Collection<string> aErrorReport
            )
        {
            if (thCompiler != null)
            {
                thCompiler.Abort();
                thCompiler = null;
            }
            aExportReport.Clear();
            aErrorReport.Clear();
            PrintBegin(aExportReport);
            int success=0;
            int failed = 0;
            thCompiler = new Thread(() =>
            {
                foreach (var i in aEntities)
                {
                    Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译{0}", Solution.GetFullPath(i.Entity)), aExportReport });
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
                        var result = prc.StandardOutput.ReadToEnd();
                        if (string.IsNullOrEmpty(result))
                        {
                            success++;
                        }
                        else
                        {
                            failed++;
                            var results = result.Split(new char[] { '\n' });
                            foreach(var j in results)
                                Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { j.Trim(), aErrorReport });
                        }
                    }
                }
                PrintEnd(aExportReport, success, failed);
            });
            thCompiler.Start();
        }

        private void PrintBegin(Collection<string> aExportReport)
        {
            PrintLine("解决方案保存完毕", aExportReport);
        }

        private void PrintEnd(Collection<string> aExportReport, int aSuccess , int aFailed)
        {
            Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译完毕，成功{0}，失败{1}。", aSuccess, aFailed), aExportReport });
            //PrintLine();
        }

        private void PrintLine(string aLog, Collection<string> aExportReport)
        {
            var line = string.Format("{0} - {1}", DateTime.Now.ToShortTimeString(), aLog);
            aExportReport.Add(line);
        }
    }
}
