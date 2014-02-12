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
        private readonly Dictionary<string, object> _memDictionary = new Dictionary<string, object>();
        private readonly IDataRepository _repository;
        private readonly IDataCacheService _cache;

        public DataService(IDataRepository repository, IDataCacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            SiteDataResult result = null;

            const string key = "site_data";
            if (_memDictionary.ContainsKey(key))
            {
                result = _memDictionary[key] as SiteDataResult;
                if (result == null)
                {
                    _memDictionary.Remove(key);
                }
                else
                {
                    return result.Events;
                }
            }

            if (await _cache.ContainsDataAsync(key))
            {
                var siteData = await _cache.GetDataAsync<SiteDataResult>(key);
                if (siteData == null)
                {
                    var invalidate = Task.Run(async () =>
                    {
                        await _cache.InvalidateDataAsync(key).ConfigureAwait(false);
                    });
                }
                else
                {
                    _memDictionary.Add(key, siteData);
                    return siteData.Events;
                }
            }

            result = await _repository.GetSiteData();
            _memDictionary.Add(key, result);
            
            if (result != null)
            {
                var storeData = Task.Run(async () =>
                {
                    await _cache.StoreDataAsync(key, result).ConfigureAwait(false);
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
