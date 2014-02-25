using System.Collections.Generic;

namespace OneMSQFT.Common.Services
{
    public interface IAnalyticsService
    {
        void Configure();
        void StartSession();
        void StopSession();
        void TrackEvents(IEnumerable<string> events, IDictionary<string, object> context = null);
        bool KioskModeEnabled { get; set; }
    }
}