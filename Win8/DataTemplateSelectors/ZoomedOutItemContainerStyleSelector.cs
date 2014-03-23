using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.Win8.DataTemplateSelectors
{
    public class ZoomedOutItemContainerStyleSelector : StyleSelector
    {
        public Style TimelineZoomedOutEventItemContainerStyle { get; set; }

        public Style TimelineZoomedOutComingSoonItemContainerStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var vm = item as ComingSoonFakeEventItemViewModel;
            if (vm != null)
            {
                return TimelineZoomedOutComingSoonItemContainerStyle;
            }
            return TimelineZoomedOutEventItemContainerStyle;
        }
    }
}
