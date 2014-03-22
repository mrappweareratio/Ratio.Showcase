using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.Win8.DataTemplateSelectors
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
