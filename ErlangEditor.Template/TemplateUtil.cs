using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template
{
    public static class TemplateUtil
    {
        public static string MakeAppFile(string aVsn, string aAppName, string aDescription, IEnumerable<string> aModules, IEnumerable<string> aRegistered, IEnumerable<string> aApplications,
            string aMod, string aArgs)
        {
            return new Templates.AppFile(aVsn, aAppName, aDescription, aModules, aRegistered, aApplications, aMod, aArgs).TransformText();
        }

        public static string MakeApplicationCode(string aModuleName, string aStartupMFA)
        {
            return new Templates.application(aModuleName, aStartupMFA).TransformText();
        }


        public static string MakeErlangCode(string aModuleName)
        {
            return new Templates.ErlangCode(aModuleName).TransformText();
        }

        public static string MakeHeaderCode()
        {
            return new Templates.HeaderCode().TransformText();
        }

        public static string Make_gen_server(string aModuleName)
        {
            return new Templates.gen_server(aModuleName).TransformText();
        }

        public static string Make_application(string aModuleName, string aMFA)
        {
            return new Templates.application(aModuleName, aMFA).TransformText();
        }
    }
}
