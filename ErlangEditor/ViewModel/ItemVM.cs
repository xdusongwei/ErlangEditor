using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ErlangEditor.Core.Entity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace ErlangEditor.ViewModel
{
    public class ItemVM : DependencyObject, INotifyPropertyChanged
    {
        public ItemVM() :this(new FileEntity{ Name ="emptyfile"}) { }
        public ItemVM(FileEntity aEntity) { entity_ = aEntity; }

        private FileEntity entity_;
        public bool IsFolder
        {
            get;
            set;
        }

        public string Name
        {
            get { return entity_.Name; }
            set { entity_.Name = value; NotifyPropertyChanged("Name"); }
        }

        public IEnumerable<ItemVM> Children
        {
            get;
            set;
        }

        public Visibility TextBlockVisibility
        {
            get { return (Visibility)GetValue(TextBlockVisibilityProperty); }
            set { SetValue(TextBlockVisibilityProperty, value); NotifyPropertyChanged("TextBlockVisibility"); }
        }

        // Using a DependencyProperty as the backing store for TextBlockVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockVisibilityProperty =
            DependencyProperty.Register("TextBlockVisibility", typeof(Visibility), typeof(ItemVM), new UIPropertyMetadata(Visibility.Visible));



        public Visibility TextBoxVisibility
        {
            get { return (Visibility)GetValue(TextBoxVisibilityProperty); }
            set { SetValue(TextBoxVisibilityProperty, value); NotifyPropertyChanged("TextBoxVisibility"); }
        }

        // Using a DependencyProperty as the backing store for TextBoxVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxVisibilityProperty =
            DependencyProperty.Register("TextBoxVisibility", typeof(Visibility), typeof(ItemVM), new UIPropertyMetadata(Visibility.Hidden));

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Generic_Document.png", UriKind.RelativeOrAbsolute));
        private static readonly ImageSource IconSource2 = new BitmapImage(new Uri("/Images/Stuffed_Folder.png", UriKind.RelativeOrAbsolute));
        public ImageSource Icon
        {
            get { return IsFolder ? IconSource2 : IconSource; }
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
