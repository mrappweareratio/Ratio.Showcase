using System.Threading.Tasks;

namespace OneMSQFT.Common.Services
{
    public interface IDataCacheService
    {
        Task<bool> ContainsKeyAsync(string key);

        /// <summary>
        /// Returns cached key deserialized to a given type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        Task<T> GetKeyAsync<T>(string key) where T : class;

        Task InvalidateKeyAsync(string key);

        Task StoreKeyAsync<T>(string key, T data) where T : class;
    }
}