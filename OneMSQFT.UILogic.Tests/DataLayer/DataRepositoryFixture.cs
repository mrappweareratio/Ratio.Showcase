using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class DataRepositoryFixture
    {
        [TestInitialize]
        public void Init()
        {
            //this.DataRepository = new SampleDataRepository();
            this.DataRepository = new DemoDataRepository();
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
            var e = result.Events.FirstOrDefault();
            var validEvents = e.Exhibits.ToList().TrueForAll(ValidateExhibit);
            Assert.IsTrue(validEvents, "ValidEvents");
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
        public bool ValidateExhibit(IExhibit e)
        {
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Exhibitor, "Exhibitor");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            Assert.IsNotNull(e.DisplayDate, "DisplayDate");
            Assert.IsNotNull(e.Description, "Description");
            //Assert.IsNotNull(e.DateStart, "DateStart");
            //Assert.IsNotNull(e.DateEnd, "DateEnd");
            //Assert.IsNotNull(e.CreatedAt, "CreatedAt");
            //Assert.IsNotNull(e.UpdatedAt, "UpdatedAt"); 

            return true;
        }


        public bool ValidateEvent(IEvent<IExhibit> e)
        {
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Description, "Description");
            Assert.IsNotNull(e.Color, "Color");
            Assert.IsTrue(e.Color.Length == 6, "Color is Hex");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            Assert.IsNotNull(e.DisplayDate, "DisplayDate");            
            Assert.IsNotNull(e.DateStart, "DateStart");
            Assert.IsNotNull(e.DateEnd, "DateEnd");
            //Assert.IsNotNull(e.CreatedAt, "CreatedAt");
            //Assert.IsNotNull(e.UpdatedAt, "UpdatedAt");
            //Assert.IsNotNull(e.Longitude, "Longitude");
            //Assert.IsNotNull(e.Lattitude, "Lattitude");
            //Assert.IsNotNull(e.SocialMediaTitle, "SocialMediaTitle");
            //Assert.IsNotNull(e.SocialMediaDescription, "SocialMediaDescription");
            //Assert.IsNotNull(e.SocialMediaImagePath, "SocialMediaImagePath");                        
            Assert.IsNotNull(e.Exhibits, "Exhibits");

            return true;
        }
    }
}
