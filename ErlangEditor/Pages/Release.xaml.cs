using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace ErlangEditor.Pages
{
    /// <summary>
    /// Release.xaml 的交互逻辑
    /// </summary>
    public partial class Release : UserControl
    {
        public Release()
        {
            InitializeComponent();
        }

        System.Collections.ObjectModel.ObservableCollection<ViewModel.CheckableApplicationVM> appList_;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ErlangEditor.Core.SolutionUtil.Solution == null)
            {
                btnStart.IsEnabled = false;
                return;
            }
            etRel.Text = ErlangEditor.Core.SolutionUtil.Solution.RelContent;
            etConfig.Text = ErlangEditor.Core.SolutionUtil.Solution.ConfigContent;
            var apps = ErlangEditor.Core.SolutionUtil.Solution.Apps;
            lstApps.ItemsSource = appList_ =new System.Collections.ObjectModel.ObservableCollection<ViewModel.CheckableApplicationVM>( apps.Select(x => new ViewModel.CheckableApplicationVM { Name = x.Name , Entity = x }));        }

        public string Title
        {
            get
            {
                return "发布";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.ToolBox.HideButtomBar();
        }

        private bool local_;
        private bool dontCompile_;
        private bool tar_;
        private bool source_;
        private bool open_;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            local_ = cbLocal.IsChecked ?? false;
            dontCompile_ = cbNoCompile.IsChecked ?? false;
            tar_ = cbTar.IsChecked ?? false;
            source_ = cbSource.IsChecked ?? false;
            open_ = cbOpen.IsChecked ?? false;
            lstCopy_.Clear();
            btnStart.IsEnabled = false;
            pb.Value = 0;
            var apps = new List<ErlangEditor.Core.Entity.ApplicationEntity>();
            ErlangEditor.Core.Entity.ApplicationEntity start = null;
            pb.Maximum = CalcProcess(ref apps);
            if (pb.Maximum != 0)
            {
                ErlangEditor.Core.SolutionUtil.Solution.RelContent = etRel.Text;
                ErlangEditor.Core.SolutionUtil.Solution.ConfigContent = etConfig.Text;
                ErlangEditor.Core.SolutionUtil.SaveSolution();
                var th = new Thread(BackgroundProc);
                th.IsBackground = true;
                th.Start(new Tuple<IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity>, ErlangEditor.Core.Entity.ApplicationEntity>(apps, start));
            }
            else
            {
                btnStart.IsEnabled = true;
            }
        }

        private void BackgroundProc(object arg)
        {
            var tup = arg as Tuple<IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity>, ErlangEditor.Core.Entity.ApplicationEntity>;
            var apps = tup.Item1;
            var start = tup.Item2;
            try
            {
                ClearLibFolder();
                CompileSource(apps);
                MakeLibFolder(apps);
                Copy();
                MakeScript(apps);
                MakeTar(apps);
                Open();
            }
            catch(Exception e) 
            {
                Dispatcher.Invoke(new Action(() => { App.Navigation.ShowMessageBox(e.Message,"发布遇到问题"); }));
            }
            finally
            {
                Dispatcher.Invoke(new Action(() => { btnStart.IsEnabled = true; }));
            }
        }

        private int CalcProcess(ref List<ErlangEditor.Core.Entity.ApplicationEntity> aApps)
        {
            var apps = appList_.Where(x => x.IsChecked == true).Select(x => x.Entity);
            aApps.Clear();
            aApps.AddRange(apps);
            int compileObjs =0;
            foreach (var i in apps)
                foreach (var j in i.Folders.First(x => x.Name == "src").Files)
                    if (System.IO.Path.GetExtension(j.Name).ToLower() == ".erl" || System.IO.Path.GetExtension(j.Name).ToLower() == ".src")
                    {
                        compileObjs++;
                    }
            int result = 10 + compileObjs + 10 + 10 + 10 + 10;
            //clear +  compileObj + make_dir + copy + make_script + pack
            return result;
        }

        private void ClearLibFolder()
        {
            var libPath = System.IO.Path.Combine( ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
            if (System.IO.Directory.Exists(libPath))
                System.IO.Directory.Delete(libPath, true);
            Dispatcher.Invoke(new Action(() => { pb.Value += 10; Debug.WriteLine(pb.Value); }));
        }

        private void MakeLibFolder(IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity> aApps)
        {
            var libPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
            System.IO.Directory.CreateDirectory(libPath);
            foreach (var i in aApps)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(libPath, i.Name));
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(libPath, i.Name, "ebin"));
                if (source_)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(libPath, i.Name, "src"));
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(libPath, i.Name, "include"));
                }
            }
            Dispatcher.Invoke(new Action(() => { pb.Value += 10; Debug.WriteLine(pb.Value); }));
        }

        private void CompileSource(IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity> aApps)
        {
            var libPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
            
            if (dontCompile_)
            {
                foreach (var i in aApps)
                {
                    var appPath = ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(i);
                    foreach (var j in i.Folders.First(x => x.Name == "src").Files)
                    {
                        if (j.IsAppFile)
                        {
                            var souPath = System.IO.Path.Combine(appPath, "ebin", j.DisplayName);
                            var tarPath = System.IO.Path.Combine(libPath, i.Name, "ebin", j.DisplayName);
                            lstCopy_.Add(new Tuple<string, string>(souPath, tarPath));
                        }
                        else
                        {
                            var souPath = System.IO.Path.Combine(appPath, "ebin", j.DisplayName + ".beam");
                            var tarPath = System.IO.Path.Combine(libPath, i.Name, "ebin", j.DisplayName + ".beam");
                            lstCopy_.Add(new Tuple<string, string>(souPath, tarPath));
                        }
                        if (source_)
                        {
                            var souPath = System.IO.Path.Combine(appPath, "src", j.Name);
                            var tarPath = System.IO.Path.Combine(libPath, i.Name, "src", j.Name);
                            lstCopy_.Add(new Tuple<string, string>(souPath, tarPath));
                        }
                        Dispatcher.Invoke(new Action(() => { pb.Value++; Debug.WriteLine(pb.Value); }));
                    }
                    if (source_)
                    {
                        foreach (var j in i.Folders.First(x => x.Name == "include").Files)
                        {
                            var souPath = System.IO.Path.Combine(appPath, "include", j.Name);
                            var tarPath = System.IO.Path.Combine(libPath, i.Name, "include", j.Name);
                            lstCopy_.Add(new Tuple<string, string>(souPath, tarPath));
                        }
                    }
                }
            }
            else
            {
                var cp = new CompilerProxy.SolutionCompiler();
                cp.Start(aApps, null, (x) => { Dispatcher.Invoke(new Action(() => { FileCompiled(x); })); }, (x) => { if (!x) throw new Exception("应用中有模块编译失败。"); }, false);
            }
        }


        private void Copy()
        {
            var relPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name , ErlangEditor.Core.SolutionUtil.Solution.Name + ".rel");
            var configPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name, ErlangEditor.Core.SolutionUtil.Solution.Name + ".config");
            var batPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name, ErlangEditor.Core.SolutionUtil.Solution.Name + ".bat");
            foreach (var i in lstCopy_)
                System.IO.File.Copy(i.Item1, i.Item2, true);
            using (var ws = new StreamWriter(relPath))
            {
                ws.Write(ErlangEditor.Core.SolutionUtil.Solution.RelContent);
                ws.Flush();
            }
            using (var ws = new StreamWriter(configPath))
            {
                ws.Write(ErlangEditor.Core.SolutionUtil.Solution.ConfigContent);
                ws.Flush();
            }
            using (var ws = new StreamWriter(batPath))
            {
                var batcontent = string.Format("\"{0}\" -boot {1} -config {1}", ErlangEditor.Core.ConfigUtil.Config.ShellPath, ErlangEditor.Core.SolutionUtil.Solution.Name);
                ws.Write(batcontent);
                ws.Flush();
            }
            Dispatcher.Invoke(new Action(() => { pb.Value += 10; Debug.WriteLine(pb.Value); }));
        }

        public void MakeScript(IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity> aApps)
        {
            var rp = new ErlangEditor.ReleaseProxy.ReleaseCore();
            rp.MakeScript(aApps, local_);
            Dispatcher.Invoke(new Action(() => { pb.Value += 10; Debug.WriteLine(pb.Value); }));
        }

        private void MakeTar(IEnumerable<ErlangEditor.Core.Entity.ApplicationEntity> aApps)
        {
            if (tar_)
            {
                var libPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
                var rp = new ErlangEditor.ReleaseProxy.ReleaseCore();
                rp.MakeTar(libPath,aApps);
            }
            Dispatcher.Invoke(new Action(() => { pb.Value += 10; Debug.WriteLine(pb.Value); }));
        }

        private void Open()
        {
            if (open_)
            {
                Process prc = new Process();
                prc.StartInfo.FileName = "explorer.exe";
                if (tar_)
                {
                    var tarPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name,ErlangEditor.Core.SolutionUtil.Solution.Name +".tar.gz" );
                    prc.StartInfo.Arguments = string.Format("/select,{0}", tarPath);
                }
                else
                {
                    var tarPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
                    prc.StartInfo.Arguments = tarPath;
                }
                prc.Start();
            }
            else
            {
                Dispatcher.Invoke(new Action(() => { App.Navigation.ShowMessageBox("应用已经成功生成！", "发布应用"); }));
            }
        }

        private List<Tuple<string, string>> lstCopy_ = new List<Tuple<string, string>>();

        private void FileCompiled(ErlangEditor.Core.Entity.FileEntity aEntity)
        {
            var libPath = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetBasePath, ErlangEditor.Core.SolutionUtil.Solution.Name);
            var src = ErlangEditor.Core.Helper.EntityTreeUtil.GetParent(aEntity);
            var app = ErlangEditor.Core.Helper.EntityTreeUtil.GetParent(src) as ErlangEditor.Core.Entity.ApplicationEntity;
            var appPath = ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(app);
            
            if (aEntity.IsAppFile)
            {
                var souPath = System.IO.Path.Combine(appPath, "ebin", aEntity.DisplayName);
                var tarPath = System.IO.Path.Combine(libPath, app.Name, "ebin", aEntity.DisplayName);
                lstCopy_.Add(new Tuple<string,string>(souPath, tarPath));
            }
            else
            {
                var souPath = System.IO.Path.Combine(appPath, "ebin", aEntity.DisplayName + ".beam");
                var tarPath = System.IO.Path.Combine(libPath, app.Name, "ebin", aEntity.DisplayName + ".beam");
                lstCopy_.Add(new Tuple<string,string>(souPath, tarPath));
            }
            pb.Value++;
            Debug.WriteLine(pb.Value);
        }
    }
}
