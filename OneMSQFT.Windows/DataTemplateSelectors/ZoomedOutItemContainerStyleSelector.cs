using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DataTemplateSelectors
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
