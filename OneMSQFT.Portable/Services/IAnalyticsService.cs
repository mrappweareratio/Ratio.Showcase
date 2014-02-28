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
        void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, int evPos);
        void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, int evPos);
        void TrackExhibitInteractionInTimeline(string evName, string exName, int evPos, int exPos);
        void TrackShowMoreExhibitsInEvent(string evName);
        void TrackAppBarInteractionInTimeline(string evName, int? sqFootage);
        void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage);
        void TrackLinkInteractionInExhibitView(string exName, string exId, string linkTitle);
        void TrackVideoPlayInExhibitView(string exName, string exId, string vidName);
        void TrackPinEventInteraction(string evName);
        void TrackPinExhibitInteraction(string exName);
        void TrackUnPinEventInteraction(string evName);
        void TrackUnPinExhibitInteraction(string exName);
        void TrackNextExhibitInteraction(string exTo);

        bool KioskModeEnabled { get; set; }
        void TrackEventLanding(string evName);
        void TrackPageViewHome();
        void TrackExhibitLanding(string exName);
        void TrackPageViewAbout();

        //void TrackShareEventInteraction(string evName, int? sqFootage, int evPos);
        //void TrackVideoShareInEventView(string evName, int? sqFootage, int evPos, string vidName);
        //void TrackShareExhibitInteraction(string exName);        
        //void TrackVideoShareInExhibitView(string exName, string vidName);

    }
}