using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.Specialized;

namespace ErlangEditor
{
    public class ListScroll
    {
        #region IsEnabled

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ListScroll),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsEnabledChanged)));

        public static bool GetIsEnabled(ItemsControl d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(ItemsControl d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldIsEnabled = (bool)e.OldValue;
            bool newIsEnabled = (bool)d.GetValue(IsEnabledProperty);

            var itemsControl = d as ItemsControl;
            if (itemsControl == null)
                return;

            if (newIsEnabled)
            {
                itemsControl.Loaded += (ss, ee) =>
                    {
                        ScrollViewer scrollviewer = WpfTreeHelper.FindChild<ScrollViewer>(itemsControl);
                        if (scrollviewer != null)
                        {
                            ((ICollectionView)itemsControl.Items).CollectionChanged += (sss, eee) =>
                                {
                                    scrollviewer.ScrollToEnd();
                                };
                        }
                    };
            }
        }

        #endregion


    }
}
