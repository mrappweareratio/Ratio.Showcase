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

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ViewModel
    {
        public EventItemViewModel(Event eventModel)
        {
            Event = eventModel;
        }
        private Event Event { get; set; }
        public String Name { get { return Event.Name; } }
        public String Description { get { return Event.Description; } }
        public IList<Curator> Curators { get { return Event.Curators; } }
        public DateTime DateStart { get { return Event.DateStart; } }
        public String Id { get { return Event.Id; } }
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
