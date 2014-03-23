using System;
using Ratio.Showcase.Shared;

namespace Ratio.Showcase.UILogic.Utils
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
