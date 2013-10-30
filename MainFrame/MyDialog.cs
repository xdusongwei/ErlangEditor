using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Media;
using System.Diagnostics;

namespace TryMessageBox
{
    public class MyDialog : Window
    {
        private DispatcherFrame _dispatcherFrame;
        private FrameworkElement _container = null;
        private double _lastLeft = 0.0;
        private double _lastTop = 0.0;

        public void Open(FrameworkElement container)
        {
            if (container != null)
            {
                _container = container;
                // 通过禁用来模拟模态的对话框
                _container.IsEnabled = false;
                // 保持总在最上
                this.Owner = GetOwnerWindow(container);
                if (this.Owner != null)
                {
                    this.Owner.Closing += new System.ComponentModel.CancelEventHandler(Owner_Closing);
                }
                // 通过监听容器的Loaded和Unloaded来显示/隐藏窗口
                _container.Loaded += new RoutedEventHandler(Container_Loaded);
                _container.Unloaded += new RoutedEventHandler(Container_Unloaded);
            }
            this.Show();
            try
            {
                ComponentDispatcher.PushModal();
                _dispatcherFrame = new DispatcherFrame(true);
                Dispatcher.PushFrame(_dispatcherFrame);
            }
            finally
            {
                ComponentDispatcher.PopModal();
            }
        }

        // 在Owner关闭的时候关闭
        private void Owner_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
        }

        private void Container_Unloaded(object sender, RoutedEventArgs e)
        {
            // 只能通过这种方式隐藏，而不能通过Visiblity = Visibility.Collapsed，否则会失效
            _lastLeft = this.Left;
            _lastTop = this.Top;
            this.Left = -10000;
            this.Top = -10000;
        }

        private void Container_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = _lastLeft;
            this.Top = _lastTop;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_container != null)
            {
                _container.Loaded -= Container_Loaded;
                _container.Unloaded -= Container_Unloaded;
            }
            if (this.Owner != null)
            {
                this.Owner.Closing -= Owner_Closing;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // 当关闭终止消息循环
            if (_dispatcherFrame != null)
            {
                _dispatcherFrame.Continue = false;
            }
            // 这里必须强制调用一下
            // 否则出现同时点开多个窗口时，只有一个窗口让代码继续
            ComponentDispatcher.PopModal();
            if (_container != null)
            {
                _container.IsEnabled = true;
            }
        }

        private Window GetOwnerWindow(FrameworkElement source)
        {
            var parent = VisualTreeHelper.GetParent(source) as FrameworkElement;
            if (parent == null)
                return null;
            var win = parent as Window;
            return
                win != null ?
                parent as Window :
                GetOwnerWindow(parent);
        }
    }
}
