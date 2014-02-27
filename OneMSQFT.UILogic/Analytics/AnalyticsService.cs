using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.Measurement;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private static readonly ADMS_Measurement _measure = ADMS_Measurement.Instance;

        public bool KioskModeEnabled { get; set; }

        private const bool Disabled = false;

        public void Configure()
        {
            if (Disabled) return;
            TrackingHelper.ConfigureAppMeasurement();
        }

        public void StartSession()
        {
            if (Disabled) return;
            _measure.StartSession();
        }

        public void StopSession()
        {
            if (Disabled) return;
            _measure.StopSession();
        }

        public void TrackEvents(TrackingEventsData eventsData, TrackingContextData context = null)
        {
            if (Disabled)
                return;
            context = context ?? new TrackingContextData();
            context.PlatformName = KioskModeEnabled
                ? TrackingContextData.PlatformNames.Kiosk
                : TrackingContextData.PlatformNames.Store;
            _measure.TrackEvents(eventsData.ToString(), context);
        }

        public void TrackEventLanding(string evName)
        {
            if (Disabled)
                return;
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                PageName = TrackingContextData.PageNames.EventLanding
            };
            TrackEvents(events, context);
        }

        public void TrackPageViewHome()
        {
            if (Disabled)
                return;
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {                
                PageName = TrackingContextData.PageNames.Home
            };
            TrackEvents(events, context);
        }

        public void TrackPageViewAbout()
        {
            if (Disabled)
                return;
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                PageName = TrackingContextData.PageNames.About
            };
            TrackEvents(events, context);
        }

        public void TrackExhibitLanding(string exName)
        {
            if (Disabled)
                return;
            var events = new TrackingEventsData { TrackingEventsData.Events.PageView, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                PageName = TrackingContextData.PageNames.ExhibitLanding
            };            
            TrackEvents(events, context);
        }

        public void TrackTimelineSemanticZoom()
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.SemanticZooms, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                AppElement = TrackingContextData.AppElements.GenerateEventSemanticZoomData()
            };

            this.TrackEvents(evData, context);
        }

        public void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, string id )
        {
            if (Disabled)
                return;
            
            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                AppElement = TrackingContextData.AppElements.GenerateEventSemanticZoomData(Convert.ToInt32(id))
            };

            this.TrackEvents(evData, context);
        }

        public void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, string id)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.VideoStart, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                VideoName = vidName,
                AppElement = TrackingContextData.AppElements.GeneratePlayVideoInEventData()
            };

            this.TrackEvents(evData, context);
        }

        public void TrackExhibitInteractionInTimeline(string evName, string exName, string evPos, string exPos)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                ExhibitName = exName,
                AppElement = TrackingContextData.AppElements.GenerateClickExhibitInEventData(Convert.ToInt32(evPos), Convert.ToInt32(exPos))
            };

            this.TrackEvents(evData, context);            
        }

        public void TrackShowMoreExhibitsInEvent(string evName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                AppElement = TrackingContextData.AppElements.GenerateShowMoreExhibitsData()
            };

            this.TrackEvents(evData, context);                  
        }

        public void TrackAppBarInteractionInTimeline(string evName, int? sqFootage)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.AppBarTaps, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                AppElement = TrackingContextData.AppElements.GenerateClickEventInTimelineAppBarData()
            };

            this.TrackEvents(evData, context);              
        }

        public void TrackAppBarInteractionInExhibitView(string evName, int? sqFootage)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.AppBarTaps, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                AppElement = TrackingContextData.AppElements.GenerateClickEventInExhibitAppBarData()
            };

            this.TrackEvents(evData, context);
        }
    }
}
