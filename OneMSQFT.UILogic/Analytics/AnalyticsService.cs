using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADMS.Measurement;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private static readonly ADMS_Measurement _measure = ADMS_Measurement.Instance;

        public void Configure()
        {
            TrackingHelper.ConfigureAppMeasurement();
        }

        public void StartSession()
        {
            _measure.StartSession();
        }
    }
}
