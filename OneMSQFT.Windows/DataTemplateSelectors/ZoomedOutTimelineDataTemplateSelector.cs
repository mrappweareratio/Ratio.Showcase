using System.ComponentModel.DataAnnotations;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DataTemplateSelectors
{
    public class ZoomedOutTimelineDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TimelineZoomedOutComingSoonItemTemplate
        {
            get;
            set;
        }

        public DataTemplate TimelineZoomedOutEventItemTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var vm = item as ComingSoonFakeEventItemViewModel;
            if (vm != null)
            {
                return TimelineZoomedOutComingSoonItemTemplate;
            }
            return TimelineZoomedOutEventItemTemplate;
        }
    }
}
