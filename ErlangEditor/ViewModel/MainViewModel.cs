using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ErlangEditor.Core;
using ErlangEditor.Template;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Telerik.Windows.Controls.Navigation;
using Telerik.Windows.Controls;
using ErlangEditor.ViewModel.ContextMenu;

namespace ErlangEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
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

        public void CreateSolution(
            string aName,
            string aPath,
            string aCompilerPath,
            string aShellPath
            )
        {
            var macro = new StdProcessTemplate(aName, true, string.Empty, string.Empty).Macro;
            currentSolution_.Clear();
            CurrentSolution.Add(new SolutionVM(Solution.CreateSolution(aName, aPath, aCompilerPath, aShellPath, macro, "Template\\module.erl")));
        }

        public void SaveSolution()
        {
            if(CurrentSolution.Count > 0)
                Solution.SaveSolution(CurrentSolution.First().Entity);
        }

        public void LoadSolution(string aPath)
        {
            currentSolution_.Clear();
            CurrentSolution.Add(new SolutionVM(Solution.LoadSolution(aPath)));
            IsModified = false;
        }

        public void CloseSolution()
        {
            CurrentSolution = null;
        }

        private ObservableCollection<SolutionVM> currentSolution_ = new ObservableCollection<SolutionVM>();
        public ObservableCollection<SolutionVM> CurrentSolution
        {
            get { return currentSolution_; }
            set
            {
                currentSolution_ = value;
                NotifyPropertyChanged("CurrentSolution");
            }
        }

        public bool IsModified
        {
            get;
            set;
        }

        private Solution Solution
        {
            get;
            set;
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

        public void UpdateContextOperationMenu(object aSelectItem)
        {
            SelectVMItem = aSelectItem;
            contextOperations_.Clear();
            if (aSelectItem is SolutionVM)
            {
                Debug.WriteLine("sln!");
                contextOperations_.Add(new RadMenuItem { Header = "添加新项目" });
                contextOperations_.Add(new RadMenuItem { Header = "排除" });
            }
            else if (aSelectItem is ProjectVM)
            {
                Debug.WriteLine("prj!");
                var newItem = new RadMenuItem { Header = "添加新Erlang代码文件" };
                var newItem2 = new RadMenuItem { Header = "添加新Hrl代码文件" };
                var newItem3 = new RadMenuItem { Header = "添加新的其他文件" };
                var existItem = new RadMenuItem { Header = "添加现有Erlang代码文件" };
                var existItem2 = new RadMenuItem { Header = "添加现有Hrl代码文件" };
                var existItem3 = new RadMenuItem { Header = "添加现有文件" };
                var folderItem = new RadMenuItem { Header = "新建文件夹"};
                folderItem.Click += NewFolder.Click;
                var addChildren = new RadMenuItem[] { newItem, newItem2, newItem3, existItem, existItem2, existItem3, folderItem };
                contextOperations_.Add(new RadMenuItem { Header = "添加", ItemsSource = new ObservableCollection<RadMenuItem>(addChildren) });
                contextOperations_.Add(new RadMenuItem { Header = "重命名" });
                contextOperations_.Add(new RadMenuItem { Header = "删除" });
            }
            else if (aSelectItem is ItemVM)
            {
                Debug.WriteLine("itm!");
                if ((aSelectItem as ItemVM).IsFolder)
                {
                    var newItem = new RadMenuItem { Header = "添加新Erlang代码文件" };
                    var newItem2 = new RadMenuItem { Header = "添加新Hrl代码文件" };
                    var newItem3 = new RadMenuItem { Header = "添加新的其他文件" };
                    var existItem = new RadMenuItem { Header = "添加现有Erlang代码文件" };
                    var existItem2 = new RadMenuItem { Header = "添加现有Hrl代码文件" };
                    var existItem3 = new RadMenuItem { Header = "添加现有文件" };
                    var folderItem = new RadMenuItem { Header = "新建文件夹" };
                    var addChildren = new RadMenuItem[] { newItem, newItem2, newItem3, existItem, existItem2, existItem3, folderItem };
                    contextOperations_.Add(new RadMenuItem { Header = "添加", ItemsSource = new ObservableCollection<RadMenuItem>(addChildren) });
                }
                contextOperations_.Add(new RadMenuItem { Header = "重命名" });
                contextOperations_.Add(new RadMenuItem { Header = "排除" });
                contextOperations_.Add(new RadMenuItem { Header = "删除" });

            }
        }

        public object SelectVMItem
        {
            get;
            private set;
        }

        private void NotifyPropertyChanged(string aName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(aName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
