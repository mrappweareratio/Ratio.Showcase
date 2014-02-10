using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Tests.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockDataCacheService : IDataCacheService
    {
        public MockDataCacheService()
        {
        }

        public Func<String, Task<bool>> ContainsKeyDelegate { get; set; }
        public Func<String, Task<object>> GetKeyDelegate { get; set; }
        public Func<String, Task> InvalidateKeyDelegate { get; set; }
        public Func<String, object, Task> StoreKeyDelegate { get; set; }

        async public Task<bool> ContainsKeyAsync(string key)
        {
            if (ContainsKeyDelegate != null)
                return await ContainsKeyDelegate(key);
            return false;
        }

        async public Task<T> GetKeyAsync<T>(string key) where T : class
        {
            if (GetKeyDelegate != null)
                return await GetKeyDelegate(key) as T;
            return default(T);
        }

        async public Task InvalidateKeyAsync(string key)
        {
            if (InvalidateKeyDelegate != null)
                await InvalidateKeyDelegate(key);
        }

        async public Task StoreKeyAsync<T>(string key, T data) where T : class
        {
            if (StoreKeyDelegate != null)
                await StoreKeyDelegate(key, data);
        }
    }
}