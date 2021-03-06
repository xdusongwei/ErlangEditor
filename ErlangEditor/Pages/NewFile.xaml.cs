﻿using Microsoft.Win32;
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

namespace ErlangEditor.Pages
{
    /// <summary>
    /// NewFile.xaml 的交互逻辑
    /// </summary>
    public partial class NewFile : UserControl
    {
        public NewFile(ViewModel.PrjTreeItemVM aVM)
        {
            InitializeComponent();
            vm_ = aVM;
        }

        private ViewModel.PrjTreeItemVM vm_;

        public string Title
        {
            get
            {
                return "创建文件";
            }
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
        }

        private void SortChildren(ViewModel.PrjTreeItemVM node)
        {
            var comparer = new Tools.Reverser<ViewModel.PrjTreeItemVM>(new ViewModel.PrjTreeItemVM().GetType(), "DisplayText", Tools.ReverserInfo.Direction.ASC);
            var lst = node.Children.ToList();
            lst.Sort(comparer);
            node.Children.Clear();
            foreach (var i in lst)
                node.Children.Add(i);
        }

        private void ErlClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            try
            {
                var fld = App.Entity.FindFolderName(vm_.Entity);
                var app = App.Entity.FindAppName(vm_.Entity);
                var entity = new ErlangEditor.Core.Entity.FileEntity { Name = name + ".erl", DisplayName = name };
                var content = ErlangEditor.Template.TemplateUtil.MakeErlangCode(name);
                ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                ErlangEditor.Core.SolutionUtil.SaveSolution();
                vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                SortChildren(vm_);
                App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "创建文件");
            }
        }

        private void HrlClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            try
            {
                var fld = App.Entity.FindFolderName(vm_.Entity);
                var app = App.Entity.FindAppName(vm_.Entity);
                var entity = new ErlangEditor.Core.Entity.FileEntity { Name = name + ".hrl", DisplayName = name };
                var content = ErlangEditor.Template.TemplateUtil.MakeHeaderCode();
                ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                ErlangEditor.Core.SolutionUtil.SaveSolution();
                vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                SortChildren(vm_);
                App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "创建文件");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(vm_.Entity is ErlangEditor.Core.Entity.ApplicationEntity)
                tbTitle.Text = string.Format("创建文件到 {0}" , vm_.DisplayText);
            if(vm_.Entity is ErlangEditor.Core.Entity.FolderEntity )
                tbTitle.Text = string.Format("创建文件到 {0}/{1}" , ((vm_.Entity as ErlangEditor.Core.Entity.FolderEntity).GetParent() as ErlangEditor.Core.Entity.ApplicationEntity).Name,
                    (vm_.Entity as ErlangEditor.Core.Entity.FolderEntity).Name);
        }

        private void AppClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            App.Navigation.GoFroward(new NewFile_app(name, vm_));
        }

        private void Gen_serverClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            try
            {
                var fld = App.Entity.FindFolderName(vm_.Entity);
                var app = App.Entity.FindAppName(vm_.Entity);
                var entity = new ErlangEditor.Core.Entity.FileEntity { Name = name + ".erl", DisplayName = name };
                var content = ErlangEditor.Template.TemplateUtil.Make_gen_server(name);
                ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                ErlangEditor.Core.SolutionUtil.SaveSolution();
                vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                SortChildren(vm_);
                App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "创建文件");
            }
        }

        private void Gen_eventClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            try
            {
                var fld = App.Entity.FindFolderName(vm_.Entity);
                var app = App.Entity.FindAppName(vm_.Entity);
                var entity = new ErlangEditor.Core.Entity.FileEntity { Name = name + ".erl", DisplayName = name };
                var content = ErlangEditor.Template.TemplateUtil.Make_gen_event(name);
                ErlangEditor.Core.FileUtil.AddFile(app, fld, entity, content);
                ErlangEditor.Core.SolutionUtil.SaveSolution();
                vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                SortChildren(vm_);
                App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
            }
            catch (Exception ecp)
            {
                App.Navigation.ShowMessageBox(ecp.Message, "创建文件");
            }
        }

        private void Gen_supervisorClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            App.Navigation.GoFroward(new NewFile_supervisor(name , vm_));
        }

        private void Gen_appClicked(object sender, RoutedEventArgs e)
        {
            var name = tbName.Text.Trim();
            if (string.IsNullOrEmpty(name) || vm_ == null || vm_.Entity == null)
                return;
            App.Navigation.GoFroward(new NewFile_application(name,vm_));
        }

        private void ExistsFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "选择现有文件";
            fileDialog.Filter = "All Files (*.*)|*.*";
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    var fld = App.Entity.FindFolderName(vm_.Entity);
                    var app = App.Entity.FindAppName(vm_.Entity);
                    foreach (var i in fileDialog.FileNames)
                    {
                        var name = System.IO.Path.GetFileName(i);
                        var displayname = System.IO.Path.GetFileNameWithoutExtension(i);
                        var newpath  = System.IO.Path.Combine(ErlangEditor.Core.Helper.EntityTreeUtil.GetPath(vm_.Entity), name);
                        if(i.ToLower() != newpath.ToLower())
                            System.IO.File.Copy(i, newpath);
                        ErlangEditor.Core.Entity.FileEntity entity = null;
                        if(System.IO.Path.GetExtension(newpath).ToLower() == ".src")
                            entity = new ErlangEditor.Core.Entity.FileEntity { Name = name, DisplayName = displayname , IsAppFile = true };
                        else
                            entity = new ErlangEditor.Core.Entity.FileEntity { Name = name, DisplayName = displayname };
                        ErlangEditor.Core.FileUtil.AddFile(app, fld, entity);
                        ErlangEditor.Core.SolutionUtil.SaveSolution();
                        vm_.Children.Add(new ViewModel.PrjTreeItemVM(entity));
                        App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                    }
                    SortChildren(vm_);
                }
                catch (Exception ecp)
                {
                    App.Navigation.ShowMessageBox(ecp.Message, "选择现有文件");
                }
                //App.ViewModel.LoadSolution(file);
            }
        }
    }
}
