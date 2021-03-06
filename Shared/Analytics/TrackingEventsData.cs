﻿using System;
using System.Collections.Generic;

namespace Ratio.Showcase.Shared.Analytics
{
    public class TrackingEventsData : HashSet<string>
    {        
        public override string ToString()
        {
            return String.Join(",", this);
        }

        public class Events
        {
            /// <summary>
            /// On load of every page
            /// </summary>            
            public const string PageView = "event1";
            /// <summary>
            /// On interaction of application elements which lead users to modals/popups within the site which don't fire a subsequent page view
            /// </summary>
            public const string ApplicationElementInteraction = "event4";
            /// <summary>
            /// On initiation of a video
            /// </summary>
            public const string VideoStart = "event8";
            /// <summary>
            /// On every interaction within the event section which triggers a subsequent event
            /// </summary>
            public const string EventInteraction = "event9";
            /// <summary>
            /// On every interaction within the exhibit section  which triggers a subsequent event
            /// </summary>
            public const string ExhibitInteraction = "event10";
            /// <summary>
            /// On load of the Semantic Zoom view
            /// </summary>
            public const string SemanticZooms = "event11";
            /// <summary>
            /// On interaction of an event in the App Bar
            /// </summary>
            public const string AppBarTaps = "event12";
            /// <summary>
            /// On every interaction
            /// </summary>
            public const string TotalInteraction = "event15";
            /// <summary>
            /// On every interaction
            /// </summary>
            public const string SocialShares = "event7";
        }
    }
}
