using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.Win8.DataTemplateSelectors
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
