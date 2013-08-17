using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MainFrame.ViewModel
{
    public class AreaVM :ViewModelBase
    {
        public int OnlineCount
        {
            get;
            set;
        }

        public int OfflineCount
        {
            get;
            set;
        }

        public int ErrorCount
        {
            get;
            set;
        }

        public string AreaName
        {
            get;
            set;
        }
    }
}
