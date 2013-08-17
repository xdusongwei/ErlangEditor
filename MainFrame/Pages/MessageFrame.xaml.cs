using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MainFrame.Pages
{
    /// <summary>
    /// MessageFrame.xaml 的交互逻辑
    /// </summary>
    public partial class MessageFrame : UserControl
    {
        public MessageFrame()
        {
            InitializeComponent();
        }
        private DispatcherFrame dispatcherFrame_;
        public void ShowError(string aTitle, string aMessage)
        {
            layoutRoot.Background = new SolidColorBrush(Colors.DarkRed);
            tbMessage.Text = aMessage;
            tbTitle.Text = aTitle;
            BeginAnimation(UserControl.OpacityProperty, (Resources["ShowSB"] as DoubleAnimation));
            Visibility = System.Windows.Visibility.Visible;
            //TryMessageBox.MyDialog dlg = new TryMessageBox.MyDialog();
            //dlg.Open(null);
            //return;
            try
            {
                ComponentDispatcher.PushModal();
                dispatcherFrame_ = new DispatcherFrame(true);
                Dispatcher.PushFrame(dispatcherFrame_);
                dispatcherFrame_.Continue = false;
            }
            finally
            {
               ComponentDispatcher.PopModal();
            }
        }

        public void Show(string aTitle , string aMessage)
        {
            layoutRoot.Background = Resources["NormalBrush"] as SolidColorBrush;
            tbMessage.Text = aMessage;
            tbTitle.Text = aTitle;
            BeginAnimation(UserControl.OpacityProperty, (Resources["ShowSB"] as DoubleAnimation));
            Visibility = System.Windows.Visibility.Visible;
            try
            {
                ComponentDispatcher.PushModal();
                dispatcherFrame_ = new DispatcherFrame(true);
                Dispatcher.PushFrame(dispatcherFrame_);
            }
            finally
            {
                ComponentDispatcher.PopModal();
            }
        }

        public void Hide()
        {
            Visibility = System.Windows.Visibility.Collapsed;
            if (dispatcherFrame_ != null)
                dispatcherFrame_.Continue = false;
　　        ComponentDispatcher.PopModal();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
