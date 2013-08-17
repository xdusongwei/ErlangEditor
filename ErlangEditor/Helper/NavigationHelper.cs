using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ErlangEditor.ViewModel;

namespace ErlangEditor
{
    public class NavigationHelper : ViewModelBase
    {
        private Stack<UserControl> stkFrames_ = new Stack<UserControl>();

        public void GoBackward()
        {
            if (stkFrames_.Count < 2) return;
            stkFrames_.Pop();
            OnPropertyChanged("ActivedFrame");
            OnPropertyChanged("ActivedTitle");
            if (stkFrames_.Count == 1 && DisableBackward != null)
            {
                var evt = DisableBackward;
                evt(this, new EventArgs());
            }
            UpdateButtomToolBar();
        }

        public void GoFroward(UserControl aUC)
        {
            aUC.Width = double.NaN;
            aUC.Height = double.NaN;
            stkFrames_.Push(aUC);
            OnPropertyChanged("ActivedFrame");
            OnPropertyChanged("ActivedTitle");
            if (stkFrames_.Count == 2 && EnableBackward != null)
            {
                var evt = EnableBackward;
                evt(this, new EventArgs());
            }
            UpdateButtomToolBar();
        }

        public void ShowMessageBox(string aMessage, string aTitle)
        {
            if (ShowingMessage != null)
            {
                var evt = ShowingMessage;
                evt(this, new EventArg.MessageArgs { Message = aMessage, Title = aTitle });
            }
        }

        public void ShowErrorMessageBox(string aMessage)
        {
            ShowErrorMessageBox(aMessage, "消息");
        }

        public void ShowErrorMessageBox(string aMessage, string aTitle)
        {
            if (ShowingErrorMessage != null)
            {
                var evt = ShowingErrorMessage;
                evt(this, new EventArg.MessageArgs { Message = aMessage, Title = aTitle });
            }
        }

        public void ShowMessageBox(string aMessage)
        {
            ShowMessageBox(aMessage, "消息");
        }

        public void ShowYesNoBox(string aMessage, string aTitle)
        {
            if (ShowingMessage != null)
            {
                var evt = ShowingYesNoMessage;
                evt(this, new EventArg.MessageArgs { Message = aMessage, Title = aTitle });
            }
        }

        public void ShowYesNoBox(string aMessage)
        {
            ShowYesNoBox(aMessage, "请做出选择");
        }

        public void JumpTo(UserControl aUC)
        {
            aUC.Width = double.NaN;
            aUC.Height = double.NaN;
            var beforeCount = stkFrames_.Count;
            stkFrames_.Clear();
            stkFrames_.Push(aUC);
            OnPropertyChanged("ActivedFrame");
            OnPropertyChanged("ActivedTitle");
            if (beforeCount > 1 && DisableBackward != null)
            {
                var evt = DisableBackward;
                evt(this, new EventArgs());
            }
            UpdateButtomToolBar();
        }

        public void JumpToWithFirstFrame(UserControl aUC)
        {
            aUC.Width = double.NaN;
            aUC.Height = double.NaN;
            var beforeCount = stkFrames_.Count;
            stkFrames_ = new Stack<UserControl>(stkFrames_.Take(1));
            stkFrames_.Push(aUC);
            OnPropertyChanged("ActivedFrame");
            OnPropertyChanged("ActivedTitle");
            if (beforeCount < 2 && EnableBackward != null)
            {
                var evt = EnableBackward;
                evt(this, new EventArgs());
            }
            UpdateButtomToolBar();
        }

        public UserControl ActivedFrame
        {
            get
            {
                if (stkFrames_.Count == 0) return null;
                return stkFrames_.Peek();
            }
        }

        public string ActivedTitle
        {
            get
            {
                string ret = string.Empty;
                try
                {
                    ret = (ActivedFrame as dynamic).Title;
                }
                catch { }
                return ret;
            }
        }

        private void UpdateButtomToolBar()
        {
            try
            {
                dynamic uc = App.Navigation.ActivedFrame;
                uc.UpdateToolBox();
            }
            catch { }
        }

        public EventHandler EnableBackward;
        public EventHandler DisableBackward;
        public EventHandler<EventArg.NavigationArgs> NavigatedTo;
        public EventHandler<EventArg.MessageArgs> ShowingMessage;
        public EventHandler<EventArg.MessageArgs> ShowingErrorMessage;
        public EventHandler<EventArg.MessageArgs> ShowingYesNoMessage;
    }
}
