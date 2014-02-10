using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;
        private readonly IDataCacheService _cache;

        public DataService(IDataRepository repository, IDataCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            if (await _cache.ContainsKeyAsync("site_data"))
            {
                var siteData = await _cache.GetKeyAsync<SiteDataResult>("site_data");
                if (siteData == null)
                {
                    await _cache.InvalidateKeyAsync("site_data");
                }
                else
                {
                    return siteData.Events;
                }
            }
            var result = await _repository.GetSiteData();
            if (result != null)
                await _cache.StoreKeyAsync("site_data", result);
            return result.Events;
        }

        public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string id)
        {
            throw new NotImplementedException();
        }
    }
}
