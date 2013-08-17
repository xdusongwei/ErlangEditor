using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MainFrame.ViewModel
{
    public class ToolBoxButtonVM : ViewModelBase
    {
        public ToolBoxButtonVM()
            :this("无名称" , null)
        {

        }

        public ToolBoxButtonVM(string aText, BitmapSource aSource)
        {
            text_ = aText;
            imageSource_ = aSource;
        }

        public string Text
        {
            get { return text_; }
            set { text_ = value; OnPropertyChanged("Text"); }
        }

        public BitmapSource ImageSource
        {
            get { return imageSource_; }
            set { imageSource_ = value; OnPropertyChanged("ImageSource");}
        }

        public Action ClickedAction
        {
            get;
            set;
        }

        private string text_;
        private BitmapSource imageSource_;
    }
}
