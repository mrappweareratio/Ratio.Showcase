using System;
using System.Collections.ObjectModel;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.DesignViewModels
{
    public class DesignTimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        public int TotalSquareFeet { get; set; }
        public DesignTimelinePageViewModel()
        {
            TotalSquareFeet = 36482;
            SquareFootFutureEvents = new ObservableCollection<EventItemViewModel>();
            SquareFootPastEvents = new ObservableCollection<EventItemViewModel>();
            const int fakeEventsCount = 10;
            for (var i = 1; i < fakeEventsCount+1; i++)
            {
                var eivm = new EventItemViewModel(new Event()
                {
                    Color = (i*2).ToString()+i.ToString()+(i*2).ToString()+i.ToString()+i.ToString()+i.ToString(),
                    Description = "Description Description " + i,
                    Id = i.ToString(),
                    Name = "Event Name " + i,
                    DateStart = DateTime.Now.Add(TimeSpan.FromDays(i * (i > fakeEventsCount / 2 ? 20 : -20) + 1)),
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

        private double _zoomedOutItemWidth;
        private double _zoomedOutItemHeight;
        public double ZoomedOutItemWidth
        {
            get
            {
                return _zoomedOutItemWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _zoomedOutItemWidth, value);
                }
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return _zoomedOutItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _zoomedOutItemHeight, value);
                }
            }
        }

        public void WindowSizeChanged(double width, double height)
        {
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
        }
    }
}
