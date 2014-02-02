using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;
using OneMSQFT.Common.Models;
using System;

namespace OneMSQFT.UILogic.ViewModels
{
    public class TimelinePageViewModel : BasePageViewModel
    {
        public int TotalSquareFeet { get; set; }
        public TimelinePageViewModel()
        {
            TotalSquareFeet = 36482;
            SquareFootFutureEvents = new ObservableCollection<EventItemViewModel>();
            SquareFootPastEvents = new ObservableCollection<EventItemViewModel>();
            var FakeEventsCount = 60;
            for (var i = 0; i < FakeEventsCount; i++)
            {
                var eivm = new EventItemViewModel(new Event()
                {
                    Color = (i*2).ToString()+i.ToString()+(i*2).ToString()+i.ToString()+i.ToString()+i.ToString(),
                    Description = "Description Description " + i,
                    Id = i.ToString(),
                    Name = "Event Name " + i,
                    DateStart = DateTime.Now.Add(TimeSpan.FromDays(i * (i > FakeEventsCount / 2 ? 20 : -20) + 1)),
                    PhotoFilePath = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg",
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString())
                });
                if (eivm.IsInTheFuture == true) { SquareFootFutureEvents.Add(eivm); } else { SquareFootPastEvents.Insert(0, eivm); }
            }

        }
        public ObservableCollection<EventItemViewModel> SquareFootFutureEvents { get; set; }
        public ObservableCollection<EventItemViewModel> SquareFootPastEvents { get; set; }
        public ObservableCollection<EventItemViewModel> TimeLineItems
        {
            get
            {
                var c = new ObservableCollection<EventItemViewModel>();
                foreach (var e in SquareFootPastEvents)
                {
                    c.Add(e);
                }
                c.Add(new EventItemViewModel(new Event() { 
                    Name = "Home", 
                    Color="FFFFFF",
                    SquareFootage = TotalSquareFeet
                    }));
                foreach (var e in SquareFootFutureEvents)
                {
                    c.Add(e);
                }
                return c as ObservableCollection<EventItemViewModel>;
            }
        }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems
        {
            get
            {
                var c = new ObservableCollection<EventItemViewModel>();
                foreach (var e in TimeLineItems)
                {
                    c.Add(e);
                    c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                    c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                }
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                return c as ObservableCollection<EventItemViewModel>;
        } }
    }
}
