using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.ViewModel
{
    public class ErrorInfoVM : INotifyPropertyChanged
    {
        public FileEntity Entity
        {
            get;
            set;
        }

        public int Line
        {
            get;
            set;
        }

        public string Log
        {
            get;
            set;
        }

        private void NotifyPropertyChanged(string aName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(aName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
