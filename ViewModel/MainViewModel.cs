using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErlangEditor.ViewModel
{
    public class MainViewModel
    {
        public bool Loaded
        {
            get;
            private set;
        }

        public void Load()
        {
            Loaded = true;
        }
    }
}
