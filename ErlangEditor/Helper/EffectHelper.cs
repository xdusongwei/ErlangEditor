using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ErlangEditor
{
    public class EffectHelper : DependencyObject
    {
        public static bool GetTitleEffect(DependencyObject obj)
        {
            return (bool)obj.GetValue(TitleEffectProperty);
        }

        public static void SetTitleEffect(DependencyObject obj, bool value)
        {
            var elem = obj as FrameworkElement;
            var transGroup = new TransformGroup();
            transGroup.Children.Add(new ScaleTransform());
            transGroup.Children.Add(new SkewTransform());
            transGroup.Children.Add(new RotateTransform());
            transGroup.Children.Add(new TranslateTransform());
            elem.RenderTransform = transGroup;
            var sb = new Storyboard();
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)", new object[0]));
            da.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero), Value = 60});
            da.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(900)), Value = 0, EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut } });
            sb.Children.Add(da);
            elem.Loaded += (a, b) => {
                sb.Begin(); 
            };
            obj.SetValue(TitleEffectProperty, value);
        }

        // Using a DependencyProperty as the backing store for TitleEffect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleEffectProperty =
            DependencyProperty.RegisterAttached("TitleEffect", typeof(bool), typeof(EffectHelper), new PropertyMetadata(TitleEffectChanged));



        public static int GetScrollEffect(DependencyObject obj)
        {
            return (int)obj.GetValue(ScrollEffectProperty);
        }

        public static void SetScrollEffect(DependencyObject obj, int value)
        {
            var elem = obj as FrameworkElement;
            var transGroup = new TransformGroup();
            transGroup.Children.Add(new ScaleTransform());
            transGroup.Children.Add(new SkewTransform());
            transGroup.Children.Add(new RotateTransform());
            transGroup.Children.Add(new TranslateTransform());
            elem.RenderTransform = transGroup;
            var sb = new Storyboard();
            var da = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[3].(TranslateTransform.X)", new object[0]));
            da.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero), Value = 60 });
            da.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(value)), Value = 60 });
            da.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(value + 900)), Value = 0, EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut } });
            sb.Children.Add(da);
            var da2 = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(da2, obj);
            Storyboard.SetTargetProperty(da2, new PropertyPath(UIElement.OpacityProperty));
            da2.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero), Value = 0 });
            da2.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(value)), Value = 0 });
            da2.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(value + 400)), Value = 1 });
            sb.Children.Add(da2);
            elem.Loaded += (a, b) =>
            {
                sb.Begin();
            };
            obj.SetValue(ScrollEffectProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollEffect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollEffectProperty =
            DependencyProperty.RegisterAttached("ScrollEffect", typeof(int), typeof(EffectHelper), new PropertyMetadata(ScrollEffectChanged));

        public static void TitleEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetTitleEffect(d, (bool)e.NewValue);
        }

        public static void ScrollEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetScrollEffect(d, (int)e.NewValue);
        }
    }
}
