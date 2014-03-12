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

        private const bool Disabled = true;

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

        public void TrackTimelineSemanticZoomEventInteraction(string evName, int? sqFootage, int evPos)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                AppElement = TrackingContextData.AppElements.GenerateEventSemanticZoomData(evPos)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackVideoPlayInEventView(string evName, string vidName, int? sqFootage, int? evPos)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.VideoStart, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                VideoName = vidName,
                AppElement = TrackingContextData.AppElements.GeneratePlayVideoInEventData(evPos)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackExhibitInteractionInTimeline(string evName, string exName, int evPos, int exPos)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                ExhibitName = exName,
                AppElement = TrackingContextData.AppElements.GenerateClickExhibitInEventData(evPos, exPos)
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

        public void TrackAppBarEventInteraction(string evName, int? sqFootage, int? evPos)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.AppBarTaps, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                AppElement = TrackingContextData.AppElements.GenerateClickEventInAppBarData(evPos)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackLinkInteractionInExhibitView(string exName, string exId, string linkTitle)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                AppElement = TrackingContextData.AppElements.GenerateClickLinkInExhibitData(linkTitle, exId)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackVideoPlayInExhibitView(string exName, string exId, string vidName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.VideoStart, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                VideoName = vidName,
                AppElement = TrackingContextData.AppElements.GeneratePlayVideoInExhibitData(exName)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackPinEventInteraction(string evName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                AppElement = TrackingContextData.AppElements.GeneratePinEventData(evName)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackPinExhibitInteraction(string exName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                AppElement = TrackingContextData.AppElements.GeneratePinExhibitData(exName)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackUnPinEventInteraction(string evName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                AppElement = TrackingContextData.AppElements.GenerateUnPinEventData(evName)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackUnPinExhibitInteraction(string exName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                AppElement = TrackingContextData.AppElements.GenerateUnPinExhibitData(exName)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackNextExhibitInteraction(string exTo)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exTo,
                AppElement = TrackingContextData.AppElements.GenerateClickNextExhibitData(exTo)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackShareEventInteraction(string evName, int? sqFootage, int? evPos, string shareUrl, string appName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.SocialShares, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                SocialShareType = appName,
                AppElement = TrackingContextData.AppElements.GenerateShareEventElement(evPos, appName, shareUrl)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackShareExhibitInteraction(string exName, string shareUrl, string appName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.ExhibitInteraction, TrackingEventsData.Events.SocialShares, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                SocialShareType = appName,
                AppElement = TrackingContextData.AppElements.GenerateShareExhibitElement(appName, shareUrl)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackVideoShareInEventView(string evName, int? sqFootage, int? evPos, string vidName, string shareUrl,
            string appName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.SocialShares, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                EventName = evName,
                EventSqFt = sqFootage,
                VideoName = vidName,
                SocialShareType = appName,
                AppElement = TrackingContextData.AppElements.GenerateShareEventVideoElement(evPos, vidName, appName, shareUrl)
            };

            this.TrackEvents(evData, context);
        }

        public void TrackVideoShareInExhibitView(string exName, string vidName, string shareUrl, string appName)
        {
            if (Disabled)
                return;

            var evData = new TrackingEventsData { TrackingEventsData.Events.ApplicationElementInteraction, TrackingEventsData.Events.EventInteraction, TrackingEventsData.Events.SocialShares, TrackingEventsData.Events.TotalInteraction };
            var context = new TrackingContextData
            {
                ExhibitName = exName,
                VideoName = vidName,
                SocialShareType = appName,
                AppElement = TrackingContextData.AppElements.GenerateShareExhibitVideoElement(exName, vidName, appName, shareUrl)
            };

            this.TrackEvents(evData, context);
        }
    }
}
