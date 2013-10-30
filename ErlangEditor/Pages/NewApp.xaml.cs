using ErlangEditor.ViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErlangEditor.Pages
{
    /// <summary>
    /// NewApp.xaml 的交互逻辑
    /// </summary>
    public partial class NewApp : UserControl
    {
        public NewApp(ViewModel.PrjTreeItemVM aSln)
        {
            InitializeComponent();
            sln_ = aSln;
        }

        public string Title
        {
            get
            {
                return "添加新应用";
            }
        }

        private ViewModel.PrjTreeItemVM sln_;

        public void UpdateToolBox()
        {
            App.MainViewModel.ContextButtonsLeft.Clear();
            App.MainViewModel.ContextButtonsLeft.Add(new ViewModel.ToolBoxButtonVM("创建", new BitmapImage(new Uri("/Images/appbar.check.rest.png", UriKind.RelativeOrAbsolute)))
            {
                ClickedAction = new Action(() =>
                {
                    var name = tbName.Text.Trim();
                    if (sln_ != null && sln_.Entity != null && sln_.Entity is ErlangEditor.Core.Entity.SolutionEntity)
                    {
                        var entity = sln_.Entity as ErlangEditor.Core.Entity.SolutionEntity;
                        if (entity.Apps.Any(i => i.Name == name))
                        {
                            App.Navigation.ShowMessageBox("已经存在此名称的应用。", "添加应用");
                            return;
                        }
                        try
                        {
                            var app = new ErlangEditor.Core.Entity.ApplicationEntity() 
                            { 
                                Name = name, 
                                AppMode = rbAppConfig.IsChecked??false , 
                                CodeMode = rbNormal.IsChecked?? false,
                                NoStartup = rbNostartup.IsChecked??false,
                                StartupAsMFA =rbMFA.IsChecked ?? false,
                                StartupMFA = tbMFA.Text.Trim()
                            };
                            ErlangEditor.Core.ApplicationUtil.AddApplication(app);
                            ErlangEditor.Core.SolutionUtil.SaveSolution();
                            App.Entity.MakeTreeLoop(sln_, app);
                            App.Navigation.JumpToWithFirstFrame(App.MainViewModel.WorkingPage);
                        }
                        catch(Exception e) 
                        {
                            App.Navigation.ShowMessageBox(e.Message, "添加应用");
                        }
                        
                    }
                })
            });
            App.ToolBox.ShowButtomBar();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
