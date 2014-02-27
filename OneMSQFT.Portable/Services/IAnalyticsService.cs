using System.Collections.Generic;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;

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
        void TrackShowMoreExhibitsInEvent(string evName);
        void TrackAppBarInteractionInTimeline(string evName, int? sqFootage);
        void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage);
        void TrackLinkInteractionInExhibitView(string exName, string exPos, string linkTitle);
        void TrackVideoPlayInExhibitView(string exName, string exPos, string vidName);
        void TrackPinEventInteraction(string evName);
        void TrackPinExhibitInteraction(string exName);

        bool KioskModeEnabled { get; set; }
        void TrackEventLanding(string evName);
        void TrackPageViewHome();
        void TrackExhibitLanding(string exName);
        void TrackPageViewAbout();
    }
}