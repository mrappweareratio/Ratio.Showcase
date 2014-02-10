using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class DataCacheService : IDataCacheService
    {
        public DataCacheService()
        {
        }

        public Task<bool> ContainsKeyAsync(string key)
        {
            return Task.FromResult<bool>(false);
        }

        public Task<T> GetKeyAsync<T>(string key) where T : class
        {
            return Task.FromResult<T>(null);
        }

        public Task InvalidateKeyAsync(string key)
        {
            return Task.FromResult(0);
        }

        public Task StoreKeyAsync<T>(string key, T data) where T : class
        {
            return Task.FromResult(0);
        }
    }
}
