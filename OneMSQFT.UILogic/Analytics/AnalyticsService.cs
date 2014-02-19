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
    }
}
