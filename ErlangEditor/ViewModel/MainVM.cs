using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using ErlangEditor.Pages;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace ErlangEditor.ViewModel
{
    public class MainVM: ViewModelBase
    {
        public MainVM()
        {
            HeaderButtons = new ObservableCollection<ToolBoxButtonVM>();
            ContextButtonsLeft = new ObservableCollection<ToolBoxButtonVM>();
            ContextButtonsRight = new ObservableCollection<ToolBoxButtonVM>();
            App.Navigation.JumpTo(new Home());
            var bdTitle = new Binding("ActivedTitle") { Source = App.Navigation, Mode = BindingMode.OneWay };
            BindingOperations.SetBinding(this, FrameTitleProperty, bdTitle);
            HeaderButtons.Add(new ToolBoxButtonVM{ 
                Text ="首页" , 
                ImageSource = new BitmapImage(new Uri("/Images/MB_0023_home2.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.JumpTo(new Home()); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "作业",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0010_tasks.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.JumpToWithFirstFrame(WorkingPage); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "设置",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0005_sett_small.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.JumpToWithFirstFrame(new Setting()); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "关于",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0018_help.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.JumpToWithFirstFrame(new About()); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "更多",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0037_Control-Panel2.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.ToolBox.ShowButtomBar(); })
            });
            ContextButtonsRight.Add(new ToolBoxButtonVM
            {
                Text = "收起",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0011_info3.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.ToolBox.HideButtomBar(); })
            });
            WorkingPage = new WorkingPage_NoLoaded();
            OpenedFiles = new List<FileVM>();
            //ContextButtonsLeft.Add(new ToolBoxButtonVM
            //{
            //    Text = "是否对话框",
            //    ImageSource = new BitmapImage(new Uri("/Images/MB_0011_info3.png", UriKind.RelativeOrAbsolute)),
            //    ClickedAction = new Action(() => { App.Navigation.ShowYesNoBox("这是是否对话框", "它的标题在这里"); })
            //});

            //ContextButtonsLeft.Add(new ToolBoxButtonVM
            //{
            //    Text = "警告框",
            //    ImageSource = new BitmapImage(new Uri("/Images/MB_0011_info3.png", UriKind.RelativeOrAbsolute)),
            //    ClickedAction = new Action(() => { App.Navigation.ShowErrorMessageBox("很严重的警告!", "错误"); })
            //});
        }

        public ObservableCollection<ToolBoxButtonVM> HeaderButtons
        {
            get;
            set;
        }

        public ObservableCollection<ToolBoxButtonVM> ContextButtonsLeft
        {
            get;
            set;
        }

        public ObservableCollection<ToolBoxButtonVM> ContextButtonsRight
        {
            get;
            set;
        }

        public ViewModelBase this[string aKey]
        {
            get
            {
                if(argsDict_.ContainsKey(aKey)) return argsDict_[aKey];
                return null;
            }
            set
            {
                argsDict_[aKey] = value;
            }
        }

        public UserControl WorkingPage
        {
            get;
            set;
        }

        public ObservableCollection<PrjTreeItemVM> TreeRoot
        {
            get;
            set;
        }

        public List<FileVM> OpenedFiles
        {
            get;
            set;
        }

        public string FrameTitle
        {
            get { return (string)GetValue(FrameTitleProperty); }
            set { SetValue(FrameTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FrameTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FrameTitleProperty =
            DependencyProperty.Register("FrameTitle", typeof(string), typeof(MainVM), new PropertyMetadata(string.Empty));

        private Dictionary<string, ViewModelBase> argsDict_ = new Dictionary<string, ViewModelBase>();
    }
}
