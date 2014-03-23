using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public class DataContextProxy : FrameworkElement
    {
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(Object), typeof(DataContextProxy), null);
        
        public DataContextProxy()
        {
            Loaded += OnDataContextProxyLoaded;            
        }

        public Object DataSource
        {
            get { return GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public string BindingPropertyName { get; set; }

        public BindingMode BindingMode { get; set; }

        private void OnDataContextProxyLoaded(object sender, RoutedEventArgs e)
        {
            var binding = new Binding();
            if (!String.IsNullOrEmpty(BindingPropertyName))
            {
                binding.Path = new PropertyPath(BindingPropertyName);
            }
            binding.Source = DataContext;
            binding.Mode = BindingMode;
            SetBinding(DataSourceProperty, binding);
        }
    }
}