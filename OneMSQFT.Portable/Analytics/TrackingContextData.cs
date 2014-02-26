using System.Collections.Generic;

namespace OneMSQFT.Common.Analytics
{
    public class TrackingContextData : Dictionary<string, object>
    {
        public class Variables
        {
            public const string PageName = "pageName";
            public const string AppElement = "appElement";
            public const string VideoName = "videoName";
            public const string EventName = "eventName";
            public const string ExhibitName = "exhibitName";
            public const string EventSqFt = "eventSqFt";
            public const string PlatformName = "platformName";
        }

        public string PageName
        {
            get
            {
                return (this.ContainsKey("pageName") ? this["pageName"] as string : null);
            }
            set
            {
                this.Add("pageName", value);
            }
        }

        public class PageNames
        {
            public const string EventLanding = "event landing";
            public const string Home = "home";
            public const string ExhibitLanding = "exhibit landing";
            public const string About = "about";
        }

        public object AppElement
        {
            get
            {
                return (this.ContainsKey("appElement") ? this["appElement"] as string : null);
            }
            set
            {
                this.Add("appElement", value);
            }
        }
        public string VideoName
        {
            get
            {
                return (this.ContainsKey("videoName") ? this["videoName"] as string : null);
            }
            set
            {
                this.Add("videoName", value);
            }
        }
        public string EventName
        {
            get
            {
                return this.ContainsKey("eventName") ? this["eventName"] as string : null;
            }
            set
            {
                this.Add("eventName", value);
            }
        }
        public string ExhibitName
        {
            get
            {
                return this.ContainsKey("exhibitName") ? this["exhibitName"] as string : null;
            }
            set
            {
                this.Add("exhibitName", value);
            }
        }
        public int? EventSqFt
        {
            get
            {
                return (this.ContainsKey("eventSqFt") ? (int?)this["eventSqFt"] : null);
            }
            set
            {
                this.Add("eventSqFt", value);
            }
        }
        public string PlatformName
        {
            get
            {
                return this.ContainsKey("platformName") ? this["platformName"] as string : null;
            }
            set
            {
                this.Add("platformName", value);
            }
        }

        public class PlatformNames
        {
            public const string Kiosk = "kiosk";
            public const string Store = "store";
        }
    }
}
