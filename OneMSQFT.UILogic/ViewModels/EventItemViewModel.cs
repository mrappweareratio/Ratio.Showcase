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

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ItemBaseViewModel
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
            LoadImages(eventModel.MediaContent);
        }


        private void LoadImages(IEnumerable<MediaContentSource> mediaContent)
        {
            //todo revert test image code
            if (mediaContent == null)
            {
                PhotoFilePath = new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
                EventHeroVideoUri = new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
                return;
            }

            var mediaContentSources = mediaContent as MediaContentSource[] ?? mediaContent.ToArray();
            var images = mediaContentSources.Where(x => x.ContentSourceType == ContentSourceType.Image);

            var firstImage = images.FirstOrDefault(x => x.ContentSourceType == ContentSourceType.Image);
            PhotoFilePath = firstImage == null ? new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute) : new Uri(firstImage.Source, UriKind.Absolute);
            
            var firstVideo = mediaContentSources.FirstOrDefault(x => x.ContentSourceType == ContentSourceType.Video);
            EventHeroVideoUri = firstVideo == null ? new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute) : new Uri(firstVideo.Source, UriKind.Absolute);

        }
        
        private Event Event { get; set; }
    
        public ObservableCollection<ExhibitItemViewModel> Exhibits { get; private set; }
        
        public DateTime DateStart { get { return Event.DateStart; } }

        public Uri EventHeroVideoUri { get; set; }        

        public Uri PhotoFilePath { get; set; }
        
        public bool IsInTheFuture
        {
            get
            {
                return (Event.DateStart > DateTime.Now);
            }
        }

        public SolidColorBrush EventColor
        {
            get
            {
                string color = "FF" + Event.Color;
                var c = Color.FromArgb(
                Convert.ToByte(color.Substring(0, 2), 16),
                Convert.ToByte(color.Substring(2, 2), 16),
                Convert.ToByte(color.Substring(4, 2), 16),
                Convert.ToByte(color.Substring(6, 2), 16));

                return new SolidColorBrush(c);
            }
        }

        //public double ItemWidth { get { return Window.Current.Bounds.Width; } }
        //public double ItemHeight { get { return Window.Current.Bounds.Width; } }
    }
}
