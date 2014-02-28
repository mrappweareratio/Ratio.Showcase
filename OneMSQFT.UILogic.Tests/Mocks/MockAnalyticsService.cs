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
            throw new NotImplementedException();
        }

        public void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, string id)
        {
            throw new NotImplementedException();
        }

        public void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, string id)
        {
            throw new NotImplementedException();
        }

        public void TrackExhibitInteractionInTimeline(string evName, string exName, string evPos, string exPos)
        {
            throw new NotImplementedException();
        }

        public void TrackShowMoreExhibitsInEvent(string evName)
        {
            throw new NotImplementedException();
        }

        public void TrackAppBarInteractionInTimeline(string evName, int? sqFootage)
        {
            throw new NotImplementedException();
        }

        public void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage)
        {
            throw new NotImplementedException();
        }

        public void TrackLinkInteractionInExhibitView(string exName, string exPos, string linkTitle)
        {
            throw new NotImplementedException();
        }

        public void TrackVideoPlayInExhibitView(string exName, string exPos, string vidName)
        {
            throw new NotImplementedException();
        }

        public void TrackPinEventInteraction(string evName)
        {
            throw new NotImplementedException();
        }

        public void TrackPinExhibitInteraction(string exName)
        {
            throw new NotImplementedException();
        }

        public void TrackUnPinEventInteraction(string evName)
        {
            throw new NotImplementedException();
        }

        public void TrackUnPinExhibitInteraction(string exName)
        {
            throw new NotImplementedException();
        }

        public void TrackNextExhibitInteraction(string exTo)
        {
            throw new NotImplementedException();
        }
    }
}