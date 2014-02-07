using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class SampleDataRepositoryFixture
    {
        //TIMELINE AND EVENT TESTS

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Not_Null()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Not_Null()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result.Events);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Has_One()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Are_Valid()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var validEvents = result.Events.ToList().TrueForAll(ValidateEvent);            
            Assert.IsTrue(validEvents, "ValidEvents");
        }

        //EXHIBIT TESTS

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Exhibits_Not_Null()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e.Exhibits);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Exhibits_HasOne()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            var exhibit = e.Exhibits.FirstOrDefault();
            Assert.IsNotNull(exhibit);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Exhibits_Are_Valid()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            var validEvents = e.Exhibits.ToList().TrueForAll(ValidateExhibit);
            Assert.IsTrue(validEvents, "ValidEvents");
        }

        private bool ValidateExhibit(Exhibit e)
        {
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Exhibitor, "Exhibitor");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            //Assert.IsNotNull(e.DisplayDate, "DisplayDate");
            Assert.IsNotNull(e.Description, "Description");
            //Assert.IsNotNull(e.DateStart, "DateStart");
            //Assert.IsNotNull(e.DateEnd, "DateEnd");
            //Assert.IsNotNull(e.CreatedAt, "CreatedAt");

            return true;
        }


        public bool ValidateEvent(Event e)
        {
            //Basic properties
            Assert.IsNotNull(e.Id, "Id");
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Color, "Color");
            Assert.IsTrue(e.Color.Length == 6, "Color is Hex");
            Assert.IsNotNull(e.SquareFootage, "SquareFootage");
            Assert.IsNotNull(e.DisplayDate, "DisplayDate");
            Assert.IsNotNull(e.Description, "Description");
            Assert.IsNotNull(e.DateStart, "DateStart");
            Assert.IsNotNull(e.DateEnd, "DateEnd");
            Assert.IsNotNull(e.CreatedAt, "CreatedAt");
            //Assert.IsNotNull(e.UpdatedAt, "UpdatedAt");  Is this optional?
            Assert.IsNotNull(e.Published, "Published");
            //Possibly unused properties
            //Assert.IsNotNull(e.Longitude, "Longitude");
            //Assert.IsNotNull(e.Lattitude, "Lattitude");
            Assert.IsNotNull(e.SocialMediaTitle, "SocialMediaTitle");
            Assert.IsNotNull(e.SocialMediaDescription, "SocialMediaDescription");
            Assert.IsNotNull(e.SocialMediaImagePath, "SocialMediaImagePath");
            //Obsolete properties to be removed?
            //Geolocation
            //SeoMetaDescription
            //PhotoFilePath
            //Curators
            //MediaContent
            //EventHeroVideoPath

            return true;
        }
    }
}
