using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    //Tests real DataService using mock data repo and mock data
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
                    }},
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
    }
}
