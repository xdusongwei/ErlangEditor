using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Template
{
    public class StdProcessTemplate
    {
        private static readonly string constantModuleNameMacro = "%_MODULE_NAME_%";
        private static readonly string constantExportAllMacro = "%_EXPORTALL_%";
        private static readonly string constantExportMacro = "%_EXPORT_%";
        private static readonly string constantImportMacro = "%_IMPORT_%";

        private Dictionary<string, string> macro_ = new Dictionary<string, string>();

        public Dictionary<string, string> Macro
        {
            get
            { return macro_; }
        }

        public StdProcessTemplate(string aModuleName, bool aIsExportAll, string aImport, string aExport ,bool aIsModule)
        {
            if (aIsExportAll)
            {
                macro_.Add(constantExportAllMacro, "-compile(export_all).");
                macro_.Add(constantExportMacro, string.Empty);
            }
            else
            {
                macro_.Add(constantExportAllMacro, string.Empty);
                macro_.Add(constantExportMacro, aExport);
            }
            if (aIsModule)
            {
                macro_.Add(constantModuleNameMacro, string.Format("-module({0}).", aModuleName));
            }
            else
            {
                macro_.Add(constantModuleNameMacro, string.Empty);
            }
            macro_.Add(constantImportMacro, aImport);
        }
    }
}
