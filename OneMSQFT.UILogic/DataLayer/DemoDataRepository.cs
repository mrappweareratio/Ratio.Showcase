using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.DataLayer
{
    public class DemoDataRepository : IDataRepository
    {
        private const string HeroImage = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg";
        private const string VideoUrl = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4";
        private const string DemoExhibitDescription = "";

        public Task<SiteDataResult> GetSiteData()
        {
            var result = new SiteDataResult
            {
                Events = new List<Event>
                {
                    new Event
                    {
                          
                        Id = "0",
                        Name = "Sundance",
                        DisplayDate = "MMM D-D",
                        Description = "Sundance Film Festival",
                        Color = "AABBCC",                      
                        DateStart = DateTime.Now.AddDays(2),
                        SquareFootage = 5647,
                        MediaContent = GetMediaCollection(2),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "0",
                                Name = "Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1236,
                                MediaContent = GetMediaCollection(1),
                            }
                        }
                    },
                     new Event
                    {
                        Id="1",
                        Name = "Miami",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Miami",
                        Color = "11FF00",     
                        DateStart = DateTime.Now.AddDays(-2),
                        SquareFootage = 7845,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "1",
                                Name = "Miami Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1000,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                                Id = "2",
                                Name = "Miami Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1000,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                        Id="3",
                        Name = "New York",
                        DisplayDate = "MMM D-D",
                        Description = "Event at New York",
                        Color = "6699AA",     
                        DateStart = DateTime.Now.AddDays(-6),
                        SquareFootage = 2587,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "1",
                                Name = "New York Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                                Id = "2",
                                Name = "New York Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                                Id = "3",
                                Name = "New York Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                        Id="4",
                        Name = "New Jersy",
                        DisplayDate = "MMM D-D",
                        Description = "Event at New Jersy",
                        Color = "1122FF",     
                        DateStart = DateTime.Now.AddDays(6),
                        SquareFootage = 9587,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "1",
                                Name = "New Jersy Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                                Id = "2",
                                Name = "New Jersy Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                                Id = "3",
                                Name = "New Jersy Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    }
                }
            };
            return Task.FromResult(result);
        }


        public List<MediaContentSource> GetMediaCollection(int count)
        {
            var mc = new List<MediaContentSource>();
            for (var i = 0; i < count; i++)
            {
                var mcsI = new MediaContentSource()
                {
                    ContentSourceType = ContentSourceType.Image,
                    Source = HeroImage
                    
                };
                var mcsV = new MediaContentSource()
                {
                    ContentSourceType = ContentSourceType.Video,
                    Source = VideoUrl

                };
                mc.Add(mcsI);
                mc.Add(mcsV);
            }
            return mc;
        }
    }
}
