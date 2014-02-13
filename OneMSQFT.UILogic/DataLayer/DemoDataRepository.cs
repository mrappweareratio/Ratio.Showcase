﻿using System;
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
        private static readonly Random ImageRandomizer = new Random();
        private static readonly List<string> Images = new List<string>()
        {
            "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/1.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/2.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/3.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/4.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/5.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/6.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/7.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/8.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/9.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/10.jpg",
            "http://www.1msqft.com/assets/img/carousel/2/11.jpg"
        };

        private const string VideoUrl = "http://player.vimeo.com/external/85202186.hd.mp4?s=a78dd60b6d00d4ea3cb24c04f8123fc5";

        private const string DemoExhibitDescription = "";

        public Task<SiteDataResult> GetSiteData()
        {
            var exhibitId = 0;
            var eventId = 0;
            var result = new SiteDataResult
            {
                Events = new List<Event>
                {
                     new Event
                    {
                          
                        Id = Convert.ToString(eventId++),
                        Name = "Sundance",
                        DisplayDate = "MMM D-D",
                        Description = "Sundance Film Festival",
                        Color = "CE7C0D",                      
                        DateStart = DateTime.Now.AddDays(2),
                        SquareFootage = 5647,
                        MediaContent = GetMediaCollection(2),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Sundance Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "CE7C0D",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1946,
                                MediaContent = GetMediaCollection(1),
                            },
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Sundance Exhibit One",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "CE7C0D",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2236,
                                MediaContent = GetMediaCollection(1),
                            }
                        }
                    },
                     new Event
                    {
                       Id = Convert.ToString(eventId++),
                        Name = "Miami",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Miami",
                        Color = "306D2C",     
                        DateStart = DateTime.Now.AddDays(-2),
                        SquareFootage = 7845,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Miami Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "306D2C",
                                Description = DemoExhibitDescription,
                                SquareFootage = 560,
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Miami Exhibit One",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "306D2C",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1070,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Miami Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Color = "306D2C",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2340,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                        Id = Convert.ToString(eventId++),
                        Name = "Fashion Week",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Fasion Week",
                        Color = "922580",     
                        DateStart = DateTime.Now.AddDays(-6),
                        SquareFootage = 2587,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Fasion Week Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                Color = "922580",
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Fasion Week Exhibit One",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                Color = "922580",
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Fasion Week Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                Color = "922580",
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Fasion Week Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                Color = "922580",
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                       Id = Convert.ToString(eventId++),
                        Name = "New Jersey",
                        DisplayDate = "MMM D-D",
                        Description = "Event at New Jersey",
                        Color = "555FE4",     
                        DateStart = DateTime.Now.AddDays(6),
                        SquareFootage = 6587,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "New Jersey Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Color = "555FE4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "New Jersey Exhibit One",
                                DisplayDate = "MMM D-D",
                                Color = "555FE4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "New Jersey Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Color = "555FE4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "New Jersey Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Color = "555FE4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                        Id = Convert.ToString(eventId++),
                        Name = "Dallas",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Dallas",
                        Color = "4CC661",     
                        DateStart = DateTime.Now.AddDays(10),
                        SquareFootage = 984,
                        MediaContent = GetMediaCollection(1),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Dallas Exhibit Zero",
                                  DisplayDate = "MMM D-D",
                                Color = "4CC661",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                          Id = Convert.ToString(exhibitId++),
                                Name = "Dallas Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Color = "4CC661",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Dallas Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Color = "4CC661",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                              Id = Convert.ToString(exhibitId++),
                                Name = "Dallas Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Color = "4CC661",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(1),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Dallas Exhibit Four",
                                DisplayDate = "MMM D-D",
                                Color = "4CC661",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                       Id = Convert.ToString(eventId++),
                        Name = "Portland",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Portland",
                        Color = "6357E4",     
                        DateStart = DateTime.Now.AddDays(-10),
                        SquareFootage = 984,
                        MediaContent = GetMediaCollection(3),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Portland Exhibit Zero",
                                  DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                                Id = Convert.ToString(exhibitId++),
                                Name = "Portland Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2345,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Portland Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Portland Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(1),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Portland Exhibit Four",
                                DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            }
                        }
                    },
                     new Event
                    {
                       Id = Convert.ToString(eventId++),
                        Name = "Nashville",
                        DisplayDate = "MMM D-D",
                        Description = "Event at Nashville",
                        Color = "CB4D3A",     
                        DateStart = DateTime.Now.AddDays(25),
                        SquareFootage = 3984,
                        MediaContent = GetMediaCollection(3),
                        Exhibits = new List<Exhibit>
                        {
                            new Exhibit()
                            {
                              Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit Zero",
                                DisplayDate = "MMM D-D",
                                Color = "CB4D3A",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 215,
                                MediaContent = GetMediaCollection(2),
                            },
                            new Exhibit()
                            {
                              Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit One",
                                  DisplayDate = "MMM D-D",
                                Color = "CB4D3A",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 2945,
                                MediaContent = GetMediaCollection(2),
                            },
                             new Exhibit()
                            {
                             Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit Two",
                                DisplayDate = "MMM D-D",
                                Color = "CB4D3A",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 1279,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                             Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit Three",
                                DisplayDate = "MMM D-D",
                                Color = "6357E4",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(1),
                            },
                             new Exhibit()
                            {
                               Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit Four",
                                DisplayDate = "MMM D-D",
                                Color = "CB4D3A",
                                Exhibitor = "Exhibitor",
                                Description = DemoExhibitDescription,
                                SquareFootage = 4578,
                                MediaContent = GetMediaCollection(3),
                            },
                             new Exhibit()
                            {
                              Id = Convert.ToString(exhibitId++),
                                Name = "Nashville Exhibit Four",
                                DisplayDate = "MMM D-D",
                                Color = "CB4D3A",
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


        public static List<MediaContentSource> GetMediaCollection(int count)
        {
            var mc = new List<MediaContentSource>();            
            for (var i = 0; i < count; i++)
            {
                var img = ImageRandomizer.Next(Images.Count - 1);                
                var mcsV = new MediaContentSource()
                {
                    ContentSourceType = ContentSourceType.Video,
                    Source = VideoUrl

                };
                var mcsI = new MediaContentSource()
                {
                    ContentSourceType = ContentSourceType.Image,
                    Source = Images[img]
                };
                mc.Add(mcsV);
                mc.Add(mcsI);
            }
            return mc;
        }
    }
}