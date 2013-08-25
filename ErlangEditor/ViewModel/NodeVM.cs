using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }

        public NodeVM(ErlangEditor.Core.Entity.NodeEntity aEntity)
        {
            Entity = aEntity;
            if (aEntity != null)
            {
                Name = aEntity.NodeName;
            }
            Proxy = new RunProxy.SolutionRunner();
            Proxy.Closed += (a, b) => { State = false; };
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
    }
}
