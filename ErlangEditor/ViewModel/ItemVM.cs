using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ErlangEditor.Core.Entity;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ErlangEditor.ViewModel
{
    public class ItemVM
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
        }

        public IEnumerable<ItemVM> Children
        {
            get;
            set;
        }

        private static readonly ImageSource IconSource =new BitmapImage(new Uri("/Images/Generic_Document.png", UriKind.RelativeOrAbsolute));
        public ImageSource Icon
        {
            get { return IconSource; }
        }
    }
}
