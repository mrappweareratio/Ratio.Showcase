(function () {
    "use strict";
    
    var TRACKING_RSID = "YOUR_RSID_HERE";
    var TRACKING_SERVER = "YOUR_SERVER_HERE";

    function ConfigureAppMeasurement() {
        var measurement = ADMS.Measurement.ADMS_Measurement.instance;
        measurement.configureMeasurement(TRACKING_RSID, TRACKING_SERVER);

        //Set additional configuration variables here
        measurement.ssl = false;        
        measurement.debugLogging = true;
    }

    function ConfigureMediaMeasurement() {
        var mediaMeasurement = ADMS.Measurement.ADMS_MediaMeasurement.instance;

        //Configure ContextDataMapping(required)
        var dataMap = new Windows.Foundation.Collections.PropertySet();        
        dataMap["a.media.name"] = "eVar2,prop2";
        dataMap["a.media.segment"] = "eVar3";
        dataMap["a.contentType"] = "eVar1";
        dataMap["a.media.timePlayed"] = "event3";
        dataMap["a.media.view"] = "event1";
        dataMap["a.media.segmentView"] = "event2";
        dataMap["a.media.complete"] = "event7";
        mediaMeasurement.contextDataMapping = dataMap;

        //Configure optional settings
        mediaMeasurement.trackMilestones = "25,50,75";
        mediaMeasurement.segmentByMilestones = true;
    }

    //Examples of custom event and app state tracking

    function TrackCustomEvents(events) {
        var measurement = ADMS.Measurement.ADMS_Measurement.instance;
        var contextData = new Windows.Foundation.Collections.PropertySet();

        //Add items to your context data
        //contextData["contextKey"] = "value";

        measurement.trackEvents(events, contextData);        
    }

    function TrackCustomAppState(appState) {
        var measurement = ADMS.Measurement.ADMS_Measurement.instance;
        var contextData = new Windows.Foundation.Collections.PropertySet();

        //Add items to your context data
        //contextData["contextKey"] = "value";

    	measurement.trackAppState(appState, contextData);
    }

    WinJS.Namespace.define("TrackingHelper", {
        ConfigureAppMeasurement: ConfigureAppMeasurement,
        ConfigureMediaMeasurement: ConfigureMediaMeasurement,
        TrackCustomEvents: TrackCustomEvents,
        TrackCustomAppState: TrackCustomAppState,
    });

	// Auto-subscribe to lifecycle tracking
	function TrackAppStart() {
        var measurement = ADMS.Measurement.ADMS_Measurement.instance;
		
        measurement.startSession();
    }

    function TrackAppStop() {
        var measurement = ADMS.Measurement.ADMS_Measurement.instance;
		
        measurement.stopSession();
    }
	
    WinJS.Application.addEventListener("activated", function (args) {
        if (args.detail.kind === Windows.ApplicationModel.Activation.ActivationKind.launch) {
            TrackAppStart();
        }
    });

    WinJS.Application.addEventListener("checkpoint", function (args) {
        TrackAppStop();
    });
	
	ConfigureAppMeasurement();
	
})();