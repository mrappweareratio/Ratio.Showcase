using System;
using System.Threading;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.DataLayer;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockDataRepository : IDataRepository
    {
        public Func<Task<SiteData>> GetSiteDataDelegate { get; set; }

        public Task<SiteData> GetSiteData(CancellationToken token)
        {
            return GetSiteDataDelegate();
        }        
    }
}