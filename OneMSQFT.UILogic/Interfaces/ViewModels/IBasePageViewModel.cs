using System.Collections.ObjectModel;
using System.ComponentModel;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface IBasePageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Application Navigation Bar Bound Top Level Event Items
        /// </summary>
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        void WindowSizeChanged(double width, double height);
    }
}