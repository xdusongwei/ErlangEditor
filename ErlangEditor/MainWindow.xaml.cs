using ErlangEditor.EventArg;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErlangEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ((tbxBackward.RenderTransform as TransformGroup).Children[3] as TranslateTransform).X = -64;
            HeaderEnable = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = App.MainViewModel;
            App.Navigation.PropertyChanged +=
                (a, b) =>
                {
                    if (b.PropertyName == "ActivedFrame")
                    {
                        frameContent.Children.Clear();
                        frameContent.Children.Add(App.Navigation.ActivedFrame);
                        if (App.Navigation.ActivedFrame.GetType().GetCustomAttributes(typeof(DialogFrameAttribute), true).Length != 0)
                        {
                            if (HeaderEnable)
                            {
                                var da = new DoubleAnimation();
                                da.EasingFunction = new CircleEase();
                                da.To = 0D;
                                da.Duration = TimeSpan.FromMilliseconds(300D);
                                navRoot.BeginAnimation(Grid.HeightProperty, da);
                                App.ToolBox.HideButtomBar();
                            }
                            HeaderEnable = false;
                        }
                        else
                        {
                            if (!HeaderEnable)
                            {
                                var da = new DoubleAnimation();
                                da.EasingFunction = new CircleEase();
                                da.From = 0D;
                                da.Duration = TimeSpan.FromMilliseconds(300D);
                                navRoot.BeginAnimation(Grid.HeightProperty, da);
                                App.ToolBox.ShowButtomBar();
                            }
                            HeaderEnable = true;
                        }
                    }
                };
            App.Navigation.EnableBackward +=
                (a, b) => { (Resources["BackInSB"] as Storyboard).Begin(); };
            App.Navigation.DisableBackward +=
                (a, b) => { (Resources["BackOutSB"] as Storyboard).Begin(); };
            App.Navigation.ShowingMessage +=
                (a, b) => { ucMsgPanel.Show(b.Title, b.Message); };
            App.Navigation.ShowingYesNoMessage +=
                (a, b) => { ucYNPanel.Show(b.Title, b.Message); };
            App.Navigation.ShowingErrorMessage +=
                (a, b) => { ucMsgPanel.ShowError(b.Title, b.Message); };
            App.ToolBox.ShowButtomButtons += (a, b) => { rootLayout_ShowToolBox(a, b); };
            App.ToolBox.HideButtomButtons += (a, b) => { rootLayout_HideToolBox(a, b); };
            frameContent.Children.Clear();
            frameContent.Children.Add(App.Navigation.ActivedFrame);
            try
            {
                ErlangEditor.Core.ConfigUtil.LoadConfig();
                App.MainViewModel.AutoCompleteCache.SetLibCache(ErlangEditor.Core.ConfigUtil.Config.AutoCompleteCache);
            }
            catch (Exception ecp)
            {
                //App.Navigation.ShowMessageBox(ecp.Message, "出错");
            }
        }

        void rootLayout_HideToolBox(object sender, EventArgs e)
        {
            var da = new DoubleAnimation();
            da.EasingFunction = new CircleEase();
            da.To = 0D;
            da.Duration = TimeSpan.FromMilliseconds(300D);
            toolboxRoot.BeginAnimation(Grid.HeightProperty, da);
        }

        void rootLayout_ShowToolBox(object sender, EventArgs e)
        {
            var da = new DoubleAnimation();
            da.EasingFunction = new BackEase { Amplitude = 1 };
            da.From = 0D;
            da.Duration = TimeSpan.FromMilliseconds(300D);
            toolboxRoot.BeginAnimation(Grid.HeightProperty, da);
        }

        private void ToolBoxButton_MouseLeftButtonDown_5(object sender, MouseButtonEventArgs e)
        {
            App.Navigation.GoBackward();
        }

        public void InvokeCustomAction(object sender, ToolBoxButtonClickEventArgs e)
        {
            e.Handled = true;
            if (e.ViewModel != null && e.ViewModel.ClickedAction != null) e.ViewModel.ClickedAction();
        }

        private bool HeaderEnable
        {
            get;
            set;
        }
    }
}
