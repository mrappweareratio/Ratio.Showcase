using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Collections;

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
        public int SquareFootage { get { return Event.SquareFootage; } }
        public Uri PhotoFilePath { get { return new Uri(Event.PhotoFilePath, UriKind.Absolute); } }
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
    }
}
