#pragma once

class TrackingHelper
{

public:
	TrackingHelper(void);
	~TrackingHelper(void);

	static void ConfigureAppMeasurement();
	static void ConfigureMediaMeasurement();
	static void TrackCustomEvents(Platform::String ^events);
	static void TrackCustomAppState(Platform::String ^appState);

private:
	
};

