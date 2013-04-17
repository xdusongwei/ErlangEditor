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
    public class SolutionVM
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
        }

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Gear.png", UriKind.RelativeOrAbsolute));

        public ImageSource Icon
        {
            get { return IconSource; }
        }
    }
}
