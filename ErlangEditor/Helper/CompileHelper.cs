using ErlangEditor.CompilerProxy;
using ErlangEditor.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Helper
{
    public class CompileHelper
    {
        public void MakeSolution()
        {
            App.MainViewModel.ErrorLog.Clear();
            var slnCompiler = new SolutionCompiler();
            slnCompiler.CodeFileError += (a, b) =>
            {
                var item = new ErrorInfoVM();
                var spIndex = b.Log.IndexOf(": ");
                var front = b.Log.Substring(0, spIndex);
                var line = Convert.ToInt32(front.Substring(front.LastIndexOf(':') + 1, front.Length - front.LastIndexOf(':') - 1));
                var back = b.Log.Substring(spIndex + 1, b.Log.Length - spIndex - 1);
                item.Entity = b.Entity;
                item.Log = back;
                item.Line = line;
                App.MainViewModel.ErrorLog.Add(item);
            };
            slnCompiler.Start(ErlangEditor.Core.SolutionUtil.Solution.Apps, App.MainViewModel.ExportLog);
        }

        public void MakeApp(PrjTreeItemVM aAppVM)
        {
            App.MainViewModel.ErrorLog.Clear();
            var slnCompiler = new SolutionCompiler();
            slnCompiler.CodeFileError += (a, b) =>
            {
                var item = new ErrorInfoVM();
                var spIndex = b.Log.IndexOf(": ");
                var front = b.Log.Substring(0, spIndex);
                var line = Convert.ToInt32(front.Substring(front.LastIndexOf(':') + 1, front.Length - front.LastIndexOf(':') - 1));
                var back = b.Log.Substring(spIndex + 1, b.Log.Length - spIndex - 1);
                item.Entity = b.Entity;
                item.Log = back;
                item.Line = line;
                App.MainViewModel.ErrorLog.Add(item);
            };
            slnCompiler.Start(new ErlangEditor.Core.Entity.ApplicationEntity[] { aAppVM.Entity as ErlangEditor.Core.Entity.ApplicationEntity }, App.MainViewModel.ExportLog);
        }
    }
}
