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
                        DateStart = DateTime.Now.Add(TimeSpan.FromDays(2)),
                        SquareFootage = 5000,
                        MediaContent = new List<MediaContentSource>
                        {
                            new MediaContentSource
                            {
                                ContentSourceType = ContentSourceType.Image,
                                Source = HeroImage
                            },
                            new MediaContentSource
                            {
                                ContentSourceType = ContentSourceType.Video,
                                Source = VideoUrl
                            }
                        },
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "0",
                                Name = "Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "AABBCC",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1000,
                                MediaContent = new List<MediaContentSource>
                                {
                                    new MediaContentSource
                                    {
                                        ContentSourceType = ContentSourceType.Image,
                                        Source = HeroImage
                                    }
                                }
                            }
                        }
                    },
                     new Event
                    {
                        Id="1",
                        Name = "Miami",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Miami",
                        Color = "AABBCC",     
                        DateStart = DateTime.Now.AddDays(-2),
                        SquareFootage = 10000,
                        MediaContent = new List<MediaContentSource>
                        {
                            new MediaContentSource
                            {
                                ContentSourceType = ContentSourceType.Image,
                                Source = HeroImage
                            }
                        },
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = "1",
                                Name = "Miami Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "AABBCC",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1000,
                                MediaContent = new List<MediaContentSource>
                                {
                                    new MediaContentSource
                                    {
                                        ContentSourceType = ContentSourceType.Image,
                                        Source = HeroImage
                                    }
                                }
                            },
                             new Exhibit()
                            {
                                Id = "2",
                                Name = "Miami Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "AABBCC",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1000,
                                MediaContent = new List<MediaContentSource>
                                {
                                    new MediaContentSource
                                    {
                                        ContentSourceType = ContentSourceType.Image,
                                        Source = HeroImage
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return Task.FromResult(result);
        }
    }
}
