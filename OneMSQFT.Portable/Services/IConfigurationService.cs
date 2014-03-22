using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.Shared.Services
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