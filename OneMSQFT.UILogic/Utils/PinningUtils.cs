using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

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

        public static PinningContext ParseArguments(string arguments)
        {
            if (String.IsNullOrEmpty(arguments))
            {
                return new PinningContext();
            }
            var args = arguments.Split(new string[] { @"," }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 2)
                return new PinningContext();
            if (args[0].Equals("Event"))
                return new PinningContext(StartupItemType.Event, args[1]);
            if (args[0].Equals("Exhibit"))
                return new PinningContext(StartupItemType.Event, args[1]);
            return new PinningContext();
        }
    }

    public class PinningContext
    {
        public PinningContext(StartupItemType startupItemType = StartupItemType.None)
        {
            StartupItemType = startupItemType;
        }

        public PinningContext(StartupItemType startupItemType, string startupItemId)
        {
            StartupItemType = startupItemType;
            StartupItemId = startupItemId;
        }

        public StartupItemType StartupItemType { get; private set; }
        public string StartupItemId { get; private set; }
    }
}
