using System;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockDataCacheService : IDataCacheService
    {
        public MockDataCacheService()
        {
        }

        public Func<String, Task<bool>> ContainsDataDelegate { get; set; }
        public Func<String, Task<object>> GetDataDelegate { get; set; }
        public Func<String, Task> InvalidateDataDelegate { get; set; }
        public Func<String, object, Task> StoreDataDelegate { get; set; }

        async public Task<bool> ContainsDataAsync(string key, bool ignoreExpirationPolicy)
        {
            if (ContainsDataDelegate != null)
                return await ContainsDataDelegate(key);
            return false;
        }

        async public Task<T> GetDataAsync<T>(string key) where T : class
        {
            if (GetDataDelegate != null)
                return await GetDataDelegate(key) as T;
            return default(T);
        }

        async public Task InvalidateDataAsync(string key)
        {
            if (InvalidateDataDelegate != null)
                await InvalidateDataDelegate(key);
        }

        async public Task StoreDataAsync<T>(string key, T data) where T : class
        {
            if (StoreDataDelegate != null)
                await StoreDataDelegate(key, data);
        }
    }
}