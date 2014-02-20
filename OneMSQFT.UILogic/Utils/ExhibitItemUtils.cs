using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
