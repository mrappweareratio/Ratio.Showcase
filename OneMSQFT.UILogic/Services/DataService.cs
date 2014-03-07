using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly IInternetConnectionService _internetConnection;
        private bool isConnected = false;
        const string _key = "site_data";


        public DataService(IDataRepository repository, IDataCacheService cache, IInternetConnectionService connection)
        {
            _repository = repository;
            _cache = cache;
            _internetConnection = connection;
            Task.Run(() => Initialize());
        }

        private void Initialize()
        {
            if (_internetConnection == null) return;
            isConnected = _internetConnection.IsConnected;
            _internetConnection.InternetConnectionChanged += OnInternetConnectionChanged;
        }

        private void OnInternetConnectionChanged(object sender, IInternetConnection internetConnection)
        {
            isConnected = _internetConnection.IsConnected;
        }

        public async Task<IEnumerable<Event>> GetEvents(CancellationToken token)
        {
            SiteData result = null;

            if (_memDictionary.ContainsKey(_key))
            {
                result = _memDictionary[_key] as SiteData;
                if (result == null)
                {
                    _memDictionary.Remove(_key);
                }
                else
                {
                    return result.Events;
                }
            }

            if (await _cache.ContainsDataAsync(_key, !isConnected))
            {
                var siteData = await _cache.GetDataAsync<SiteData>(_key);
                if (siteData == null)
                {
                    var invalidate = Task.Run(async () =>
                    {
                        await _cache.InvalidateDataAsync(_key).ConfigureAwait(false);
                    }, token);
                }
                else
                {
                    _memDictionary.Add(_key, siteData);
                    return siteData.Events;
                }
            }

            result = await _repository.GetSiteData(token);
            _memDictionary.Add(_key, result);
            
            if (result != null)
            {
                var storeData = Task.Run(async () =>
                {
                    await _cache.StoreDataAsync(_key, result).ConfigureAwait(false);
                }, token);
                return result.Events;
            }

            return null;
        }

        async public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId, CancellationToken token)
        {
            if (exhibitId == null)
                throw new ArgumentOutOfRangeException("ExhibitId");
            var events = await GetEvents(new CancellationToken());
            var exhibits = events.SelectMany(x => x.Exhibits).ToList();
            if (exhibits.Count == 0)
                throw new ArgumentOutOfRangeException("ExhibitId");
            var exhibit = exhibits.FirstOrDefault(x => x.Id.Equals(exhibitId));

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

        async public Task<Event> GetEventById(string eventId, CancellationToken token)
        {
            if (eventId == null)
                throw new ArgumentOutOfRangeException("eventId");
            var events = await GetEvents(token);
            var ev = events.FirstOrDefault(x => x.Id.Equals(eventId));
            return ev;
        }

        public void ClearDataCacheInStorage()
        {
            if (null != _cache)
                _cache.InvalidateDataAsync(_key);
        }
    }
}
