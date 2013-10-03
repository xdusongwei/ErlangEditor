using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ErlangEditor.AutoComplete
{
    public class AcCore
    {
        private Dictionary<string, List<AcEntity>> dictMods_ = new Dictionary<string, List<AcEntity>>();
        private Dictionary<string, List<AcEntity>> dictLibCache_ = new Dictionary<string, List<AcEntity>>();

        public void SetLibCache(string aJsonDict)
        {
            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<AcEntity>>>(aJsonDict);
            dictLibCache_ = dict;
        }

        public IEnumerable<string> GetModules
        {
            get
            {
                var result = new List<string>(dictMods_.Keys);
                result.Sort();
                return result;
            }
        }

        public IEnumerable<AcEntity> GetFunctions(string aModule)
        {
            if (dictMods_.ContainsKey(aModule))
            {
                var result =  new List<AcEntity>(dictMods_[aModule]);
                return result;
            }
            else
            {
                return new List<AcEntity>();
            }
        }

        public void FreeCache()
        {
            dictMods_.Clear();
            dictMods_ = new Dictionary<string, List<AcEntity>>(dictLibCache_);
        }

        public void ScanAllBin(ErlangEditor.Core.Entity.SolutionEntity aSln)
        {
            FreeCache();
            ScanBin(aSln);
        }

        public void ScanBin(ErlangEditor.Core.Entity.SolutionEntity aEntity)
        {
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var lstBeam = new List<string>();
            foreach (var j in aEntity.Apps)
            {
                var ebin = j.Folders.First(i => i.Name == "ebin");
                var ebinpath = ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(ebin);
                foreach (var i in j.Folders.First(x => x.Name == "src").Files)
                {
                    var filepath = System.IO.Path.Combine(ebinpath, i.DisplayName + ".beam");
                    if (System.IO.File.Exists(filepath))
                        lstBeam.Add(i.DisplayName);
                }
            }
            if (lstBeam.Count>0)
            {
                var lst = string.Format("[{0}]" , string.Join("," , lstBeam));
                var prc = new Process();
                prc.StartInfo = new ProcessStartInfo();
                prc.StartInfo.CreateNoWindow = true;
                prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
                prc.StartInfo.Arguments = string.Format("-noshell -eval \"" +
                    "B = fun(X)-> lists:map(fun({{A,B}})-> io:format(\\\"~p,~p,~p~n\\\",[X,A,B]) end, X:module_info(exports)) end," +
                    "lists:map(fun(X)-> B(X) end, {0})," +
                    "init:stop().\" {1}", lst, string.Concat( aEntity.Apps.Select(x => " -pa " + System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(x) + "\\ebin"))));
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.WorkingDirectory = ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath;
                prc.StartInfo.RedirectStandardOutput = true;
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.EnableRaisingEvents = true;
                prc.Start();
                var result = prc.StandardOutput.ReadToEnd();
                var lines = result.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));
                foreach (var i in lines)
                {
                    var args = i.Split(',');
                    var mod = args[0];
                    var func = args[1];
                    var arity = args[2];
                    if (!dictMods_.ContainsKey(mod))
                        dictMods_[mod] = new List<AcEntity>();
                    dictMods_[mod].Add(new AcEntity { Arity = arity, FunctionName = func, ModuleName = mod });
                }
                prc.WaitForExit(30000);
            }
        }

        public void ScanBin(ErlangEditor.Core.Entity.ApplicationEntity aEntity)
        {
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var lstBeam = new List<string>();
            foreach (var j in new ErlangEditor.Core.Entity.ApplicationEntity[] { aEntity })
            {
                var ebin = j.Folders.First(i => i.Name == "ebin");
                var ebinpath = ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(ebin);
                foreach (var i in j.Folders.First(x => x.Name == "src").Files)
                {
                    var filepath = System.IO.Path.Combine(ebinpath, i.DisplayName + ".beam");
                    if (System.IO.File.Exists(filepath))
                    {
                        lstBeam.Add(i.DisplayName);
                        dictMods_.Remove(i.DisplayName);
                    }
                }
            }
            if (lstBeam.Count > 0)
            {
                var lst = string.Format("[{0}]", string.Join(",", lstBeam));
                var prc = new Process();
                prc.StartInfo = new ProcessStartInfo();
                prc.StartInfo.CreateNoWindow = true;
                prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
                prc.StartInfo.Arguments = string.Format("-noshell -eval \"" +
                    "B = fun(X)-> lists:map(fun({{A,B}})-> io:format(\\\"~p,~p,~p~n\\\",[X,A,B]) end, X:module_info(exports)) end," +
                    "lists:map(fun(X)-> B(X) end, {0})," +
                    "init:stop().\" {1}", lst, " -pa " + System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(aEntity) + "\\ebin"));
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.WorkingDirectory = ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath;
                prc.StartInfo.RedirectStandardOutput = true;
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.EnableRaisingEvents = true;
                prc.Start();
                var result = prc.StandardOutput.ReadToEnd();
                var lines = result.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));
                foreach (var i in lines)
                {
                    var args = i.Split(',');
                    var mod = args[0];
                    var func = args[1];
                    var arity = args[2];
                    if (!dictMods_.ContainsKey(mod))
                        dictMods_[mod] = new List<AcEntity>();
                    dictMods_[mod].Add(new AcEntity { Arity = arity, FunctionName = func, ModuleName = mod });
                }
                prc.WaitForExit(30000);
            }
        }

        public void DropModule(string aModName)
        {
            dictMods_.Remove(aModName);
        }

        public void DropApplication(ErlangEditor.Core.Entity.ApplicationEntity aEntity)
        {
            foreach (var i in aEntity.Folders.First(x => x.Name == "src").Files)
            {
                dictMods_.Remove(i.DisplayName);
            }
        }

        public void CacheLib()
        {
            Dictionary<string, List<AcEntity>> dict = new Dictionary<string, List<AcEntity>>();
            if (string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                string.IsNullOrWhiteSpace(ErlangEditor.Core.ConfigUtil.Config.ShellPath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ConsolePath) ||
                !System.IO.File.Exists(ErlangEditor.Core.ConfigUtil.Config.ShellPath))
                throw new Exception("Erlang shell设置有误，请在\"设置\"页设置好shell路径。");
            var libPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName( ErlangEditor.Core.ConfigUtil.Config.CompilerPath), "..\\lib");
            var dirs = System.IO.Directory.EnumerateDirectories(libPath).Select(x=>System.IO.Path.GetFullPath(x));
            int a = 0;
            foreach (var i in dirs)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("cache {0},process {1}/{2}",i,++a ,dirs.Count() ));
                var lstBeam = new List<string>();
                var ebinPath = System.IO.Path.Combine(i, "ebin");
                if (!System.IO.Directory.Exists(ebinPath))
                    continue;
                foreach (var j in System.IO.Directory.EnumerateFiles(ebinPath))
                {
                    if (System.IO.Path.GetExtension(j).ToLower() == ".beam")
                    {
                        lstBeam.Add(System.IO.Path.GetFileNameWithoutExtension(j));
                    }
                }
                if (lstBeam.Count > 0)
                {
                    var lst = string.Format("[{0}]", string.Join(",", lstBeam));
                    var prc = new Process();
                    prc.StartInfo = new ProcessStartInfo();
                    prc.StartInfo.CreateNoWindow = true;
                    prc.StartInfo.FileName = ErlangEditor.Core.ConfigUtil.Config.ConsolePath;
                    prc.StartInfo.Arguments = string.Format("-noshell -eval \"" +
                        "B = fun(X)-> lists:map(fun({{A,B}})-> io:format(\\\"~p,~p,~p~n\\\",[X,A,B]) end, X:module_info(exports)) end," +
                        "lists:map(fun(X)-> B(X) end, {0})," +
                        "init:stop().\" {1}", lst, " -pa " + System.IO.Path.Combine(i, "\\ebin"));
                    prc.StartInfo.UseShellExecute = false;
                    prc.StartInfo.WorkingDirectory = ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath;
                    prc.StartInfo.RedirectStandardOutput = true;
                    prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    prc.EnableRaisingEvents = true;
                    prc.Start();
                    var result = prc.StandardOutput.ReadToEnd();
                    if (result.Contains("init terminating in do_boot"))
                        continue;
                    var lines = result.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));
                    foreach (var k in lines)
                    {
                        var args = k.Split(',');
                        var mod = args[0];
                        var func = args[1];
                        var arity = args[2];
                        if (!dict.ContainsKey(mod))
                            dict[mod] = new List<AcEntity>();
                        dict[mod].Add(new AcEntity { Arity = arity, FunctionName = func, ModuleName = mod });
                    }
                    prc.WaitForExit(30000);
                }
            }
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            ErlangEditor.Core.ConfigUtil.Config.AutoCompleteCache = json;
            ErlangEditor.Core.ConfigUtil.SaveConfig();
            dictLibCache_ = dict;
        }
    }
}
