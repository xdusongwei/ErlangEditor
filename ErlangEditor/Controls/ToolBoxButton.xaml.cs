using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ErlangEditor.ViewModel;

namespace ErlangEditor
{
    /// <summary>
    /// ToolBoxButton.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBoxButton : UserControl
    {
        public ToolBoxButton()
        {
            InitializeComponent();
        }


        public readonly static RoutedEvent ClickEvent = 
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(EventHandler<EventArg.ToolBoxButtonClickEventArgs>), typeof(ToolBoxButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            RaiseEvent(new EventArg.ToolBoxButtonClickEventArgs(ClickEvent, this) { ViewModel = DataContext as ToolBoxButtonVM });
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ToolBoxButton), new PropertyMetadata("无名称"));



        public BitmapSource ImageSource
        {
            get { return (BitmapSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(BitmapSource), typeof(ToolBoxButton), new PropertyMetadata(null));
    }
}
