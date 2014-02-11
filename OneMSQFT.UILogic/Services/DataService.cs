using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Documents;
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
            if (await _cache.ContainsDataAsync("site_data"))
            {
                var siteData = await _cache.GetDataAsync<SiteDataResult>("site_data");
                if (siteData == null)
                {
                    var invalidate = Task.Run(async () =>
                    {
                        await _cache.InvalidateDataAsync("site_data").ConfigureAwait(false);
                    });
                }
                else
                {
                    return siteData.Events;
                }
            }
            var result = await _repository.GetSiteData();
            if (result != null)
            {
                var storeData = Task.Run(async () =>
                {
                    await _cache.StoreDataAsync("site_data", result).ConfigureAwait(false);
                });
                return result.Events;
            }

            return null;
        }

        async public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId)
        {
            var events = await GetEvents();
            var exhibits = events.SelectMany(x => x.Exhibits).ToList();
            if (exhibits.Count == 0)
                throw new ArgumentOutOfRangeException("ExhibitId");
            var exhibit = exhibits.FirstOrDefault(x => x.Id.Equals(exhibitId));
            if (exhibitId == null)
                throw new ArgumentOutOfRangeException("ExhibitId");
            var index = exhibits.IndexOf(exhibit);
            var nextIndex = index == exhibits.Count - 1 ? 0 : index + 1;
            var nextExhibit = exhibits.Count == 1 ? null : exhibits[nextIndex];
            var detail = new ExhibitDetail()
            {
                Exhibit = exhibit,
                NextExhibit = nextExhibit
            };
            return detail;
        }
    }
}
