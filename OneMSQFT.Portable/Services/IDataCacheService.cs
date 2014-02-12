using System.Threading.Tasks;

namespace OneMSQFT.Common.Services
{
    public interface IDataCacheService
    {
        Task<bool> ContainsDataAsync(string key, bool ignoreExpirationPolicy);

        /// <summary>
        /// Returns cached key deserialized to a given type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<T> GetDataAsync<T>(string key) where T : class;

        Task InvalidateDataAsync(string key);

        Task StoreDataAsync<T>(string key, T data) where T : class;
    }
}