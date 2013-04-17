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
    public class ProjectVM
    {
        private ProjectEntity entity_;
        public ProjectVM():this(new ProjectEntity{ Name="EmptyPrj"}) { }
        public ProjectVM(ProjectEntity aEntity) { entity_ = aEntity; }
        public string Name
        {
            get { return entity_.Name; }
        }

        public ObservableCollection<ItemVM> Children
        {
            get
            {
                return new ObservableCollection<ItemVM>(entity_.Children.Select(x => new ItemVM(x)));
            }
        }

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Settings.png", UriKind.RelativeOrAbsolute));

        public ImageSource Icon
        {
            get { return IconSource; }
        }
    }
}
