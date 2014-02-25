using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class DataRepositoryFixture
    {
        [TestInitialize]
        public void Init()
        {
            this.DataRepository = new DemoDataRepository();
            //this.DataRepository = new ApiDataRepository(new MockApiConfiguration("http://1msqft-stage.azurewebsites.net/api"));
        }

        public IDataRepository DataRepository { get; set; }

        //TIMELINE AND EVENT TESTS

        [TestMethod]
        async public Task DataRepository_TimlineResult_Not_Null()
        {
            var result = await DataRepository.GetSiteData();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        async public Task DataRepository_TimlineResult_Events_Not_Null()
        {
            var result = await DataRepository.GetSiteData();
            Assert.IsNotNull(result.Events);
        }

        [TestMethod]
        async public Task DataRepository_TimlineResult_Events_Has_One()
        {
            var result = await DataRepository.GetSiteData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e);
        }

        [TestMethod]
        async public Task DataRepository_TimlineResult_Events_Are_Valid()
        {
            var result = await DataRepository.GetSiteData();
            var validEvents = result.Events.ToList().TrueForAll(ValidateEvent);            
            Assert.IsTrue(validEvents, "ValidEvents");
        }        

        //EXHIBIT TESTS

        [TestMethod]
        async public Task DataRepository_TimlineResult_Exhibits_Not_Null()
        {
            var result = await DataRepository.GetSiteData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e.Exhibits);
        }

        [TestMethod]
        async public Task DataRepository_TimlineResult_Exhibits_HasOne()
        {
            var result = await DataRepository.GetSiteData();
            var e = result.Events.FirstOrDefault();
            var exhibit = e.Exhibits.FirstOrDefault();
            Assert.IsNotNull(exhibit);
        }

        [TestMethod]
        async public Task DataRepository_TimlineResult_Exhibits_Are_Valid()
        {
            var result = await DataRepository.GetSiteData();
            var exhibits = result.Events.SelectMany(x => x.Exhibits).ToList();
            Assert.IsTrue(exhibits.TrueForAll(ValidateExhibit), "ValidateExhibit");
        }

        [TestMethod]
        async public Task DataRepository_Events_Unique()
        {
            var result = await DataRepository.GetSiteData();
            var events = result.Events.ToList();
            var uniqueEvents = result.Events.Select(x => x.Id).Distinct().Count();            
            Assert.AreEqual(events.Count, uniqueEvents,  "All Events Unique");
        }

        [TestMethod]
        async public Task DataRepository_Exhibits_Unique()
        {
            var result = await DataRepository.GetSiteData();
            var exhibits = result.Events.SelectMany(x => x.Exhibits).ToList();
            var uniqueEvents = exhibits.Select(x => x.Id).Distinct().Count();
            Assert.AreEqual(exhibits.Count, uniqueEvents, "All Exhibits Unique");
        }


        //HELPER METHODS
        public static bool ValidateExhibit(IExhibit<ICurator> e)
        {
            //TODO: Uncomment all required properties
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            //Assert.IsNotNull(e.Exhibitor, "Exhibitor");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            //Assert.IsNotNull(e.DisplayDate, "DisplayDate");
            Assert.IsNotNull(e.Description, "Description");
            Assert.IsNotNull(e.ThumbImage, "ThumbImage");
            Assert.IsNotNull(e.MediaContent, "MediaContent");
            Assert.IsTrue(ValidateMediaContent(e.MediaContent), "ValidateMediaContent");
            Assert.IsNotNull(e.Curator, "Curator");
            Assert.IsTrue(ValidateCurator(e.Curator), "ValidateCurator");
            if (e.Links != null)
                Assert.IsTrue(ValidateLinks(e.Links), "ValidateLinks");
            //Assert.IsNotNull(e.Links);
            if (e.RsvpUrl != null)
                Assert.IsTrue(Uri.IsWellFormedUriString(e.RsvpUrl, UriKind.RelativeOrAbsolute), "RsvpUrl");
            //Assert.IsNotNull(e.DateStart, "DateStart");
            //Assert.IsNotNull(e.DateEnd, "DateEnd");

            return true;
        }

        public static bool ValidateLinks(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                Assert.IsNotNull(link.Title);
                Assert.IsNotNull(link.Url);
                Assert.IsTrue(Uri.IsWellFormedUriString(link.Url, UriKind.RelativeOrAbsolute));
            }

            return true;
        }

        public static bool ValidateMediaContent(IEnumerable<MediaContentSource> mediaContent)
        {
            foreach (var media in mediaContent)
            {
                Assert.IsNotNull(media.Img, "Media.Img");
                Assert.IsTrue(media.SortOrder > 0, "Media.SortOrder");
                Assert.IsNotNull(media.ContentSourceType, "Media.ContentSourceType");
                if (media.ContentSourceType == ContentSourceType.Video)
                {
                    Assert.IsTrue(MediaContentSourceUtils.HasVideoUrl(media), "Media HasVideoUrl");
                }
            }

            return true;
        }

        public static bool ValidateCurator(ICurator curator)
        {
            Assert.IsNotNull(curator.Name,"Curator.Name");
            Assert.IsNotNull(curator.WhiteLogoImage,"Curator.WhiteLogoImage");
            Assert.IsNotNull(curator.Id,"Curator.Id");

            return true;
        }


        public static bool ValidateEvent(IEvent<IExhibit<ICurator>> e)
        {
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Description, "Description");            
            Assert.IsNotNull(e.Color, "Color");
            Assert.IsTrue(e.Color.Length == 6, "Color is Hex");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            Assert.IsNotNull(e.DisplayDate, "DisplayDate");
            Assert.IsNotNull(e.MediaContent, "MediaContent");
            Assert.IsTrue(ValidateMediaContent(e.MediaContent), "ValidateMediaContent");
            //Assert.IsNotNull(e.DateStart, "DateStart");
            //Assert.IsNotNull(e.DateEnd, "DateEnd");
            //Assert.IsNotNull(e.CreatedAt, "CreatedAt");
            //Assert.IsNotNull(e.UpdatedAt, "UpdatedAt");
            //Assert.IsNotNull(e.Longitude, "Longitude");
            //Assert.IsNotNull(e.Lattitude, "Lattitude");
            //Assert.IsNotNull(e.SocialMediaTitle, "SocialMediaTitle");
            //Assert.IsNotNull(e.SocialMediaDescription, "SocialMediaDescription");
            Assert.IsNotNull(e.Exhibits, "Exhibits");

            return true;
        }
    }
}
