using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.VisualBasic;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Collections;
using Windows.UI.Xaml;
using System.Globalization;
using System.Collections.ObjectModel;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Utils;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ItemBaseViewModel, IHasMediaContentViewModel
    {
        private int _exhibitsIndex;
        private Visibility _showMoreVisibility;
        public override string Id { get; set; }
        private readonly IAnalyticsService _analyticsService;

        public EventItemViewModel(Event eventModel, IAnalyticsService analyticsService = null)
        {
            if (eventModel == null)
                return;
            _analyticsService = analyticsService;
            Event = eventModel;
            Name = eventModel.Name;
            Description = eventModel.Description;
            Location = eventModel.Location;
            Id = eventModel.Id;
            SquareFootage = eventModel.SquareFootage;
            EventColor = ColorUtils.GetEventColor(eventModel);
            Exhibits = new List<ExhibitItemViewModel>(eventModel.Exhibits.Select(x => new ExhibitItemViewModel(x)));
            LoadMediaContent(eventModel.MediaContent);
            LoadDisplayedExhibits();
            LoadCuratorLogos();
            ShowMoreCommand = new DelegateCommand(ShowMoreCommandExecuteMethod, ShowMoreCommandCanExecuteMethod);
            ShowMoreVisibility = ShowMoreCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadDisplayedExhibits()
        {
            _exhibitsIndex = 0;
            if (Exhibits.Count > 4)
            {
                DisplayedExhibits = new ObservableCollection<ExhibitItemViewModel>(Exhibits.Take(3));
                DisplayedExhibits.Add(new ShowMoreFakeExhibitItemViewModel());
            }
            else
            {
                DisplayedExhibits = new ObservableCollection<ExhibitItemViewModel>(Exhibits.Take(4));
            }
        }

        /// <summary>
        /// Navigates contained Exhibits and creates a collection of their curators.
        /// No duplicate curators are added to this list.
        /// </summary>
        private void LoadCuratorLogos()
        {
            CuratorLogos = new ObservableCollection<Uri>();

            if (Exhibits == null || Exhibits.Count < 1)
                return;

            var logos = new Dictionary<string, int>();

            foreach (var exhibit in Exhibits)
            {
                if (exhibit.Curator != null && exhibit.Curator.LogoImage != null)
                    logos[exhibit.Curator.LogoImage] = 1; //Add or replace logo dictionary entry. We use the key to store the url. The value is irrelevant.
            }

            foreach (var logo in logos.Keys)
            {
                var url = new Uri(logo, UriKind.RelativeOrAbsolute);
                CuratorLogos.Add(url);
            }
        }

        private void LoadMediaContent(IEnumerable<MediaContentSource> mediaContent)
        {
            var mediaContentViewModels = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContent).ToList();
            MediaContent = new ObservableCollection<MediaContentSourceItemViewModel>(mediaContentViewModels);
            MediaContentVisibility = MediaContent.Any() ? Visibility.Visible : Visibility.Collapsed;

            if (!MediaContent.Any())
            {
                return;
            }
            //todo remove PhotoFilePath and EventHeroVideoUri
            PhotoFilePath = MediaContent.First().ImageSource;
            EventHeroVideoUri = MediaContent.First().VideoSource;
        }

        public Event Event { get; set; }

        public List<ExhibitItemViewModel> Exhibits { get; private set; }

        public ObservableCollection<Uri> CuratorLogos { get; set; }

        public string Location { get; set; }


        public DateTime DateStart { get { return Event.DateStart.GetValueOrDefault(); } }

        public Uri EventHeroVideoUri { get; set; }

        public Uri PhotoFilePath { get; set; }

        public Visibility MediaContentVisibility { get; set; }

        private ObservableCollection<MediaContentSourceItemViewModel> _mediaContent;
        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent
        {
            get
            {
                return _mediaContent;
            }
            private set
            {
                SetProperty(ref _mediaContent, value); 
            }
        }

        public bool? IsInTheFuture
        {
            get
            {
                var start = Event.DateStart;                
                if (start.HasValue)
                {
                    return DateTime.Today <= start.GetValueOrDefault().Date;
                }
                var end = Event.DateEnd;         
                if (end.HasValue)
                {
                    return DateTime.Today <= end.GetValueOrDefault().Date;
                }
                return null;
            }
        }

        public bool? IsInThePast
        {
            get
            {                
                var end = Event.DateEnd;
                if (end.HasValue)
                {
                    return end.GetValueOrDefault().Date < DateTime.Today;
                }
                var start = Event.DateStart;   
                if (start.HasValue)
                {
                    return start.GetValueOrDefault().Date < DateTime.Today;
                }
                return null;
            }
        }

        public Color EventColor { get; set; }

        public DelegateCommand ShowMoreCommand { get; private set; }

        public ObservableCollection<ExhibitItemViewModel> DisplayedExhibits { get; private set; }
        public Visibility ShowMoreVisibility
        {
            get { return _showMoreVisibility; }
            set { SetProperty(ref _showMoreVisibility, value); }
        }

        private bool ShowMoreCommandCanExecuteMethod()
        {
            return Exhibits.Count > 4;
        }

        private void ShowMoreCommandExecuteMethod()
        {
            //Track Show More interaction
            if (_analyticsService != null) 
                _analyticsService.TrackShowMoreExhibitsInEvent(this.Name);

            //load next three
            _exhibitsIndex += 3;
            if (_exhibitsIndex > Exhibits.Count - 1)
            {
                _exhibitsIndex = 0;
            }
            DisplayedExhibits.Clear();
            foreach (var ex in Exhibits.GetRange(_exhibitsIndex, Math.Min(3, Exhibits.Count - _exhibitsIndex)))
            {
                DisplayedExhibits.Add(ex);
            }
            DisplayedExhibits.Add(new ShowMoreFakeExhibitItemViewModel());
        }

        public String TileBlurb
        {
            get
            {
                return SquareFootageStringPlain + " " + Strings.SquareFeet + " " + Name + " " + Strings.In + " " + Location;
            }
        }

    }
}
