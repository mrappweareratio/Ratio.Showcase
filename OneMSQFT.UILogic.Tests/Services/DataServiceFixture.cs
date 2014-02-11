using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class DataServiceFixture
    {
        [TestMethod]
        async public Task DataService_GetEvents_Calls_Repository()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    called = true;
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var dataService = new DataService(mock, new MockDataCacheService() { ContainsDataDelegate = s => Task.FromResult(false) });
            await dataService.GetEvents();
            Assert.IsTrue(called);
        }

        [TestMethod]
        async public Task DataService_GetSiteData_Checks_Cache_False_Then_Calls_Repository()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    called = true;
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var cache = new MockDataCacheService
            {
                ContainsDataDelegate = s =>
                {
                    called = true;
                    return Task.FromResult(false);
                }
            };
            var dataService = new DataService(mock, cache);
            await dataService.GetEvents();
            Assert.IsTrue(called);
        }
        [TestMethod]
        async public Task DataService_GetSiteData_Checks_Cache_True_Then_Skips_Repository()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var cache = new MockDataCacheService
            {
                ContainsDataDelegate = s => Task.FromResult(true),
                GetDataDelegate = s =>
                {
                    {
                        called = true;
                        return Task.FromResult<object>(null);
                    }
                },
                InvalidateDataDelegate = s => Task.FromResult(0)
            };
            var dataService = new DataService(mock, cache);
            await dataService.GetEvents();
            Assert.IsTrue(called, "called cache");
        }
        [TestMethod]
        async public Task DataService_GetSiteData_Checks_Cache_True_Then_Null_Invalidates_Cache()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var cache = new MockDataCacheService
            {
                ContainsDataDelegate = s => Task.FromResult(true),
                GetDataDelegate = s =>
                {
                    {
                        return Task.FromResult<object>(null);
                    }
                },
                InvalidateDataDelegate = s =>
                {
                    called = true;
                    return Task.FromResult(0);
                }
            };
            var dataService = new DataService(mock, cache);
            await dataService.GetEvents();
            Assert.IsTrue(called, "callede InvalidateDataDelegate");
        }
        [TestMethod]
        async public Task DataService_GetSiteData_Checks_Cache_True_Then_Null_Invalidates_Cache_Calls_Repo()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    called = true;
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var cache = new MockDataCacheService
            {
                ContainsDataDelegate = s => Task.FromResult(true),
                GetDataDelegate = s =>
                {
                    {
                        return Task.FromResult<object>(null);
                    }
                },
                InvalidateDataDelegate = s =>
                {
                    return Task.FromResult(0);
                }
            };
            var dataService = new DataService(mock, cache);
            await dataService.GetEvents();
            Assert.IsTrue(called, "invalidated cache then called repo");
        }
        [TestMethod]
        async public Task DataService_GetSiteData_Calls_Repo_Then_StoreKeyAsync()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult());
                }
            };
            var cache = new MockDataCacheService
            {
                ContainsDataDelegate = s => Task.FromResult(true),
                GetDataDelegate = s =>
                {
                    {
                        return Task.FromResult<object>(null);
                    }
                },
                InvalidateDataDelegate = s =>
                {
                    return Task.FromResult(0);
                },
                StoreDataDelegate = (s, o) =>
                {
                    called = true;
                    return Task.FromResult(0);
                }
            };
            var dataService = new DataService(mock, cache);
            await dataService.GetEvents();
            Assert.IsTrue(called, "Call StoreDataAsync");
        }

        [TestMethod]
        async public Task DataService_GetExhibitDetailByExhibitId_Throws_ArgumentOutOfRangeException()
        {
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult() { Events = new List<Event> { } });
                }
            };
            var cache = new MockDataCacheService();
            var dataService = new DataService(mock, cache);
            try
            {
                await dataService.GetExhibitDetailByExhibitId("0");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentOutOfRangeException);
            }
        }

        [TestMethod]
        async public Task DataService_GetExhibitDetailByExhibitId_Finds_Exhibit()
        {
            const string exhibitId = "0";
            const string nextExhibitId = "1";
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult()
                    {
                        Events = new List<Event> {
                        {
                           new Event()
                           {
                               Exhibits = new List<Exhibit>{new Exhibit()
                               {
                                  Id= exhibitId
                               },
                               new Exhibit()
                               {
                                   Id = nextExhibitId
                               }}
                           }
                        } }
                    });
                }
            };
            var cache = new MockDataCacheService();
            var dataService = new DataService(mock, cache);            
            var exhibitDetail = await dataService.GetExhibitDetailByExhibitId(exhibitId);            
            Assert.IsNotNull(exhibitDetail, "exhibitDetail");
            Assert.AreEqual(exhibitDetail.Exhibit.Id, exhibitId);

        }

        [TestMethod]
        async public Task DataService_GetExhibitDetailByExhibitId_Finds_Next_Exhibit()
        {
            const string exhibitId = "0";
            const string nextExhibitId = "1";
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult()
                    {
                        Events = new List<Event> {
                        {
                           new Event()
                           {
                               Exhibits = new List<Exhibit>{new Exhibit()
                               {
                                  Id= exhibitId
                               },
                               new Exhibit()
                               {
                                   Id = nextExhibitId
                               }}
                           }
                        } }
                    });
                }
            };
            var cache = new MockDataCacheService();
            var dataService = new DataService(mock, cache);
            var exhibitDetail = await dataService.GetExhibitDetailByExhibitId(exhibitId);
            Assert.IsNotNull(exhibitDetail, "exhibitDetail");
            Assert.AreEqual(exhibitDetail.Exhibit.Id, exhibitId);
            Assert.AreEqual(exhibitDetail.NextExhibit.Id, nextExhibitId);
        }

        [TestMethod]
        async public Task DataService_GetExhibitDetailByExhibitId_Next_Exhibit_Loops()
        {
            const string exhibitId = "0";
            const string nextExhibitId = "1";
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult()
                    {
                        Events = new List<Event> {
                        {
                           new Event()
                           {
                               Exhibits = new List<Exhibit>{new Exhibit()
                               {
                                  Id= nextExhibitId
                               },
                               new Exhibit()
                               {
                                   Id = exhibitId
                               }}
                           }
                        } }
                    });
                }
            };
            var cache = new MockDataCacheService();
            var dataService = new DataService(mock, cache);
            var exhibitDetail = await dataService.GetExhibitDetailByExhibitId(exhibitId);
            Assert.IsNotNull(exhibitDetail, "exhibitDetail");
            Assert.AreEqual(exhibitDetail.Exhibit.Id, exhibitId);
            Assert.AreEqual(exhibitDetail.NextExhibit.Id, nextExhibitId);
        }
        [TestMethod]
        async public Task DataService_GetExhibitDetailByExhibitId_Next_Exhibit_Does_Not_Loop_Single()
        {
            const string exhibitId = "0";
            const string nextExhibitId = "1";
            var mock = new MockDataRepository
            {
                GetSiteDataDelegate = () =>
                {
                    return Task.FromResult(new SiteDataResult()
                    {
                        Events = new List<Event> {
                        {
                           new Event()
                           {
                               Exhibits = new List<Exhibit>{new Exhibit()
                               {
                                  Id= exhibitId
                               }}
                           }
                        } }
                    });
                }
            };
            var cache = new MockDataCacheService();
            var dataService = new DataService(mock, cache);
            var exhibitDetail = await dataService.GetExhibitDetailByExhibitId(exhibitId);
            Assert.IsNotNull(exhibitDetail, "exhibitDetail");
            Assert.AreEqual(exhibitDetail.Exhibit.Id, exhibitId, "Exhibit");
            Assert.IsNull(exhibitDetail.NextExhibit, "Next Exhibit Null");
        }
    }
}
