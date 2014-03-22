using Ratio.Showcase.Shared.Analytics;

namespace Ratio.Showcase.Shared.Services
{
    public interface IAnalyticsService
    {
        void Configure();
        void StartSession();
        void StopSession();
        void TrackEvents(TrackingEventsData eventsData, TrackingContextData context = null);
        void TrackTimelineSemanticZoom();
        void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, int evPos);
        void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, int? evPos);
        void TrackExhibitInteractionInTimeline(string evName, string exName, int evPos, int exPos);
        void TrackShowMoreExhibitsInEvent(string evName);
        void TrackAppBarEventInteraction(string evName, int? sqFootage, int? evPos);        
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

        void TrackShareEventInteraction(string evName, int? sqFootage, int? evPos, string shareUrl, string appName);
        void TrackVideoShareInEventView(string evName, int? sqFootage, int? evPos, string vidName, string shareUrl, string appName);
        void TrackShareExhibitInteraction(string exName, string shareUrl, string appName);
        void TrackVideoShareInExhibitView(string exName, string vidName, string shareUrl, string appName);

    }
}