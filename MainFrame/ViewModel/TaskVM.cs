using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MainFrame.ViewModel
{
    public class TaskVM:ViewModelBase
    {
        private string title_ = string.Empty;

        public string Title
        {
            get { return title_; }
            set { title_ = value; OnPropertyChanged("Title"); }
        }

        private UserControl content_;
        public UserControl Content
        {
            get { return content_; }
            set { content_ = value; OnPropertyChanged("Content"); }
        }
    }
}
