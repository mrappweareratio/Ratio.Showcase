using System;
using System.Collections.Generic;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockAnalyticsService : IAnalyticsService
    {
        public Action ConfigureDelegate { get; set; }
        public Action StartSessionDelegate { get; set; }
        public Action StopSessionDelegate { get; set; }
        public Action<IEnumerable<string>, IDictionary<string, object>> TrackEventsDelegate { get; set; }
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

        public void TrackEvents(IEnumerable<string> events, IDictionary<string, object> context = null)
        {
            if (TrackEventsDelegate != null)
                TrackEventsDelegate(events, context);
        }
    }
}