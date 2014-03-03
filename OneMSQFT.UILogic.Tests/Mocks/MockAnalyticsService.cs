using System;
using System.Collections.Generic;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockAnalyticsService : IAnalyticsService
    {
        public Action ConfigureDelegate { get; set; }
        public Action StartSessionDelegate { get; set; }
        public Action StopSessionDelegate { get; set; }
        public Action<TrackingEventsData, TrackingContextData> TrackEventsDelegate { get; set; }

        public bool KioskModeEnabled { get; set; }

        public void Configure()
        {
            if (ConfigureDelegate != null)
                ConfigureDelegate();
        }

        public void StartSession()
        {
            if (StartSessionDelegate != null)
                StartSessionDelegate();
        }

        public void StopSession()
        {
            if (StopSessionDelegate != null)
                StopSessionDelegate();
        }

        public void TrackEvents(TrackingEventsData eventsData, TrackingContextData context = null)
        {
            if (TrackEventsDelegate != null)
                TrackEventsDelegate(eventsData, context);
        }

        public void TrackEventLanding(string evName)
        {
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView };
            var context = new TrackingContextData
            {
                EventName = evName,
                PageName = TrackingContextData.PageNames.EventLanding
            };
            TrackEvents(events, context);
        }

        public void TrackPageViewHome()
        {
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView };
            var context = new TrackingContextData
            {
                PageName = TrackingContextData.PageNames.Home
            };
            TrackEvents(events, context);
        }

        public void TrackPageViewAbout()
        {          
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView };
            var context = new TrackingContextData
            {
                PageName = TrackingContextData.PageNames.About
            };
            TrackEvents(events, context);
        }

        public void TrackExhibitLanding(string exName)
        {
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                PageName = TrackingContextData.PageNames.ExhibitLanding
            };
            TrackEvents(events, context);
        }

        public void TrackTimelineSemanticZoom()
        {
        }

        public void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, int evPos)
        {
        }

        public void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, int? evPos)
        {
        }

        public void TrackExhibitInteractionInTimeline(string evName, string exName, int evPos, int exPos)
        {
        }

        public void TrackShowMoreExhibitsInEvent(string evName)
        {
        }

        public void TrackAppBarInteractionInTimeline(string evName, int? sqFootage)
        {
        }

        public void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage)
        {
        }

        public void TrackLinkInteractionInExhibitView(string exName, string exId, string linkTitle)
        {
        }

        public void TrackVideoPlayInExhibitView(string exName, string exId, string vidName)
        {
        }

        public void TrackPinEventInteraction(string evName)
        {
        }

        public void TrackPinExhibitInteraction(string exName)
        {
        }

        public void TrackUnPinEventInteraction(string evName)
        {
        }

        public void TrackUnPinExhibitInteraction(string exName)
        {
        }

        public void TrackNextExhibitInteraction(string exTo)
        {
        }

        public void TrackShareEventInteraction(string evName, int? sqFootage, int? evPos, string shareUrl, string appName)
        {            
        }

        public void TrackShareExhibitInteraction(string exName, string shareUrl, string appName)
        {            
        }

        public void TrackVideoShareInEventView(string evName, int? sqFootage, int? evPos, string vidName, string shareUrl,
            string appName)
        {
        }

        public void TrackVideoShareInExhibitView(string exName, string vidName, string shareUrl, string appName)
        {
        }
    }
}