using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Helper
{
    public static class EntityTreeUtil
    {
        public static string GetPath(object aNode)
        {
            var x = aNode;
            var ret = string.Empty;
            while (GetParent(x) != null)
            {
                ret = string.IsNullOrWhiteSpace(ret)? (x as dynamic).Name.Trim() : ((x as dynamic).Name + "\\" + ret).Trim();
                x = GetParent(x);
            }
            return System.IO.Path.Combine(GetBasePath, ret);
        }

        public static object GetParent(object aChild)
        {
            if (dictTree_.ContainsKey(aChild))
                return dictTree_[aChild];
            return null;
        }

        private static string slnBasePath_ = string.Empty;
        public static string GetBasePath
        {
            get { return slnBasePath_; }
            internal set { slnBasePath_ = value; }
        }

        private static string slnFilePath_ = string.Empty;
        public static string GetFilePath
        {
            get
            {
                return slnFilePath_;
            }
            internal set { slnFilePath_ = value; }
        }

        private static Dictionary<object, object> dictTree_ = new Dictionary<object, object>();

        internal static void UpdateDict()
        {
            dictTree_.Clear();
            if (SolutionUtil.Solution == null) return;
            foreach (var i in SolutionUtil.Solution.Apps)
            {
                dictTree_.Add(i, SolutionUtil.Solution);
                UpdateLoop(i);
            }
        }

        private static void UpdateLoop(object x)
        {
            if (x is Entity.ApplicationEntity)
            {
                var a = x as Entity.ApplicationEntity;
                foreach (var i in a.Folders)
                {
                    dictTree_.Add(i, a);
                    UpdateLoop(i);
                }
                foreach (var i in a.Files)
                {
                    dictTree_.Add(i, a);
                }
            }
            if (x is Entity.FolderEntity)
            {
                var b = x as Entity.FolderEntity;
                foreach (var i in b.Files)
                {
                    dictTree_.Add(i, b);
                }
            }
        }
    }
}
