using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using OneMSQFT.Common;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class DataCacheService : IDataCacheService
    {
        private static readonly StorageFolder _cacheFolder = ApplicationData.Current.TemporaryFolder;
        private static TimeSpan _expirationPolicy = new TimeSpan(0, 15, 0); // 15 minutes

        // We remember the most recently started task for each cache key, since only one I/O operation at a time may 
        // access that key. Cache read and write operations always wait for the prior task of the current cache key
        // to complete before they start. 
        private Dictionary<string, Task> _cacheKeyPreviousTask = new Dictionary<string, Task>();

        public DataCacheService()
        {
        }

        public async Task<bool> ContainsDataAsync(string key)
        {
            bool result = false;
            try
            {
                StorageFile file = await _cacheFolder.GetFileAsync(key);
            
                if (file != null)
                {
                    result = true;
                }            
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<T> GetDataAsync<T>(string key) where T : class
        {
            await CacheKeyPreviousTask(key);
            var result = GetDataAsyncInternal<T>(key);
            SetCacheKeyPreviousTask(key, result);
            return await result;
        }

        public async Task InvalidateDataAsync(string key)
        {
            var found = await ContainsDataAsync(key);
            if (found)
            {
                StorageFile file = await _cacheFolder.GetFileAsync(key);
                if (null != file)
                {
                    await file.DeleteAsync();
                }
            }
        }

        public async Task StoreDataAsync<T>(string key, T data) where T : class
        {
            await CacheKeyPreviousTask(key);
            var result = SaveDataAsyncInternal<T>(key, data);
            SetCacheKeyPreviousTask(key, result);
            await result;
        }

        //PRIVATE METHODS
        
        // Note: This method assumes that we are controlling the interleaving of async methods on a single thread.
        private async Task CacheKeyPreviousTask(string cacheKey)
        {
            if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
            {
                Task previousTask = null;
                // Wait for prior I/O task to complete. The while loop ensures that if multiple operations wait 
                // for the same task, the operations will be run one after another.
                while (_cacheKeyPreviousTask[cacheKey] != previousTask)
                {
                    previousTask = _cacheKeyPreviousTask[cacheKey];
                    try
                    {
                        await previousTask;
                    }
                    // Catch all exceptions; we can continue regardless of the exception status of the prior task.
                    // This exception will be handled by the original creator of the task.
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private async Task<T> GetDataAsyncInternal<T>(string cacheKey) where T : class
        {
            StorageFile file = await _cacheFolder.GetFileAsync(cacheKey);
            if (file == null) 
                throw new FileNotFoundException("File does not exist");

            // Check if the file has expired
            var fileBasicProperties = await file.GetBasicPropertiesAsync();
            var expirationDate = fileBasicProperties.DateModified.Add(_expirationPolicy).DateTime;
            bool fileIsValid = DateTime.Now.CompareTo(expirationDate) < 0;
            if (!fileIsValid)
                return null;

            string JsonText = await FileIO.ReadTextAsync(file);
            var dataFromJson = JsonHelper.DeserializeObject<T>(JsonText);
            return dataFromJson;
        }

        // Note: This method assumes that we are controlling the interleaving of async methods on a single thread.
        // We require fully synchronous execution between the return of "await CacheKeyPreviousTask(key)" and the invocation of this method
        // with argument "key".
        private void SetCacheKeyPreviousTask(string cacheKey, Task task)
        {
            if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
            {
                _cacheKeyPreviousTask[cacheKey] = task;
            }
            else
            {
                _cacheKeyPreviousTask.Add(cacheKey, task);
            }
        }

        private async Task SaveDataAsyncInternal<T>(string cacheKey, T data) where T : class
        {
            StorageFile file = await _cacheFolder.CreateFileAsync(cacheKey, CreationCollisionOption.ReplaceExisting);

            var jsonText = JsonHelper.SerializeObject(data);
            await FileIO.WriteTextAsync(file, jsonText);
        }
    }
}
