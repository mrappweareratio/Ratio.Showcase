using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public interface IHasMediaContentViewModel
    {
        ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; }

        Visibility MediaContentVisibility { get; }
    }
}