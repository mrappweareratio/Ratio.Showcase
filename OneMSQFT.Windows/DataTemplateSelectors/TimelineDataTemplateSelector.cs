using System.ComponentModel.DataAnnotations;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DataTemplateSelectors
{
    public class TimelineDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BufferItemFakeEventItemTemplate
        {
            get;
            set;
        }

        public DataTemplate EventTimelineItemTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var vm = item as BufferItemFakeEventItemViewModel;
            if (vm != null)
            {
                return BufferItemFakeEventItemTemplate;
            }
            return EventTimelineItemTemplate;
        }
    }
}
