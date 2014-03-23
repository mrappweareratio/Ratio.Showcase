using System.Threading.Tasks;

namespace Ratio.Showcase.Shared.DataLayer
{
    public interface ILocalStorageProvider
    {
        Task<string> LoadFile(string fileName);
        Task SaveFile(string fileName, byte[] data);
        string GetLocalFolder();
    }
}
