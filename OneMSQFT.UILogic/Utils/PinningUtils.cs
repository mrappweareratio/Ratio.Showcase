using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.Utils
{
    public class PinningUtils
    {
        public static string GetSecondaryTileIdByEventId(string eventId)
        {
            return String.Format("Event,{0}", eventId);
        }

        public static string GetSecondaryTileIdByExhibitId(string exhibitId)
        {
            return String.Format("Exhibit,{0}", exhibitId);
        }
    }
}
