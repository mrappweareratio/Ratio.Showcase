using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using OneMSQFT.Common;

namespace OneMSQFT.UILogic.Utils
{
    public class ExhibitItemUtils
    {
        //TODO: Confirm Rsvp visibility / enable logic
        public static bool IsRsvpValid( Uri rsvpUrl)
        {
            return rsvpUrl != null && rsvpUrl.IsWellFormedOriginalString();
        }

        public static bool IsRsvpExpired(DateTime? expireDate)
        {
            var today = DateTime.Today;
            return today <= expireDate;
        }

        public static Uri GetRsvpFallbackUri()
        {
            return new Uri(Strings.RSVP_Fallback_Uri);
        }

        public static Uri GetExitLinkFallbackUri()
        {
            return new Uri(Strings.RSVP_Fallback_Uri);
        }
    }
}
