using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;

namespace OneMSQFT.UILogic.ViewModels
{
    public abstract class BasePageViewModel : ViewModel, IBasePageViewModel
    {
        private ObservableCollection<EventItemViewModel> _squareFootEvents;
        public ObservableCollection<EventItemViewModel> SquareFootEvents
        {
            get { return _squareFootEvents; }
            set { SetProperty(ref _squareFootEvents, value); }
        }



        public event EventHandler<EventArgs> PinContextChanged;

        protected void RaisePinContextChanged()
        {
            var handler = PinContextChanged;
            if (handler != null)
                handler(null, null);
        }

        public virtual SecondaryTileArgs GetSecondaryTileArguments()
        {
            return null;
        }

        public virtual Task<SecondaryTileImages> GetSecondaryTileImages()
        {
            return Task.FromResult<SecondaryTileImages>(null);
        }

        #region resizing

        private const int BaseDesignLandscapeWidth = 1366;
        private const int BaseDesignPortraitWidth = 768;
        protected double GetWidthDelta()
        {
            if (IsHorizontal)
            {
                return BaseDesignLandscapeWidth / Window.Current.Bounds.Width;
            }
            else
            {
                return BaseDesignPortraitWidth / Window.Current.Bounds.Width;
            }
        }

        private const double LargeBaseFontSize = 60;
        public double LargeFlexyFontSize
        {
            get { return Convert.ToInt16(LargeBaseFontSize / GetWidthDelta()); }
        }
        private const double MediumLargeBaseFontSize = 42;
        public double MediumLargeFlexyFontSize
        {
            get { return Convert.ToInt16(MediumLargeBaseFontSize / GetWidthDelta()); }
        }
        public double MediumLargeFlexyTightLeading
        {
            get { return Convert.ToInt16(MediumLargeFlexyFontSize * .9); }
        }
        private const double MediumBaseFontSize = 32;
        public double MediumFlexyFontSize
        {
            get { return Convert.ToInt16(MediumBaseFontSize / GetWidthDelta()); }
        }
        private const double MediumSmallBaseFontSize = 24;
        public double MediumSmallFlexyFontSize
        {
            get { return Convert.ToInt16(MediumSmallBaseFontSize / GetWidthDelta()); }
        }
        private const double SmallBaseFontSize = 20;
        public double SmallFlexyFontSize
        {
            get { return Convert.ToInt16(SmallBaseFontSize / GetWidthDelta()); }
        }
        public Boolean IsHorizontal
        {
            get { return Window.Current.Bounds.Width > Window.Current.Bounds.Height; }
        }

        public virtual void WindowSizeChanged(double width, double height)
        {
            OnPropertyChanged("LargeFlexyFontSize");
            OnPropertyChanged("MediumLargeFlexyFontSize");
            OnPropertyChanged("MediumLargeFlexyTightLeading");
            OnPropertyChanged("MediumFlexyFontSize");
            OnPropertyChanged("MediumSmallFlexyFontSize");
            OnPropertyChanged("SmallFlexyFontSize");
        }

        #endregion
    }
}
