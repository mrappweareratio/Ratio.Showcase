using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public interface IHasMediaContentViewModel
    {
        ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; }

        Visibility MediaContentVisibility { get; }
    }
}