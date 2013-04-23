using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ErlangEditor.Core.Entity;

namespace ErlangEditor.ViewModel
{
    public class OpenedFileVM : INotifyPropertyChanged
    {
        public string BarText
        {
            get 
            { 
                return Modified ? Entity.Name + "*" : Entity.Name;
            }
        }

        public FileEntity Entity
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public bool Modified
        {
            get { return Entity.Modified; }
            set { Entity.Modified = value; NotifyPropertyChanged("BarText"); }
        }

        public OpenedFileVM(FileEntity aEntity)
        {
            Entity = aEntity;
        }

        public OpenedFileVM()
        {
            Entity = new FileEntity();
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
