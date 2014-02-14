using System.ComponentModel.DataAnnotations;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DataTemplateSelectors
{
    public class ExhibitShowMoreDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TimelineExhibitTemplate
        {
            get;
            set;
        }

        public DataTemplate TimelineShowMoreExhibitTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var vm = item as ShowMoreFakeExhibitItemViewModel;
            if (vm != null)
            {
                return TimelineShowMoreExhibitTemplate;
            }
            return TimelineExhibitTemplate;
        }
    }
}
