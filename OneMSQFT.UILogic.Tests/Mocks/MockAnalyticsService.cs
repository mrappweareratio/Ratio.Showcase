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
    }
}