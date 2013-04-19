using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ErlangEditor.Core.Entity;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class SolutionVM : DependencyObject, INotifyPropertyChanged
    {
        private SolutionEntity entity_;
        public SolutionVM() { entity_ = new SolutionEntity { Name = "EmptySln" }; }
        public SolutionVM(SolutionEntity aEntity) { entity_ = aEntity; }
        public SolutionEntity Entity
        {
            get { return entity_; }
            private set { entity_ = value; }
        }

        
        public ObservableCollection<ProjectVM> Children
        {
            get
            {
                return new ObservableCollection<ProjectVM>(entity_.Children.Select(x => new ProjectVM(x)));
            }
        }

        public string Name
        {
            get { return entity_.Name; }
            set { entity_.Name = value; NotifyPropertyChanged("Name"); }
        }

        public Visibility TextBlockVisibility
        {
            get { return (Visibility)GetValue(TextBlockVisibilityProperty); }
            set { SetValue(TextBlockVisibilityProperty, value); NotifyPropertyChanged("TextBlockVisibility"); }
        }

        // Using a DependencyProperty as the backing store for TextBlockVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBlockVisibilityProperty =
            DependencyProperty.Register("TextBlockVisibility", typeof(Visibility), typeof(SolutionVM), new UIPropertyMetadata(Visibility.Visible));



        public Visibility TextBoxVisibility
        {
            get { return (Visibility)GetValue(TextBoxVisibilityProperty); }
            set { SetValue(TextBoxVisibilityProperty, value); NotifyPropertyChanged("TextBoxVisibility"); }
        }

        // Using a DependencyProperty as the backing store for TextBoxVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxVisibilityProperty =
            DependencyProperty.Register("TextBoxVisibility", typeof(Visibility), typeof(SolutionVM), new UIPropertyMetadata(Visibility.Collapsed));

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Gear.png", UriKind.RelativeOrAbsolute));

        public ImageSource Icon
        {
            get { return IconSource; }
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
