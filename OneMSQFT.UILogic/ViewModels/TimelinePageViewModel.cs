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
        public TimelinePageViewModel()
        {
            SquareFootFutureEvents = new ObservableCollection<EventItemViewModel>();
            SquareFootPastEvents = new ObservableCollection<EventItemViewModel>();
            var FakeEventsCount = 6;
            for (var i = 0; i < FakeEventsCount; i++)
            {
                var eivm = new EventItemViewModel(new Event()
                {
                    Color = i.ToString()+i.ToString()+i.ToString()+i.ToString()+i.ToString()+i.ToString(),
                    Description = "Description Description " + i,
                    Id = i.ToString(),
                    Name = "Event Name " + i,
                    DateStart = DateTime.Now.Add(TimeSpan.FromDays(i * (i > FakeEventsCount / 2 ? 20 : -20) + 1))
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
                    TimeLineItems.Add(e);
                }
                TimeLineItems.Add(new EventItemViewModel(new Event() { Name = "Home" }));
                foreach (var e in SquareFootFutureEvents)
                {
                    TimeLineItems.Add(e);
                }
                return c as ObservableCollection<EventItemViewModel>;
            }
        }

    }
}
