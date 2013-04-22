﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ErlangEditor.Core;
using ErlangEditor.Core.Entity;
using ErlangEditor.Template;
using ErlangEditor.ViewModel.ContextMenu;
using ErlangEditor.ViewModel.ContextMenuMaker;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;

namespace ErlangEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region All about load
        public bool Loaded
        {
            get;
            private set;
        }

        public void Load()
        {
            Solution = new Solution();
            Loaded = true;
        }

        #endregion

        #region  解决方案的操作
        public void CreateSolution(
            string aName,
            string aPath,
            string aCompilerPath,
            string aShellPath
            )
        {
            var macro = new StdProcessTemplate(aName, true, string.Empty, string.Empty , true).Macro;
            CurrentSolutions.Clear();
            CurrentSolutions.Add(new SolutionVM(Solution.CreateSolution(aName, aPath, aCompilerPath, aShellPath, macro, TemplateConstant.StdModuleTemplateFilePath)));
        }

        public void SaveSolution()
        {
            if (CurrentSolutions.Count > 0)
            {
                Solution.SaveSolution(CurrentSolution.Entity);
                IsModified = false;
            }
        }

        public void SaveSolutionFile()
        {
            if (CurrentSolutions.Count > 0)
            {
                Solution.SaveSolutionFile(CurrentSolution.Entity);
                IsModified = false;
            }
        }

        public void LoadSolution(string aPath)
        {
            CurrentSolutions.Clear();
            CurrentSolutions.Add(new SolutionVM(Solution.LoadSolution(aPath)));
            IsModified = false;
        }

        public void CloseSolution()
        {
            CurrentSolutions.Clear();
        }

        #endregion

        #region Property
        private ObservableCollection<SolutionVM> currentSolutions_ = new ObservableCollection<SolutionVM>();
        public ObservableCollection<SolutionVM> CurrentSolutions
        {
            get { return currentSolutions_; }
            set
            {
                currentSolutions_ = value;
                NotifyPropertyChanged("CurrentSolutions");
            }
        }

        public SolutionVM CurrentSolution
        {
            get {  return currentSolutions_.DefaultIfEmpty(new SolutionVM()).First(); }
        }

        public bool IsModified
        {
            get;
            set;
        }

        public Solution Solution
        {
            get;
            private set;
        }

        private ObservableCollection<RadMenuItem> contextOperations_ = new ObservableCollection<RadMenuItem>();
        public ObservableCollection<RadMenuItem> ContextOperations
        {
            get
            {
                return contextOperations_;
            }
            private set
            {
                if (contextOperations_ != value)
                {
                    contextOperations_ = value;
                    NotifyPropertyChanged("ContextOperations");
                }
            }
        }

        //下面这三个属性是上下文菜单有关的
        public object SelectVMItem
        {
            get;
            private set;
        }

        public RadTreeViewItem SelectItem
        {
            get;
            private set;
        }

        public ContextOperationTypeEnum ContextOperation
        {
            get;
            set;
        }
        #endregion

        #region 文件的操作
        public void OpenFile(ItemVM aItemVM)
        {

        }

        public void CloseFile(ItemVM aItemVM)
        {

        }

        public void SaveFile(ItemVM aItemVM)
        {

        }
        #endregion
        public void CommitItemAddOrUpdate(object aVM , string aNewItemName)
        {
            if (aVM is SolutionVM)
            {

            }
            else if (aVM is ProjectVM)
            {

            }
            else if (aVM is ItemVM)
            {
                if ((aVM as ItemVM).IsFolder)
                {
                    if (ContextOperation == ContextOperationTypeEnum.Add)
                        NewFolder.Commit(aVM, aNewItemName);
                    else
                        RenameFolder.Commit(aVM, aNewItemName);
                }
                else
                {
                    if (ContextOperation == ContextOperationTypeEnum.Rename)
                        RenameFile.Commit(aVM, aNewItemName);
                }
            }
            
        }

        public void UpdateContextOperationMenu(RadTreeViewItem aSelectItem)
        {
            if (aSelectItem == null)
            {
                contextOperations_.Clear();
                return;
            }
            SelectVMItem = aSelectItem.Item;
            SelectItem = aSelectItem;
            
            if (SelectVMItem is SolutionVM)
            {
                SolutionContextMenu.MakeMenu(contextOperations_);
            }
            else if (SelectVMItem is ProjectVM)
            {
                ProjectContextMenu.MakeMenu(contextOperations_);
            }
            else if (SelectVMItem is ItemVM)
            {
                ItemContextMenu.MakeMenu(contextOperations_);
            }
            else
            {
                contextOperations_.Clear();
            }
        }

        public string GetVMFilePath(object aVM)
        {
            dynamic dyVM = aVM;
            return Solution.GetFullPath(dyVM.Entity);
        }

        #region Enum Types
        public enum ContextOperationTypeEnum { Add, Rename };
        #endregion

        #region All about INotifyPropertyChanged
        private void NotifyPropertyChanged(string aName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(aName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
#endregion
    }
}
