using System.ComponentModel.DataAnnotations;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DataTemplateSelectors
{
    public class TimelineDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PastTimeLineItemTemplate
        {
            get;
            set;
        }

        public DataTemplate HomeTimeLineItemTemplate
        {
            get;
            set;
        }

        public DataTemplate FutureTimeLineItemTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var defaultLayoutType = HomeTimeLineItemTemplate;
            var vm = item as EventItemViewModel;
            if (vm != null)
            {
                if (vm.Name == "Home")
                {
                    return HomeTimeLineItemTemplate;
                }
                if (vm.IsInTheFuture)
                {
                    return FutureTimeLineItemTemplate;
                }
            }
            return PastTimeLineItemTemplate;
        }
    }
}
