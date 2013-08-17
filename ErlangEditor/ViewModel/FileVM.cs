using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ErlangEditor.ViewModel
{
    public class FileVM : ViewModelBase
    {
        public string FileName
        {
            get;
            set;
        }

        public bool Changed
        {
            get { return (bool)GetValue(ChangedProperty); }
            set { SetValue(ChangedProperty, value); }
        }

        public Telerik.Windows.Controls.RadPane Pane
        {
            get;
            set;
        }

        public ErlangEditor.Core.Entity.FileEntity FileEntity
        {
            get;
            set;
        }

        public ICSharpCode.AvalonEdit.TextEditor Editor
        {
            get;
            set;
        }

        public string Content
        {
            get
            {
                return Editor.Text;
            }
        }

        // Using a DependencyProperty as the backing store for Changed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangedProperty =
            DependencyProperty.Register("Changed", typeof(bool), typeof(FileVM), new PropertyMetadata(false));
    }
}
