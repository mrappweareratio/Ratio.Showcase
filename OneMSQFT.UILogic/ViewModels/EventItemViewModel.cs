using System.Collections.Generic;
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
        public EventItemViewModel(Event eventModel)
        {
            Event = eventModel;
            Name = eventModel.Name;
            Description = eventModel.Description;
            Id = eventModel.Id;
        }
        private Event Event { get; set; }
        public ObservableCollection<ExhibitItemViewModel> Exhibits
        {
            get
            {
                var fakeExhibits = new ObservableCollection<ExhibitItemViewModel>();
                for (var i = 0; i < 4; i++)
                {
                    var exhibit = new ExhibitItemViewModel(new Exhibit()
                    {
                        Id = i.ToString(),
                        Name = "Exhibit Name " + i,
                        Description = "Exhibit Description Name " + i,
                        PhotoFilePath = "http://www.1msqft.com/assets/img/cultivators/sundance/laBlogo/1.jpg",
                        SquareFootage = i*1234+123
                    });
                    fakeExhibits.Add(exhibit);
                }
                return fakeExhibits;
            }
        }
        public DateTime DateStart { get { return Event.DateStart; } }
        public String SquareFootage
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", Event.SquareFootage)).Trim();
            }
        }
        public Uri PhotoFilePath
        {
            get
            {
                if (Event.PhotoFilePath != null)
                {
                    return new Uri(Event.PhotoFilePath, UriKind.Absolute);
                }
                return new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
            }
        }
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

        public double ItemWidth { get { return Window.Current.Bounds.Width; } }
        public double ItemHeight { get { return Window.Current.Bounds.Width; } }
        public double ZoomedOutItemWidth { get { return Window.Current.Bounds.Width / 6; } }
        public double ZoomedOutItemHeight { get { return Window.Current.Bounds.Height / 4; } }
    }
}
