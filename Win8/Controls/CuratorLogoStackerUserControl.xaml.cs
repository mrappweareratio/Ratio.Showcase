using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Ratio.Showcase.Win8.Controls
{
    public sealed partial class CuratorLogoStackerUserControl : UserControl
    {
        public CuratorLogoStackerUserControl()
        {
            this.InitializeComponent();
        }

        private double AvailableColumnWidthThreshold = 260.0;

        public double AvailableColumnWidth
        {
            get { return (double)GetValue(AvailableColumnWidthProperty); }
            set { SetValue(AvailableColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableColumnWidthProperty =
            DependencyProperty.Register("AvailableColumnWidth", typeof(double), typeof(CuratorLogoStackerUserControl), new PropertyMetadata(0.0, AvailableColumnWidthPropertyChangedCallback));

        private static void AvailableColumnWidthPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var clsuc = dependencyObject as CuratorLogoStackerUserControl;
            if (clsuc != null)
            {
                var currentAvailableWidth = (Convert.ToDouble(dependencyPropertyChangedEventArgs.NewValue)/3);
                ReLayoutLogos(clsuc, currentAvailableWidth < clsuc.AvailableColumnWidthThreshold);
            }
        }


        public bool IsLayoutHorizontal
        {
            get { return (bool)GetValue(IsLayoutHorizontalProperty); }
            set { SetValue(IsLayoutHorizontalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLayoutHorizontal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLayoutHorizontalProperty =
            DependencyProperty.Register("IsLayoutHorizontal", typeof(bool), typeof(CuratorLogoStackerUserControl), new PropertyMetadata(null, IsLayoutHorizontalPropertyChangedCallback));

        private static void IsLayoutHorizontalPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var clsuc = dependencyObject as CuratorLogoStackerUserControl;
            if (clsuc != null)
            {
                var currentAvailableWidth = (Convert.ToDouble(clsuc.AvailableColumnWidth)/3);
                ReLayoutLogos(clsuc, currentAvailableWidth < clsuc.AvailableColumnWidthThreshold);
            }
        }


        public ObservableCollection<Uri> CuratorLogoCollection
        {
            get { return (ObservableCollection<Uri>)GetValue(CuratorLogoCollectionProperty); }
            set { SetValue(CuratorLogoCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CuratorLogoCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CuratorLogoCollectionProperty =
            DependencyProperty.Register("CuratorLogoCollection", typeof(ObservableCollection<Uri>), typeof(CuratorLogoStackerUserControl), new PropertyMetadata(null, CuratorLogoCollectionChanged));

        private static void CuratorLogoCollectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var clsuc = dependencyObject as CuratorLogoStackerUserControl;
            if (clsuc != null)
            {
                var currentAvailableWidth = (Convert.ToDouble(clsuc.AvailableColumnWidth) / 3);
                ReLayoutLogos(clsuc, currentAvailableWidth < clsuc.AvailableColumnWidthThreshold);
            }
        }

        private static void ReLayoutLogos(CuratorLogoStackerUserControl clsuc, bool widthThresholdCrossed)
        {
            if (clsuc.CuratorLogoCollection != null)
                if (clsuc.IsLayoutHorizontal && !widthThresholdCrossed)
                {
                    // vertical stack of 2 column horizontal stacks
                    clsuc.MainStack.Children.Clear();
                    clsuc.MainStack.Orientation = Orientation.Vertical;
                    for (var i = 0; i < clsuc.CuratorLogoCollection.Count; i = i + 2)
                    {
                        var sp = new StackPanel() {Orientation = Orientation.Horizontal};
                        var bi = new BitmapImage(new Uri(clsuc.CuratorLogoCollection[i].AbsoluteUri, UriKind.Absolute));
                        var image = new Image() {Source = bi, Margin = new Thickness(0, 0, 10, 10)};
                        sp.Children.Add(image);
                        if (!(i + 1 >= clsuc.CuratorLogoCollection.Count))
                        {
                            var bi2 = new BitmapImage(new Uri(clsuc.CuratorLogoCollection[i + 1].AbsoluteUri, UriKind.Absolute));
                            var image2 = new Image() {Source = bi2, Margin = new Thickness(0, 0, 10, 10)};
                            sp.Children.Add(image2);
                        }

                        if (clsuc != null) clsuc.MainStack.Children.Add(sp);
                    }
                }
                else if (clsuc.IsLayoutHorizontal && widthThresholdCrossed)
                {
                    // vertical stack 1 column
                    clsuc.MainStack.Children.Clear();
                    clsuc.MainStack.Orientation = Orientation.Vertical;
                    for (var i = 0; i < clsuc.CuratorLogoCollection.Count; i++)
                    {
                        var bi = new BitmapImage(new Uri(clsuc.CuratorLogoCollection[i].AbsoluteUri, UriKind.Absolute));
                        var image = new Image() { Source = bi, Margin = new Thickness(0, 0, 10, 10), MaxWidth = 142.0, MaxHeight = 34.0, HorizontalAlignment = HorizontalAlignment.Left};

                        if (clsuc != null) clsuc.MainStack.Children.Add(image);
                    }
                }
                else
                {   
                    // horizontal stack 1 row
                    clsuc.MainStack.Children.Clear();
                    clsuc.MainStack.Orientation = Orientation.Horizontal;
                    for (var i = 0; i < clsuc.CuratorLogoCollection.Count; i++)
                    {
                        var bi = new BitmapImage(new Uri(clsuc.CuratorLogoCollection[i].AbsoluteUri, UriKind.Absolute));
                        var image = new Image() {Source = bi, Margin = new Thickness(0, 0, 10, 10)};

                        if (clsuc != null) clsuc.MainStack.Children.Add(image);
                    }
                }
        }
    }
}
