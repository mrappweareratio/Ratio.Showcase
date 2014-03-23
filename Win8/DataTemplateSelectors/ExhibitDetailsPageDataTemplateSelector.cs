using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.Win8.DataTemplateSelectors
{
    public class ExhibitDetailsPageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ExhibitMediaItemTemplate
        {
            get;
            set;
        }

        public DataTemplate ExhibitHeaderItemTemplate
        {
            get;
            set;
        }

        public DataTemplate ExhibitFooterItemTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var hivm = item as HeaderFakeMediaItemViewModel;
            if (hivm != null)
            {
                return ExhibitHeaderItemTemplate;
            }

            var fivm = item as FooterFakeMediaItemViewModel;
            if (fivm != null)
            {
                return ExhibitFooterItemTemplate;
            }

            return ExhibitMediaItemTemplate;
        }
    }
}
