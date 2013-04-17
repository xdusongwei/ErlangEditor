using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ErlangEditor.ViewModel
{
    public class ContextOperationVM
    {
        public ContextOperationVM() { Children = new ObservableCollection<ContextOperationVM>(); }
        public string Header { get; set; }
        public bool IsSeparator { get; set; }
        public ObservableCollection<ContextOperationVM> Children { get; set; }
    }
}
