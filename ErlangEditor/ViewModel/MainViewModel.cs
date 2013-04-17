using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ErlangEditor.Core;
using ErlangEditor.Template;
using System.Collections.ObjectModel;

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

        private void NotifyPropertyChanged(string aName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(aName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
