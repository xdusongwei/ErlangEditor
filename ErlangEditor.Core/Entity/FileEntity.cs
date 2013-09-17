using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class FileEntity
    {
        public FileEntity()
        {
            Name = string.Empty;
            DisplayName = string.Empty;
        }

        public string Name
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public bool IsAppFile
        {
            get;
            set;
        }

        public object GetParent()
        {
            return Helper.EntityTreeUtil.GetParent(this);
        }
    }
}
