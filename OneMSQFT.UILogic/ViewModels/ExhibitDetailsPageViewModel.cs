using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
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
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;

        public ExhibitDetailsPageViewModel(IDataService dataService, IAlertMessageService messageService)
        {
            _dataService = dataService;
            _messageService = messageService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
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

        public void WindowSizeChanged(double width, double height)
        {
        }

        public double FullScreenHeight { get; set; }


        public double FullScreenWidth { get; set; }

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
                    PopulateExhibitPhotos();
                }
            }
        }

        public Uri HeroPhotoPath
        {
            get
            {
                return Exhibit.PhotoFilePath;
            }
        }

        private void PopulateExhibitPhotos()
        {
            PhotoCollection = new ObservableCollection<Uri>();
            PhotoCollection.Add(HeroPhotoPath);
            PhotoCollection.Add(HeroPhotoPath);
            PhotoCollection.Add(HeroPhotoPath);
            PhotoCollection.Add(HeroPhotoPath);
            PhotoCollection.Add(HeroPhotoPath);
        }

        public ObservableCollection<Uri> PhotoCollection { get; set; }
    }
}