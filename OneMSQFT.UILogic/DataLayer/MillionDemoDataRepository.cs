using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.DataLayer
{
    public class MillionDemoDataRepository : IDataRepository
    {
        private static readonly Random Randomizer = new Random();


        private const string DemoExhibitDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis condimentum laoreet magna, ut rutrum purus cursus ut. Nulla convallis, elit et consequat sollicitudin, eros ligula pretium dolor, a bibendum est eros eu mauris. Sed in mi cursus, dapibus justo et, condimentum est. Vestibulum velit eros, rhoncus a ante sed, tincidunt scelerisque nibh. Maecenas ullamcorper lacus quis justo aliquet iaculis at placerat nulla. Phasellus dictum lectus vel erat vehicula cursus. Suspendisse euismod consequat rhoncus. Duis id lectus orci.";
        private const string DemoEventDescription = DemoExhibitDescription;

        private int _totalSquareFeet;
        private int _id;
        private const int MinImages = 5;
        private const int MaxImages = 20;
        private const int FullImageWidth = 1280;
        private const int FullImageHeight = 720;
        private const int ExhibitImageWidth = 581;
        private const int ExhibitImageHeight = 327;
        private const int MinExhibitSqFt = 500;
        private const int MaxExhibitSqFt = 1500;
        private const int MinExhibits = 3;
        private const int MaxExhibits = 10;
        private const int ThemeCount = 10;

        async public Task<SiteData> GetSiteData(CancellationToken token)
        {
            var siteData = new SiteData();
            var events = new List<Event>();
            while (_totalSquareFeet < 200000)
            {
                _id++;
                var ev = MakeNewEvent();
                events.Add(ev);
                _totalSquareFeet += ev.SquareFootage;
            }
            var themes = MakeThemes(ThemeCount).ToList();
            foreach (var evt in events)
            {
                var theme = themes.FirstOrDefault(x => x.Id.Equals(evt.ThemeId));
                if (theme == null) continue;
                evt.Color = theme.Color;
                if (String.IsNullOrEmpty(evt.Color)) continue;
                foreach (var ex in evt.Exhibits)
                {
                    //persist event color to exhibits
                    ex.Color = evt.Color;
                    //persist event id to exhibits
                    ex.EventId = ex.EventId ?? evt.Id;
                    ex.DateEnd = ex.DateEnd ?? evt.DateEnd;
                    //persist event dates to exhibits
                    ex.DateStart = ex.DateStart ?? evt.DateStart;
                }
            }
            siteData.Events = events;
            siteData.Themes = themes;
            return siteData;
        }

        private IEnumerable<Theme> MakeThemes(int count)
        {
            List<Theme> themes = new List<Theme>();
            for (var i = 0; i < count; i++)
            {
                var theme = new Theme() { Id = i.ToString() };
                theme.Color = Randomizer.Next(100000, 999999).ToString();
                themes.Add(theme);
            }
            return themes;
        }

        private Event MakeNewEvent()
        {
            var id = _id++;
            var theme = Randomizer.Next(ThemeCount);
            var ev = new Event()
            {
                Id = id.ToString(),
                Name = String.Format("Event {0}", id),
                ThemeId = theme.ToString(),
                Description = DemoEventDescription,
                DateStart = id % 2 == 0 ? DateTime.Now.AddDays(id) : DateTime.Now.AddDays(-id)
            };
            ev.MediaContent = GetMediaCollection(Randomizer.Next(MinImages, MaxImages));
            ev.Exhibits = MakeNewExhibits();
            ev.SquareFootage = ev.Exhibits.Sum(x => x.SquareFootage);
            return ev;
        }

        private IEnumerable<Exhibit> MakeNewExhibits()
        {
            var retVal = new List<Exhibit>();
            var count = Randomizer.Next(MinExhibits, MaxExhibits);
            for (int i = 0; i < count; i++)
            {
                var id = _id++;
                var theme = Randomizer.Next(ThemeCount);
                var exhibit = new Exhibit()
                {
                    Id = id.ToString(),
                    Name = String.Format("Event {0}", id),
                    Description = DemoEventDescription,
                    DateStart = id % 2 == 0 ? DateTime.Now.AddDays(id) : DateTime.Now.AddDays(-id),
                    RsvpUrl = "http://rsvp.url",
                    ThumbImage = string.Format("http://placehold.it/{0}x{1}/text=Exhibit+{2}", ExhibitImageWidth, ExhibitImageHeight, id)
                };
                exhibit.MediaContent = GetMediaCollection(Randomizer.Next(MinImages, MaxImages));
                exhibit.SquareFootage = Randomizer.Next(MinExhibitSqFt, MaxExhibitSqFt);
                retVal.Add(exhibit);
            }
            return retVal;
        }


        public static List<MediaContentSource> GetMediaCollection(int count)
        {
            var mc = new List<MediaContentSource>();
            for (var i = 0; i < count; i++)
            {
                var image = new MediaContentSource()
                {
                    ContentSourceType = ContentSourceType.Image,
                    Img = String.Format("http://placehold.it/{0}x{1}&text={2}+of+{3}", FullImageWidth, FullImageHeight, i, count),
                    SortOrder = i
                };
                mc.Add(image);
            }
            return mc;
        }
    }
}
