using System.Linq;
using System.Text;
using System.Collections.Generic;
using System;
namespace ErlangEditor.Template.Templates
{
    public partial class AppFile
    {
        public AppFile()
            : this("0.0.0", "no_name", string.Empty ,new string[0], new string[0], new string[0], "no_mod", "[]")
        {

        }

        public AppFile(string aVsn, string aAppName, string aDescription, IEnumerable<string> aModules, IEnumerable<string> aRegistered, IEnumerable<string> aApplications,
            string aMod, string aArgs)
        {
            Vsn = aVsn;
            AppName = aAppName;
            Description = aDescription;
            Modules = aModules;
            Registered = aRegistered;
            Applications = aApplications;
            Mod = aMod;
            Args = aArgs;
        }

        public string Vsn
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public IEnumerable<string> Modules
        {
            get;
            set;
        }

        public IEnumerable<string> Registered
        {
            get;
            set;
        }

        public IEnumerable<string> Applications
        {
            get;
            set;
        }

        public string Mod
        {
            get;
            set;
        }

        public string Args
        {
            get;
            set;
        }
    }
}