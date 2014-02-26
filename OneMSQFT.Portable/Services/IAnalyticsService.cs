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
        void TrackTimelineSemanticZoom();
        void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, string id);
        void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, string id);
        void TrackExhibitInteractionInTimeline(string evName, string exName, string evPos, string exPos);
        void TrackAppBarInteractionInTimeline(string evName, int? sqFootage);
        void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage);
        bool KioskModeEnabled { get; set; }
    }
}