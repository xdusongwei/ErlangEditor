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
            IEnumerable<ApplicationEntity> aApps,
            Collection<string> aExportReport
            )
        {
            if (thCompiler != null)
            {
                thCompiler.Abort();
                thCompiler = null;
            }
            int success = 0;
            int failed = 0;
            aExportReport.Clear();
            PrintBegin(aExportReport);
            thCompiler = new Thread(() =>
            {
                foreach(var i in aApps)
                    foreach(var k in i.Folders.Where(x=>x.Name=="src"))
                        foreach (var j in k.Files)
                        {
                            Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译{0}", j.Name), aExportReport });
                            using (var prc = new Process())
                            {
                                prc.StartInfo = new ProcessStartInfo();
                                prc.StartInfo.CreateNoWindow = true;
                                prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.CompilerPath;
                                prc.StartInfo.WorkingDirectory = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, i.Name);
                                var fullPath = j.Name;
                                prc.StartInfo.Arguments = string.Format("-I {0} -o ebin src\\{1}", "include", fullPath);
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
                                    var results = result.Replace(prc.StartInfo.Arguments, string.Empty).Split(new char[] { '\n' }).Where(a => !string.IsNullOrEmpty(a));
                                    foreach (var l in results)
                                    {
                                        var evt = CodeFileError;
                                        if (evt != null)
                                            Dispatcher.Invoke(new Action<object, CodeFileErrorEventArgs>(evt), new object[] { this, new CodeFileErrorEventArgs(l, j) });
                                    }
                                }
                            }
                        }
                PrintEnd(aExportReport, success, failed);
            });
            thCompiler.Start();
        }

        //private static void Dig(object aEntity, Action<object> aAction)
        //{
        //    foreach (dynamic i in (aEntity as dynamic).Children)
        //    {
        //        if (i.Compilable && i.Name.IndexOf("m") == 0)
        //        {
        //            aAction(i);
        //        }
        //        else if (i.IsFolder)
        //        {
        //            Dig(i, aAction);
        //        }
        //    }
        //}


        //public void Start(
        //    SolutionEntity aEntity, 
        //    IEnumerable<CodeEntity> aEntities, 
        //    string aExportPath, 
        //    Collection<string> aExportReport
        //    )
        //{
        //    if (thCompiler != null)
        //    {
        //        thCompiler.Abort();
        //        thCompiler = null;
        //    }
        //    aExportReport.Clear();
        //    PrintBegin(aExportReport);
        //    int success=0;
        //    int failed = 0;
        //    thCompiler = new Thread(() =>
        //    {
        //        foreach (var i in aEntities)
        //        {
        //            Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译{0}", Solution.GetFullPath(i.Entity)), aExportReport });
        //            using (var prc = new Process())
        //            {
        //                prc.StartInfo = new ProcessStartInfo();
        //                prc.StartInfo.CreateNoWindow = true;
        //                prc.StartInfo.FileName = aEntity.CompilerPath;
        //                prc.StartInfo.WorkingDirectory = Path.Combine(aEntity.SolutionPath, aEntity.MakeFolder);
        //                prc.StartInfo.Arguments = Solution.GetFullPath(i.Entity);
        //                prc.StartInfo.UseShellExecute = false;
        //                prc.StartInfo.RedirectStandardInput = prc.StartInfo.RedirectStandardOutput = true;
        //                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //                prc.Start();
        //                prc.WaitForExit();
        //                var result = prc.StandardOutput.ReadToEnd();
        //                if (string.IsNullOrEmpty(result))
        //                {
        //                    success++;
        //                }
        //                else
        //                {
        //                    failed++;
        //                    var results = result.Replace(prc.StartInfo.Arguments,string.Empty).Split(new char[] { '\n' }).Where(a => !string.IsNullOrEmpty(a));
        //                    foreach (var j in results)
        //                    {
        //                        var evt = CodeFileError;
        //                        if (evt != null)
        //                            Dispatcher.Invoke(new Action<object, CodeFileErrorEventArgs>(evt), new object[] { this, new CodeFileErrorEventArgs(j, i.Entity) });
        //                    }
        //                }
        //            }
        //        }
        //        PrintEnd(aExportReport, success, failed);
        //    });
        //    thCompiler.Start();
        //}

        public event EventHandler<CodeFileErrorEventArgs> CodeFileError;

        private static void PrintBegin(Collection<string> aExportReport)
        {
            PrintLine("解决方案保存完毕", aExportReport);
        }

        private void PrintEnd(Collection<string> aExportReport, int aSuccess, int aFailed)
        {
            Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译完毕，成功{0}，失败{1}。", aSuccess, aFailed), aExportReport });
            //PrintLine();
        }

        private static void PrintLine(string aLog, Collection<string> aExportReport)
        {
            var line = string.Format("{0} - {1}", DateTime.Now.ToShortTimeString(), aLog);
            aExportReport.Add(line);
        }
    }
}
