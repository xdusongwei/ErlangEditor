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

        private static SolidColorBrush compiled_ = new SolidColorBrush(Colors.DeepSkyBlue);
        private static SolidColorBrush notcompiled_ = new SolidColorBrush(Colors.Gold);
        private static SolidColorBrush error_ = new SolidColorBrush(Colors.Crimson);

        private static SolidColorBrush sln_ = new SolidColorBrush(Colors.LawnGreen);
        private static SolidColorBrush app_ = new SolidColorBrush(Colors.DarkOrchid);
        private static SolidColorBrush fld_ = new SolidColorBrush(Colors.CadetBlue);
        private static SolidColorBrush fle_ = new SolidColorBrush(Colors.Lavender);

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
                StateColor = notcompiled_;
                TypeColor = fle_;
                DisplayText = (aNode as ErlangEditor.Core.Entity.FileEntity).DisplayName;
            }
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

        //public override bool Equals(object obj)
        //{
        //    if (obj == null || GetType() != obj.GetType())
        //    {
        //        return false;
        //    }

        //    PrjTreeItemVM other = obj as PrjTreeItemVM;

        //    if (DisplayText != other.DisplayText)
        //        return false;
        //    return true;
        //}

        //public override int GetHashCode()
        //{
        //    return DisplayText.GetHashCode();
        //}

        //public static bool operator ==(PrjTreeItemVM a, PrjTreeItemVM b)
        //{
        //    if (Object.Equals(a, null) && Object.Equals(b, null))
        //    {
        //        return true;
        //    }
        //    return a.Equals(b);
        //}

        //public static bool operator !=(PrjTreeItemVM a, PrjTreeItemVM b)
        //{
        //    return !(a == b);
        //}
    }
}
