using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://boco.com.cn/presentation/", "Boco.UI.Panels")]
namespace Boco.UI.Panels
{
    public class GridWithState : Grid
    {
        public GridWithState()
        {
            
        }

        public bool EnableToolBox
        {
            get { return (bool)GetValue(EnableToolBoxProperty); }
            set 
            { 
                SetValue(EnableToolBoxProperty, value);
                if (EnableToolBox && ShowToolBox != null) { var evt = ShowToolBox; evt(this, new EventArgs()); }
                if (!EnableToolBox && HideToolBox != null) { var evt = HideToolBox; evt(this, new EventArgs()); }
            }
        }

        // Using a DependencyProperty as the backing store for EnableToolBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableToolBoxProperty =
            DependencyProperty.Register("EnableToolBox", typeof(bool), typeof(GridWithState), new PropertyMetadata(false));

        public event EventHandler ShowToolBox;
        public event EventHandler HideToolBox;
    }
}
