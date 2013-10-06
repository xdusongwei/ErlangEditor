using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class CheckableApplicationVM :ViewModelBase
    {
        public CheckableApplicationVM()
        {
            
        }

        public string Name
        {
            get;
            set;
        }

        public ErlangEditor.Core.Entity.ApplicationEntity Entity
        {
            get;
            set;
        }

        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool?), typeof(CheckableApplicationVM), new PropertyMetadata(true));

        
    }
}
