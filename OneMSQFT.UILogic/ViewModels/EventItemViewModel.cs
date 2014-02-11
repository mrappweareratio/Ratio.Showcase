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
        public override string Id { get; set; }

        public EventItemViewModel(Event eventModel)
        {
            Event = eventModel;
            Name = eventModel.Name;
            Description = eventModel.Description;
            Id = eventModel.Id;
            SquareFootage = eventModel.SquareFootage;
            Exhibits = new ObservableCollection<ExhibitItemViewModel>(eventModel.Exhibits.Select(x => new ExhibitItemViewModel(x)));
            LoadMediaContent(eventModel.MediaContent);
            EventColor = ColorUtils.GetEventColor(eventModel);
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

        public ObservableCollection<ExhibitItemViewModel> Exhibits { get; private set; }

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
    }
}
