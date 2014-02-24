using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.WindowsStore.Controls
{
    public sealed partial class LogoUserControl : UserControl
    {
        public LogoUserControl()
        {
            this.InitializeComponent();
        }


        public String LogoColor
        {
            get { return (String)GetValue(LogoColorProperty); }
            set { SetValue(LogoColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogoColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoColorProperty =
            DependencyProperty.Register("LogoColor", typeof(String), typeof(LogoUserControl), new PropertyMetadata(null, SelectedMediaContentSourcePropertyChanged));

        private static void SelectedMediaContentSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var luc = d as LogoUserControl;
            if (luc == null) return;
            luc.Path0.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
            luc.Path1.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
            luc.Path2.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
            luc.Path3.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
            luc.Path4.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
            luc.Path5.Fill = new SolidColorBrush(ColorUtils.GetColorFromHexString(e.NewValue.ToString(), Colors.White));
        }



        public double ScaleRatio
        {
            get { return (double)GetValue(ScaleRatioProperty); }
            set { SetValue(ScaleRatioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleRatio.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleRatioProperty =
            DependencyProperty.Register("ScaleRatio", typeof(double), typeof(LogoUserControl), new PropertyMetadata(null, SelectedScaleRatioPropertyChanged));

        private static void SelectedScaleRatioPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var luc = d as LogoUserControl;
            if (luc == null) return;
            luc.CompositeTransform.ScaleX = Convert.ToDouble(e.NewValue);
            luc.CompositeTransform.ScaleY = Convert.ToDouble(e.NewValue);
        }
    }
}
