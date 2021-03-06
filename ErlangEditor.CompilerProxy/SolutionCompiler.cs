﻿using System;
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
            Collection<string> aExportReport,
            Action<FileEntity> aFileComplete,
            Action<bool> aFinish)
        {
            Start(aApps, aExportReport, aFileComplete, aFinish, true);
        }

        public void Start(
            IEnumerable<ApplicationEntity> aApps,
            Collection<string> aExportReport,
            Action<FileEntity> aFileComplete,
            Action<bool> aFinish,
            bool aAsync
            )
        {
            if (thCompiler != null)
            {
                thCompiler.Abort();
                thCompiler = null;
            }
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.CompilerPath))
                throw new Exception("尚未制定编译器位置，请从\"设置\"页设置正确的Erlang编译器程序位置。");
            if(!System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.CompilerPath))
                throw new Exception("编译器程序位置不存在，请从\"设置\"页设置正确的Erlang编译器程序位置。");
            if (aExportReport == null) aExportReport = new Collection<string>();
            aExportReport.Clear();
            PrintBegin(aExportReport);
            if (aAsync)
            {
                thCompiler = new Thread(() =>
                {
                    ExecuteProc(aApps, aExportReport, aFileComplete, aFinish);
                });
                thCompiler.IsBackground = true;
                thCompiler.Start();
            }
            else
            {
                ExecuteProc(aApps, aExportReport, aFileComplete, aFinish);
            }
        }

        private void ExecuteProc(IEnumerable<ApplicationEntity> aApps, Collection<string> aExportReport, Action<FileEntity> aFileComplete, Action<bool> aFinish)
        {
            int success = 0;
            int failed = 0;
            int warning = 0;
            foreach (var i in aApps)
            {
                var existAppName = i.IncludePath.Where(x => ErlangEditor.Core.SolutionUtil.Solution.Apps.Where(y => y.Name == x).Count() > 0);
                var existApp = ErlangEditor.Core.SolutionUtil.Solution.Apps.Where(x => existAppName.Any(y => y == x.Name));
                var pa = string.Concat(existApp.Select(x => string.Format("-pa {0} ", System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(x), "ebin"))));
                foreach (var k in i.Folders.Where(x => x.Name == "src"))
                {
                    foreach (var j in k.Files)
                    {
                        if (j.IsAppFile)
                        {
                            System.IO.File.Copy(
                                ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(j),
                                System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, i.Name, "ebin", j.DisplayName),
                                true);
                            if (aFileComplete != null) aFileComplete(j);
                            continue;
                        }
                        Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译{0}", j.Name), aExportReport });
                        using (var prc = new Process())
                        {
                            prc.StartInfo = new ProcessStartInfo();
                            prc.StartInfo.CreateNoWindow = true;
                            prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.CompilerPath;
                            prc.StartInfo.WorkingDirectory = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, i.Name);
                            var fullPath = j.Name;
                            prc.StartInfo.Arguments = string.Format("{0} {1} -I \"{2}\" -o ebin {3} src\\{4}", i.CompileNative ? "+native +\"{hipe, [o3]}\"" : string.Empty, i.DebugInfo ? "+debug_info" : string.Empty, "include", pa, fullPath);
                            prc.StartInfo.UseShellExecute = false;
                            prc.StartInfo.RedirectStandardInput = prc.StartInfo.RedirectStandardOutput = true;
                            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            prc.Start();
                            prc.WaitForExit(10000);
                            var result = prc.StandardOutput.ReadToEnd();
                            if (string.IsNullOrEmpty(result))
                            {
                                success++;
                                if (aFileComplete != null) aFileComplete(j);
                            }
                            else
                            {
                                var lines = result.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
                                if (lines.All(x => x.Contains("Warning")))
                                {
                                    success++;
                                    warning += lines.Where(x => x.Contains("Warning")).Count();
                                    var results = result.Replace(prc.StartInfo.Arguments, string.Empty).Split(new char[] { '\n' }).Where(a => !string.IsNullOrEmpty(a));
                                    foreach (var l in results)
                                    {
                                        var evt = CodeFileError;
                                        if (evt != null)
                                            Dispatcher.Invoke(new Action<object, CodeFileErrorEventArgs>(evt), new object[] { this, new CodeFileErrorEventArgs(l, j) });
                                    }
                                    if (aFileComplete != null) aFileComplete(j);
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
                            if (aFinish != null) aFinish(failed == 0);
                        }
                    }
                }
            }
            PrintEnd(aExportReport, success,warning, failed);
        }

        
        public event EventHandler<CodeFileErrorEventArgs> CodeFileError;

        private static void PrintBegin(Collection<string> aExportReport)
        {
            PrintLine("开始编译", aExportReport);
        }

        private void PrintEnd(Collection<string> aExportReport, int aSuccess,int aWarning, int aFailed)
        {
            Dispatcher.Invoke(new Action<string, Collection<string>>(PrintLine), new object[] { string.Format("编译完毕，成功{0}，警告{1}，失败{2}。", aSuccess,aWarning, aFailed), aExportReport });
            //PrintLine();
        }

        private static void PrintLine(string aLog, Collection<string> aExportReport)
        {
            var line = string.Format("{0} - {1}", DateTime.Now.ToShortTimeString(), aLog);
            aExportReport.Add(line);
        }
    }
}
