using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using MainFrame.ViewModel;

namespace MainFrame
{
    public class ToolBoxHelper
    {
        public EventHandler ShowButtomButtons;
        public EventHandler HideButtomButtons;

        public ToolBoxHelper()
        {
            IsShowed = true;
        }

        public void ShowButtomBar()
        {
            if (ShowButtomButtons != null && IsShowed == false)
            {
                var evt = ShowButtomButtons;
                evt(this, new EventArgs());
            }
            IsShowed = true;
        }

        public void HideButtomBar()
        {
            if (HideButtomButtons != null && IsShowed == true)
            {
                var evt = HideButtomButtons;
                evt(this, new EventArgs());
            }
            IsShowed = false;
        }

        private bool IsShowed
        {
            get;
            set;
        }

        public void SelectVM(ViewModelBase aVM)
        {
            control_ = Keyboard.FocusedElement as Control;
            if (aVM == null || control_ == null)
            {
                Switch2DefaultToolBox();
                return;
            }
            control_.LostFocus += ControlLostFocus;
        }

        private Control control_;

        private void ControlLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            control_.LostFocus -= ControlLostFocus;
        }

        private void Switch2DefaultToolBox()
        {

        }
    }
}
