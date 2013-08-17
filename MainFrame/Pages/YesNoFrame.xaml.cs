using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// YesNoFrame.xaml 的交互逻辑
    /// </summary>
    public partial class YesNoFrame : UserControl
    {
        public YesNoFrame()
        {
            InitializeComponent();
        }
        private DispatcherFrame dispatcherFrame_;
        public void Show(string aTitle, string aMessage)
        {
            tbMessage.Text = aMessage;
            tbTitle.Text = aTitle;
            BeginAnimation(UserControl.OpacityProperty, (Resources["ShowSB"] as DoubleAnimation));
            Visibility = System.Windows.Visibility.Visible;
            Result = false;
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
            if (sender == btnYes) Result = true;
            Result = false;
        }

        public bool Result
        {
            get;
            private set;
        }
    }
}
