using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ErlangEditor.ViewModel
{
    public class PrjTreeItemVM :ViewModelBase
    {
        public PrjTreeItemVM():this(null)
        {

        }

        public void ContentChanged()
        {
            if (Entity is ErlangEditor.Core.Entity.FileEntity)
            {
                var name = (Entity as ErlangEditor.Core.Entity.FileEntity).Name;
                var ext = System.IO.Path.GetExtension(name).ToLower();
                if (ext == ".erl")
                {
                    //StateColor = notcompiled_;
                }
            }
        }

        private static SolidColorBrush compiled_ = new SolidColorBrush(Colors.DeepSkyBlue);
        private static SolidColorBrush notcompiled_ = new SolidColorBrush(Colors.Gold);
        private static SolidColorBrush error_ = new SolidColorBrush(Colors.Crimson);
        private static SolidColorBrush other_ = new SolidColorBrush(Colors.Transparent);

        private static SolidColorBrush sln_ = new SolidColorBrush(Colors.LawnGreen);
        private static SolidColorBrush app_ = new SolidColorBrush(Colors.DarkOrchid);
        private static SolidColorBrush fld_ = new SolidColorBrush(Colors.CadetBlue);
        private static SolidColorBrush fle_ = new SolidColorBrush(Colors.Lavender);
        private static SolidColorBrush erlFile_ = new SolidColorBrush(Colors.Yellow);
        private static SolidColorBrush hrlFile_ = new SolidColorBrush(Colors.CornflowerBlue);
        private static SolidColorBrush appFile_ = new SolidColorBrush(Colors.DarkSeaGreen);

        public PrjTreeItemVM(object aNode)
        {
            if (aNode == null) return;
            
            if (aNode is ErlangEditor.Core.Entity.SolutionEntity)
            {
                StateColor = TypeColor = sln_;
                DisplayText = (aNode as ErlangEditor.Core.Entity.SolutionEntity).Name;
            }
            if (aNode is ErlangEditor.Core.Entity.ApplicationEntity)
            {
                StateColor = TypeColor = app_;
                DisplayText = (aNode as ErlangEditor.Core.Entity.ApplicationEntity).Name;
            }
            if (aNode is ErlangEditor.Core.Entity.FolderEntity)
            {
                StateColor = TypeColor = fld_;
                DisplayText = (aNode as ErlangEditor.Core.Entity.FolderEntity).Name;
            }
            if (aNode is ErlangEditor.Core.Entity.FileEntity)
            {
                var name = (aNode as ErlangEditor.Core.Entity.FileEntity).Name;
                var ext = System.IO.Path.GetExtension(name).ToLower();
                if (ext == ".erl")
                    TypeColor = erlFile_;
                else if (ext == ".hrl")
                    TypeColor = hrlFile_;
                else if (ext == ".src")
                    TypeColor = appFile_;
                else
                    TypeColor = fle_;
                //if (ext == ".erl")
                //{
                //    StateColor = notcompiled_;
                //}
                //else
                //{
                //    StateColor = other_;
                //}
                StateColor = other_;
                DisplayText = (aNode as ErlangEditor.Core.Entity.FileEntity).DisplayName;
            }
            Entity = aNode;
        }

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(string), typeof(PrjTreeItemVM), new PropertyMetadata("No_Text"));

        public object Entity
        {
            get;
            set;
        }

        public SolidColorBrush TypeColor
        {
            get { return (SolidColorBrush)GetValue(TypeColorProperty); }
            set { SetValue(TypeColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeColorProperty =
            DependencyProperty.Register("TypeColor", typeof(SolidColorBrush), typeof(PrjTreeItemVM), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        public SolidColorBrush StateColor
        {
            get { return (SolidColorBrush)GetValue(StateColorProperty); }
            set { SetValue(StateColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StateColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateColorProperty =
            DependencyProperty.Register("StateColor", typeof(SolidColorBrush), typeof(PrjTreeItemVM), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public ObservableCollection<PrjTreeItemVM> Children
        {
            get { return (ObservableCollection<PrjTreeItemVM>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Children", typeof(ObservableCollection<PrjTreeItemVM>), typeof(PrjTreeItemVM), new PropertyMetadata(null));


        public Visibility RemoveToolbarVisibility
        {
            get { return (Visibility)GetValue(RemoveToolbarVisibilityProperty); }
            set { SetValue(RemoveToolbarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoveToolbarVisility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveToolbarVisibilityProperty =
            DependencyProperty.Register("RemoveToolbarVisibility", typeof(Visibility), typeof(PrjTreeItemVM), new PropertyMetadata(Visibility.Collapsed));



        public Visibility AddToolbarVisibility
        {
            get { return (Visibility)GetValue(AddToolbarVisibilityProperty); }
            set { SetValue(AddToolbarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddToolbarVisility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddToolbarVisibilityProperty =
            DependencyProperty.Register("AddToolbarVisibility", typeof(Visibility), typeof(PrjTreeItemVM), new PropertyMetadata(Visibility.Collapsed));



        public Visibility PropToolbarVisibility
        {
            get { return (Visibility)GetValue(PropToolbarVisibilityProperty); }
            set { SetValue(PropToolbarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropToolbarVisility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropToolbarVisibilityProperty =
            DependencyProperty.Register("PropToolbarVisibility", typeof(Visibility), typeof(PrjTreeItemVM), new PropertyMetadata(Visibility.Collapsed));



        public Visibility CompileToolbarVisibility
        {
            get { return (Visibility)GetValue(CompileToolbarVisibilityProperty); }
            set { SetValue(CompileToolbarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompileToolbarVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompileToolbarVisibilityProperty =
            DependencyProperty.Register("CompileToolbarVisibility", typeof(Visibility), typeof(PrjTreeItemVM), new PropertyMetadata(Visibility.Collapsed));



        public Visibility SaveToolbarVisibility
        {
            get { return (Visibility)GetValue(SaveToolbarVisibilityProperty); }
            set { SetValue(SaveToolbarVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveToolbarVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveToolbarVisibilityProperty =
            DependencyProperty.Register("SaveToolbarVisibility", typeof(Visibility), typeof(PrjTreeItemVM), new PropertyMetadata(Visibility.Collapsed));

        
    }
}
