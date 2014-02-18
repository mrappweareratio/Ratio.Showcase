using System;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockAnalyticsService : IAnalyticsService
    {
        public Action ConfigureDelegate { get; set; }
        public Action StartSessionDelegate { get; set; }

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
    }
}