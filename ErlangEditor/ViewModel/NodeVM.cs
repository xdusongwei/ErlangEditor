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
        }

        public NodeVM(ErlangEditor.Core.Entity.NodeEntity aEntity)
        {
            Entity = aEntity;
            if (aEntity != null)
            {
                Name = aEntity.NodeName;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public bool State
        {
            get;
            set;
        }

        private ErlangEditor.Core.Entity.NodeEntity Entity
        {
            get;
            set;
        }
    }
}
