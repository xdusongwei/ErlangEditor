using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class RecentProjectVM: ViewModelBase
    {
        public RecentProjectVM(ErlangEditor.Core.Entity.RecentProjectEntity aEntity)
        {
            Entity = aEntity;
            if (aEntity != null)
            {
                Path = aEntity.Path;
                Title = aEntity.Title;
            }
        }

        public ErlangEditor.Core.Entity.RecentProjectEntity Entity
        {
            get;
            set;
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(RecentProjectVM), new PropertyMetadata(string.Empty));

        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RecentProjectVM), new PropertyMetadata(string.Empty));
    }
}
