#include "pch.h"
#include "TrackingHelper.h"

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation::Collections;

using namespace ADMS::Measurement;

static Platform::String ^TRACKING_RSID = "YOUR_RSID_HERE";
static Platform::String ^TRACKING_SERVER = "YOUR_SERVER_HERE";

typedef Map<Platform::String ^, Platform::Object ^>^ Dictionary;

TrackingHelper::TrackingHelper(void) { }
TrackingHelper::~TrackingHelper(void) { }

void TrackingHelper::ConfigureAppMeasurement() 
{	
	ADMS_Measurement ^measurement = ADMS_Measurement::Instance;
	measurement->ConfigureMeasurement(TRACKING_RSID, TRACKING_SERVER);

	//Set additional configuration variables here
	measurement->ssl = false;	
	measurement->debugLogging = true;
}

void TrackingHelper::ConfigureMediaMeasurement()
{
	ADMS_MediaMeasurement ^mediaMeasurement = ADMS_MediaMeasurement::Instance;

	//Configure ContextDataMapping(required)
	Dictionary dataMap = ref new Map<Platform::String ^, Platform::Object ^>();
	dataMap->Insert("a.media.name", "eVar2,prop2");
	dataMap->Insert("a.media.segment", "eVar3");
	dataMap->Insert("a.contentType", "eVar1");
	dataMap->Insert("a.media.timePlayed", "event3");
	dataMap->Insert("a.media.view", "event1");
	dataMap->Insert("a.media.segmentView", "event2");
	dataMap->Insert("a.media.complete", "event7");
	mediaMeasurement->contextDataMapping = dataMap;

	//Configure optional settings
	mediaMeasurement->trackMilestones = "25,50,75";
	mediaMeasurement->segmentByMilestones = true;
}


// Examples of custom event and app state tracking

void TrackingHelper::TrackCustomEvents(Platform::String ^events)
{	
	ADMS_Measurement ^measurement = ADMS_Measurement::Instance;
	Dictionary contextData = ref new Map<Platform::String ^, Platform::Object ^>();
	
	//Add items to context data
	//contextData->Insert("contextKey", "value");	

	measurement->TrackEvents(events, contextData);
}

void TrackingHelper::TrackCustomAppState(Platform::String ^appState) 
{
	ADMS_Measurement ^measurement = ADMS_Measurement::Instance;
	Dictionary contextData = ref new Map<Platform::String ^, Platform::Object ^>();
	
	//Add items to context data
	//contextData->Insert("contextKey", "value");

	measurement->TrackAppState(appState, contextData);
}