using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.Common.DataLayer
{
    public interface ILocalStorageProvider
    {
        Task<string> LoadFile(string fileName);
        Task SaveFile(string fileName, byte[] data);
        string GetLocalFolder();
    }
}
