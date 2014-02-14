using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.Services
{
    public interface IConfigurationService
    {
        void SetStartupEvent(string id);
        void SetStartupExhibit(string id);
        void ClearStartupItem();
        StartupItemType StartupItemType { get; }
        string StartupItemId { get; }
    }
}