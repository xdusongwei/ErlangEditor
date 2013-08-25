using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class SolutionEntity
    {
        public SolutionEntity()
        {
            Name = BasePath = string.Empty;
            Apps = new List<ApplicationEntity>();
            Nodes = new List<NodeEntity>();
        }

        public string Name
        {
            get;
            set;
        }

        public string BasePath
        {
            get;
            set;
        }

        public List<ApplicationEntity> Apps
        {
            get;
            set;
        }

        public List<NodeEntity> Nodes
        {
            get;
            set;
        }
    }
}
