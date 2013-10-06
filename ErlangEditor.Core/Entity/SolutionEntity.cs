using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class SolutionEntity
    {
        public SolutionEntity(string aName)
        {
            Name = aName;
            Apps = new List<ApplicationEntity>();
            Nodes = new List<NodeEntity>();
            RelContent = "{ release ,\n{ \"{0}\" , \"vsn\"},\n{ erts , \"vsn\" },\n[\n{kernel,\"vsn\"},\n{stdlib,\"vsn\"},\n{sasl,\"vsn\"},\n{mnesia,\"vsn\"}\n]\n}.".Replace("{0}",aName);
            ConfigContent = "[].";
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

        //use for release
        public string RelContent
        {
            get;
            set;
        }

        public string ConfigContent
        {
            get;
            set;
        }
    }
}
