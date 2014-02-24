using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

namespace OneMSQFT.UILogic.Analytics
{
    class TrackingContextData : Dictionary<string, object>
    {
        //TODO: Replace string keys with enums
        public string PageName 
        {
            get
            {
                return (this.ContainsKey("pageName") ? this["pageName"] as string: null);
            }
            set
            {
                this.Add("pageName", value);
            }
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
        public object VideoName
        {
            get
            {
                return (this.ContainsKey("videoName") ? this["videoName"] as string: null);
            }
            set
            {
                this.Add("videoName", value);
            }
        }
        public object EventName
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
        public object ExhibitName
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
        public object EventSqFt
        {
            get
            {
                return (this.ContainsKey("eventSqFt") ? this["eventSqFt"] as string: null);
            }
            set
            {
                this.Add("eventSqFt", value);
            }
        }
        public object PlatformName
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
    }
}
