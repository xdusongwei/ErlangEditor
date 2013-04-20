using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ErlangEditor.Core.Entity;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class ProjectVM : DependencyObject, INotifyPropertyChanged, IComparable 
    {
        private ProjectEntity entity_;
        public ProjectVM():this(new ProjectEntity{ Name="EmptyPrj"}) { }
        public ProjectVM(ProjectEntity aEntity) { entity_ = aEntity; }
        public string Name
        {
            get { return entity_.Name; }
            set { entity_.Name = value; NotifyPropertyChanged("Name"); }
        }

        ObservableCollection<ItemVM> children_ = null;
        public ObservableCollection<ItemVM> Children
        {
            get
            {
                if (children_ == null)
                {
                    var lst = new List<ItemVM>(entity_.Children.Select(x => new ItemVM(x)));
                    lst.Sort();
                    children_ = new ObservableCollection<ItemVM>(lst);
                }
                return children_;
            }
        }

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Settings.png", UriKind.RelativeOrAbsolute));

        public ImageSource Icon
        {
            get { return IconSource; }
        }

        public ProjectEntity Entity
        {
            get { return entity_; }
        }


        public Visibility TextBlockVisibility
        {
            get { return (Visibility)GetValue(TextBlockVisibilityProperty); }
            set { SetValue(TextBlockVisibilityProperty, value); NotifyPropertyChanged("TextBlockVisibility");}
        }

        // Using a DependencyProperty as the backing store for TextBlockVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockVisibilityProperty =
            DependencyProperty.Register("TextBlockVisibility", typeof(Visibility), typeof(ProjectVM), new UIPropertyMetadata(Visibility.Visible));

        

        public Visibility TextBoxVisibility
        {
            get { return (Visibility)GetValue(TextBoxVisibilityProperty); }
            set { SetValue(TextBoxVisibilityProperty, value); NotifyPropertyChanged("TextBoxVisibility"); }
        }

        // Using a DependencyProperty as the backing store for TextBoxVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxVisibilityProperty =
            DependencyProperty.Register("TextBoxVisibility", typeof(Visibility), typeof(ProjectVM), new UIPropertyMetadata(Visibility.Collapsed));


        private void NotifyPropertyChanged(string aName)
        {
            var evt = PropertyChanged;
            if (evt != null)
                evt(this, new PropertyChangedEventArgs(aName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object obj)
        {
            var other = obj as ProjectVM;
            return Name.CompareTo(other.Name);
        }
    }
}
