using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace MainFrame.Pages
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    [DialogFrame]
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Tile_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Tile).TileType == TileType.Single)
            {
                
            }
            else
            {
                App.Navigation.ShowMessageBox("我让你点小瓷砖,就不要点大瓷砖.\n我让你点小瓷砖,就不要点大瓷砖.\n我让你点小瓷砖,就不要点大瓷砖.", "你不应该这么做");
            }
            //(sender as Tile).IsSelected = false;
            TileList1.SelectionHelper.ClearSelection();
            //
        }

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
        }

        private void DeviceManagerAction(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
