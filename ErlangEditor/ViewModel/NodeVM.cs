using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class NodeVM : ViewModelBase
    {
        public NodeVM()
        {
            Entity = new Core.Entity.NodeEntity();
            Name = "NoName";
            Proxy = new RunProxy.SolutionRunner();
            Proxy.Closed += (a, b) => { State = false; };
            AppNames = new ObservableCollection<string>();
        }

        public NodeVM(ErlangEditor.Core.Entity.NodeEntity aEntity)
        {
            Entity = aEntity;
            if (aEntity != null)
            {
                Name = aEntity.NodeName;
            }
            Proxy = new RunProxy.SolutionRunner();
            Proxy.Closed += (a, b) => { State = false; ErlangEditor.Core.NodeUtil.StopNode(aEntity.NodeName); };
            AppNames = new ObservableCollection<string>();
            foreach (var i in aEntity.Apps)
                AppNames.Add(i);
            Proxy.NewOutput += (a, b) => { Dispatcher.Invoke(new Action(() => { App.MainViewModel.Output.Add(new OutputVM { NodeName = b.NodeName, Info = b.Data }); })); };
        }

        public string Name
        {
            get;
            set;
        }

        private bool state_;
        public bool State
        {
            get
            { return state_; }
            set
            {
                state_ = value;
                OnPropertyChanged("State");
            }
        }

        public ErlangEditor.Core.Entity.NodeEntity Entity
        {
            get;
            set;
        }

        public ErlangEditor.RunProxy.SolutionRunner Proxy
        {
            get;
            set;
        }

        public bool ShowShell
        {
            get { return Entity == null ? false : Entity.ShowShell; }
            set { if (Entity != null) Entity.ShowShell = value; OnPropertyChanged("ShowShell"); }
        }

        public ObservableCollection<string> AppNames
        {
            get { return (ObservableCollection<string>)GetValue(AppNamesProperty); }
            set { SetValue(AppNamesProperty, value); OnPropertyChanged("AppNames"); }
        }

        // Using a DependencyProperty as the backing store for AppNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AppNamesProperty =
            DependencyProperty.Register("AppNames", typeof(ObservableCollection<string>), typeof(NodeVM), new PropertyMetadata(new ObservableCollection<string>()));

        
        //public ObservableCollection<string> AppNames
        //{
        //    get;
        //    set;
        //}
    }
}
