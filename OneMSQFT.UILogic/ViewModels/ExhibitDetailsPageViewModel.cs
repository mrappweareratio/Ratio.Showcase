using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Core;
using OneMSQFT.Common.Services;
using System;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitDetailsPageViewModel : BasePageViewModel, IExhibitDetailsPageViewModel
    {
        public TaskCompletionSource<bool> LoadingTaskCompletionSource { get; set; }
        public DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; set; }
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;

        public ExhibitDetailsPageViewModel(IDataService dataService, IAlertMessageService messageService)
        {
            _dataService = dataService;
            _messageService = messageService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
        }

        async public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {

            LoadingTaskCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                ExhibitDetail ed = await _dataService.GetExhibitDetailByExhibitId(navigationParameter as string);
                Exhibit = new ExhibitItemViewModel(ed.Exhibit);
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            }
            finally
            {
                LoadingTaskCompletionSource.SetResult(true);
            }
        }

        async public void LaunchVideoCommandHandler(MediaContentSourceItemViewModel item)
        {
            if (item == null) return;
            SelectedMediaContentSource = item;
        }

        private void PopulateExhibitMediaCollection()
        {
            MediaContentCollection = new ObservableCollection<MediaContentSourceItemViewModel>();

            MediaContentCollection.Add(new MediaContentSourceItemViewModel(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            }));
            MediaContentCollection.Add(new MediaContentSourceItemViewModel(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4",
                ThumbnailSource = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/3.jpg"
            }));
            MediaContentCollection.Add(new MediaContentSourceItemViewModel(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            }));
            MediaContentCollection.Add(new MediaContentSourceItemViewModel(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Source = "http://smf.blob.core.windows.net/samples/videos/wildlife.mp4",
                ThumbnailSource = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/3.jpg"
            }));
        }

        #region ContentProperties

        private MediaContentSourceItemViewModel _selectedMediaContentSource;
        public MediaContentSourceItemViewModel SelectedMediaContentSource
        {
            get
            {
                return _selectedMediaContentSource;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMediaContentSource, value);
                }
            }
        }

        public SolidColorBrush EventColor
        {
            get
            {
                return Exhibit.EventColor;
            }
        }

        private ExhibitItemViewModel _exhibit;
        public ExhibitItemViewModel Exhibit
        {
            get
            {
                return _exhibit;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _exhibit, value);
                    PopulateExhibitMediaCollection();
                }
            }
        }

        public String Panel0Title
        {
            get
            {
                return Exhibit.SquareFootage + " square feet at " + Exhibit.Name;
            }
        }

        public String Panel0Description
        {
            get
            {
                return Exhibit.Description;
            }
        }

        public Uri HeroPhotoFilePath
        {
            get
            {
                return Exhibit.HeroPhotoFilePath;
            }
        }

        public String Panel1LongDescription
        {
            get
            {
                return Exhibit.Description;
            }
        }      

        public ObservableCollection<MediaContentSourceItemViewModel> MediaContentCollection { get; set; }


        #endregion

        #region ResizingProperties

        public double FullScreenWidth
        {
            get
            {
                return Window.Current.Bounds.Width;
            }
        }
        public double FullScreenHeight
        {
            get
            {
                return Window.Current.Bounds.Height;
            }
        }

        public double ExhibitItemWidth
        {
            get
            {
                return Window.Current.Bounds.Width * .9;
            }
        }

        public double ExhibitItemHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double OneThirdPanelWidth
        {
            get { return ExhibitItemWidth / 3; }
        }

        public void WindowSizeChanged(double width, double height)
        {
            OnPropertyChanged("FullScreenHeight");
            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ExhibitItemWidth");
        }

        #endregion

       
    }
}