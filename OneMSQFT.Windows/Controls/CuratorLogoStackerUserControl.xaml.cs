using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneMSQFT.WindowsStore.Controls
{
    public sealed partial class CuratorLogoStackerUserControl : UserControl
    {
        public CuratorLogoStackerUserControl()
        {
            this.InitializeComponent();
        }

        public bool IsLayoutHorizontal
        {
            get { return (bool)GetValue(IsLayoutHorizontalProperty); }
            set { SetValue(IsLayoutHorizontalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLayoutHorizontal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLayoutHorizontalProperty =
            DependencyProperty.Register("IsLayoutHorizontal", typeof(bool), typeof(CuratorLogoStackerUserControl), new PropertyMetadata(null));


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
            var localObservableCollection = dependencyPropertyChangedEventArgs.NewValue as ObservableCollection<Uri>;

            if (localObservableCollection != null)
                if (clsuc.IsLayoutHorizontal == true)
                {
                    clsuc.MainStack.Children.Clear();
                    clsuc.MainStack.Orientation = Orientation.Vertical;
                    for (var i = 0; i < localObservableCollection.Count; i = i + 2)
                    {
                        var sp = new StackPanel() { Orientation = Orientation.Horizontal };
                        var bi = new BitmapImage(new Uri(localObservableCollection[i].AbsoluteUri, UriKind.Absolute));
                        var image = new Image() { Source = bi, Margin = new Thickness(0, 0, 10, 10) };
                        sp.Children.Add(image);
                        if (!(i + 1 >= localObservableCollection.Count))
                        {
                            var bi2 = new BitmapImage(new Uri(localObservableCollection[i + 1].AbsoluteUri, UriKind.Absolute));
                            var image2 = new Image() { Source = bi2, Margin = new Thickness(0, 0, 10, 10) };
                            sp.Children.Add(image2);
                        }

                        if (clsuc != null) clsuc.MainStack.Children.Add(sp);
                    }
                }
                else
                {
                    clsuc.MainStack.Children.Clear();
                    clsuc.MainStack.Orientation = Orientation.Horizontal;
                    for (var i = 0; i < localObservableCollection.Count; i++)
                    {
                        var bi = new BitmapImage(new Uri(localObservableCollection[i].AbsoluteUri, UriKind.Absolute));
                        var image = new Image() { Source = bi, Margin = new Thickness(0, 0, 10, 10) };

                        if (clsuc != null) clsuc.MainStack.Children.Add(image);
                    }
                }
        }
    }
}
