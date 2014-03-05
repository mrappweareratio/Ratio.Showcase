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
        public DelegateCommand SetStartupCommand { get; protected set; }
        public DelegateCommand ClearStartupCommand { get; protected set; }
        public Visibility ClearStartupVisibility { get; protected set; }
        public Visibility SetStartupVisibility { get; protected set; }

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
        private const int BaseDesignPortraitHeight = 1366;
        private const int BaseDesignLandscapeHeight = 768;

        public double FullScreenHeight
        {
            get { return _fullScreenHeight; }
            private set { SetProperty(ref _fullScreenHeight, value); }
        }

        public double FullScreenWidth
        {
            get { return _fullScreenWidth; }
            private set { SetProperty(ref _fullScreenWidth, value); }
        }

        public bool IsHorizontal
        {
            get { return _isHorizontal; }
            private set { SetProperty(ref _isHorizontal, value); }
        }

        private double _fullScreenWidth;
        private double _fullScreenHeight;
        private bool _isHorizontal;

        protected double WidthDelta { get; private set; }
        protected double HeightDelta { get; private set; }

        private const double LargeBaseFontSize = 60;
        public double LargeFlexyFontSize
        {
            get { return Convert.ToInt16(LargeBaseFontSize / WidthDelta); }
        }
        private const double MediumLargeBaseFontSize = 42;
        public double MediumLargeFlexyFontSize
        {
            get { return Convert.ToInt16(MediumLargeBaseFontSize / WidthDelta); }
        }
        public double MediumLargeFlexyTightLeading
        {
            get { return Convert.ToInt16(MediumLargeFlexyFontSize * .9); }
        }
        private const double MediumBaseFontSize = 32;
        public double MediumFlexyFontSize
        {
            get { return Convert.ToInt16(MediumBaseFontSize / WidthDelta); }
        }
        private const double MediumSmallBaseFontSize = 24;
        public double MediumSmallFlexyFontSize
        {
            get { return Convert.ToInt16(MediumSmallBaseFontSize / WidthDelta); }
        }
        private const double SmallBaseFontSize = 20;
        public double SmallFlexyFontSize
        {
            get { return Convert.ToInt16(SmallBaseFontSize / WidthDelta); }
        }

        public virtual void WindowSizeChanged(double width, double height)
        {
            FullScreenWidth = width;
            FullScreenHeight = height;
            IsHorizontal = FullScreenWidth > FullScreenHeight;
            WidthDelta = IsHorizontal
                 ? BaseDesignLandscapeWidth / FullScreenWidth
                 : BaseDesignPortraitWidth / FullScreenWidth;

            HeightDelta = IsHorizontal
                 ? BaseDesignLandscapeHeight / FullScreenHeight
                 : BaseDesignPortraitHeight / FullScreenHeight;
            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("FullScreenHeight");
            OnPropertyChanged("IsHorizontal");
            OnPropertyChanged("WidthDelta");
            OnPropertyChanged("HeightDelta");
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
