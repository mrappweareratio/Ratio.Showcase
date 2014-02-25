using System.Collections.Generic;
using OneMSQFT.Common.Analytics;

namespace OneMSQFT.Common.Services
{
    public interface IAnalyticsService
    {
        void Configure();
        void StartSession();
        void StopSession();
        void TrackEvents(TrackingEventsData eventsData, TrackingContextData context = null);        
        bool KioskModeEnabled { get; set; }
    }
}