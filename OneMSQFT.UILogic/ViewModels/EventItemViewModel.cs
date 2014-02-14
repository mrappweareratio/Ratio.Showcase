using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Collections;
using Windows.UI.Xaml;
using System.Globalization;
using System.Collections.ObjectModel;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ItemBaseViewModel, IHasMediaContentViewModel
    {
        private int _exhibitsIndex;
        private Visibility _showMoreVisibility;
        public override string Id { get; set; }

        public EventItemViewModel(Event eventModel)
        {
            Event = eventModel;
            Name = eventModel.Name;
            Description = eventModel.Description;
            Id = eventModel.Id;
            SquareFootage = eventModel.SquareFootage;
            _exhibitsIndex = 0;
            Exhibits = new List<ExhibitItemViewModel>(eventModel.Exhibits.Select(x => new ExhibitItemViewModel(x)));
            DisplayedExhibits = new ObservableCollection<ExhibitItemViewModel>(Exhibits.Count > 4
                ? Exhibits.Take(3)
                : Exhibits.Take(4));                        
            LoadMediaContent(eventModel.MediaContent);
            EventColor = ColorUtils.GetEventColor(eventModel);
            ShowMoreCommand = new DelegateCommand(ShowMoreCommandExecuteMethod);
            ShowMoreVisibility = ShowMoreCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;

            if (DisplayedExhibits.Count == 3)
            {
                DisplayedExhibits.Add(new ShowMoreFakeExhibitItemViewModel(new Exhibit(), true));
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

        private Event Event { get; set; }

        public List<ExhibitItemViewModel> Exhibits { get; private set; }

        public DateTime DateStart { get { return Event.DateStart; } }

        public Uri EventHeroVideoUri { get; set; }

        public Uri PhotoFilePath { get; set; }

        public Visibility MediaContentVisibility { get; set; }

        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; private set; }

        public bool IsInTheFuture
        {
            get
            {
                return (Event.DateStart > DateTime.Now);
            }
        }

        public SolidColorBrush EventColor { get; set; }

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
            DisplayedExhibits.Add(new ShowMoreFakeExhibitItemViewModel(new Exhibit(), true));
        }
    }
}
