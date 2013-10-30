using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using MainFrame.Pages;

namespace MainFrame.ViewModel
{
    public class MainVM
    {
        public MainVM()
        {
            Tasks = new ObservableCollection<object>();
            HeaderButtons = new ObservableCollection<ToolBoxButtonVM>();
            ContextButtonsLeft = new ObservableCollection<ToolBoxButtonVM>();
            ContextButtonsRight = new ObservableCollection<ToolBoxButtonVM>();
            AreaVMQueryData = new ObservableCollection<AreaVM>();
            App.Navigation.JumpTo(new Home());
            HeaderButtons.Add(new ToolBoxButtonVM{ 
                Text ="首页" , 
                ImageSource = new BitmapImage(new Uri("/Images/MB_0023_home2.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.JumpTo(new Home()); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "作业",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0010_tasks.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.GoFroward(new Tasks()); })
            });
            HeaderButtons.Add(new ToolBoxButtonVM
            {
                Text = "设置",
                ImageSource = new BitmapImage(new Uri("/Images/MB_0005_sett_small.png", UriKind.RelativeOrAbsolute)),
                ClickedAction = new Action(() => { App.Navigation.GoFroward(new Setting()); })
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

        public ObservableCollection<object> Tasks
        {
            get;
            set;
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

        public ObservableCollection<AreaVM> AreaVMQueryData
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

        private Dictionary<string, ViewModelBase> argsDict_ = new Dictionary<string, ViewModelBase>();
    }
}
