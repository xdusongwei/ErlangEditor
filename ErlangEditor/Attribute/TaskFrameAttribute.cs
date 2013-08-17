using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor
{
    public class TaskFrameAttribute : Attribute
    {
        public TaskFrameAttribute()
        {
            IsSingleFrame = true;
            CanClose = true;
        }

        public bool IsSingleFrame
        {
            get;
            set;
        }

        public bool CanClose
        {
            get;
            set;
        }
    }
}
