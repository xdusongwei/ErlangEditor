using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.Core.Entity
{
    public class FolderEntity
    {
        public FolderEntity()
        {
            Name = string.Empty;
            Files = new List<FileEntity>();
        }

        public string Name
        {
            get;
            set;
        }

        public List<FileEntity> Files
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
