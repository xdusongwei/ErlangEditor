﻿using ErlangEditor.CompilerProxy;
using ErlangEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace ErlangEditor.Helper
{
    public class CompileHelper : DispatcherObject
    {
        public void MakeSolution()
        {
            App.MainViewModel.ErrorLog.Clear();
            App.MainViewModel.ErrorCount = 0;
            var slnCompiler = new SolutionCompiler();
            slnCompiler.CodeFileError += (a, b) =>
            {
                var item = new ErrorInfoVM();
                var spIndex = b.Log.IndexOf(": ");
                if (spIndex == -1)
                {
                    item.Log = b.Log;
                }
                else
                {
                    var front = b.Log.Substring(0, spIndex);
                    var line = front.Substring(front.LastIndexOf(':') + 1, front.Length - front.LastIndexOf(':') - 1);
                    var back = b.Log.Substring(spIndex + 1, b.Log.Length - spIndex - 1);
                    item.Entity = b.Entity;
                    item.Log = back;
                    item.Line = line;
                }
                App.MainViewModel.ErrorLog.Add(item);
                App.MainViewModel.ErrorCount++;
            };
            App.MainViewModel.IsCompiling = true;
            slnCompiler.Start(
                ErlangEditor.Core.SolutionUtil.Solution.Apps, App.MainViewModel.ExportLog, 
                null, 
                (x) => 
                { 
                    App.MainViewModel.AutoCompleteCache.ScanAllBin(ErlangEditor.Core.SolutionUtil.Solution);
                    Dispatcher.Invoke(new Action(() =>
                        {
                            App.MainViewModel.IsCompiling = false;
                        }));
                });
        }

        public void MakeApp(PrjTreeItemVM aAppVM)
        {
            App.MainViewModel.ErrorLog.Clear();
            var slnCompiler = new SolutionCompiler();
            App.MainViewModel.ErrorCount = 0;
            slnCompiler.CodeFileError += (a, b) =>
            {
                var item = new ErrorInfoVM();
                var spIndex = b.Log.IndexOf(": ");
                if (spIndex == -1)
                {
                    item.Log = b.Log;
                }
                else
                {
                    var front = b.Log.Substring(0, spIndex);
                    var line = front.Substring(front.LastIndexOf(':') + 1, front.Length - front.LastIndexOf(':') - 1);
                    var back = b.Log.Substring(spIndex + 1, b.Log.Length - spIndex - 1);
                    item.Entity = b.Entity;
                    item.Log = back;
                    item.Line = line;
                }
                App.MainViewModel.ErrorLog.Add(item);
                App.MainViewModel.ErrorCount++;
            };
            App.MainViewModel.IsCompiling = true;
            slnCompiler.Start(
                new ErlangEditor.Core.Entity.ApplicationEntity[] { aAppVM.Entity as ErlangEditor.Core.Entity.ApplicationEntity }, 
                App.MainViewModel.ExportLog, null,
                (x) =>
                {
                    App.MainViewModel.AutoCompleteCache.ScanAllBin(ErlangEditor.Core.SolutionUtil.Solution);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        App.MainViewModel.IsCompiling = false;
                    }));
                });
        }
    }
}
