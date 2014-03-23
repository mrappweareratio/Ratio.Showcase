using System.Collections.Generic;
using ADMS.Measurement;

namespace Ratio.Showcase.UILogic.Analytics
{
	public static class TrackingHelper
	{

#if RELEASE
		private static string TRACKING_RSID = @"mswindows1msqftapp";
#else
        private static string TRACKING_RSID = @"mswindows1msqftappdev";
#endif
        private static string TRACKING_SERVER = @"microsoftwindows.112.2o7.net";		

		public static void ConfigureAppMeasurement()
		{
			var measurement = ADMS_Measurement.Instance;
			measurement.ConfigureMeasurement(TRACKING_RSID, TRACKING_SERVER);

			//Set additional configuration variables here
			measurement.ssl = false;
			measurement.debugLogging = true;
		}

		public static void ConfigureMediaMeasurement()
		{
			ADMS_MediaMeasurement mediaMeasurement = ADMS_MediaMeasurement.Instance;

			//Configure ContextDataMapping(required)
			Dictionary<string, object> dataMap = new Dictionary<string, object>();
			dataMap.Add("a.media.name", "eVar2,prop2");
			dataMap.Add("a.media.segment", "eVar3");
			dataMap.Add("a.contentType", "eVar1");
			dataMap.Add("a.media.timePlayed", "event3");
			dataMap.Add("a.media.view", "event1");
			dataMap.Add("a.media.segmentView", "event2");
			dataMap.Add("a.media.complete", "event7");
			mediaMeasurement.contextDataMapping = dataMap;

			//Configure optional settings
			mediaMeasurement.trackMilestones = "25,50,75";
			mediaMeasurement.segmentByMilestones = true;
		}


		//Examples of custom event and app state tracking

		public static void TrackCustomEvents(string events)
		{
			var measurement = ADMS_Measurement.Instance;
			Dictionary<string, object> contextData = new Dictionary<string, object>();

			//Add items to your context data here
			//contextData.Add("contextKey", "value");

			measurement.TrackEvents(events, contextData);
		}

		public static void TrackCustomAppState(string appState)
		{
			var measurement = ADMS_Measurement.Instance;
			Dictionary<string, object> contextData = new Dictionary<string, object>();

			//Add items to your context data here
			//contextData.Add("contextKey", "value");

			measurement.TrackAppState(appState, contextData);
		}
	}
}
