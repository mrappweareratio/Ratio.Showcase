using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.Common.DataLayer
{
    public class LocalStorageManager
    {
        private readonly ILocalStorageProvider _localStorage;

        public LocalStorageManager(ILocalStorageProvider localStorage)
        {
            _localStorage = localStorage;
        }

        async public Task<string> LoadFile(string fileName)
        {
            if (null != _localStorage)
            {
                return await _localStorage.LoadFile(fileName);
            }
            return null;
        }

        public void SaveFile(string fileName, byte[] data)
        {
            if (null != _localStorage)
            {
                _localStorage.SaveFile(fileName, data);
            }
        }

        public string GetLocalFolder()
        {
            return null != _localStorage ? _localStorage.GetLocalFolder() : string.Empty;
        }
    }
}
