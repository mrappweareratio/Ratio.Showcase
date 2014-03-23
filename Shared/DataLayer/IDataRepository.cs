using System.Threading;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.Shared.DataLayer
{
    public interface IDataRepository
    {
        Task<SiteData> GetSiteData(CancellationToken token);       
    }        
}
