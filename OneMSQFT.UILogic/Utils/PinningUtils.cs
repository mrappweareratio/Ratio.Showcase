using System;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.Utils
{
    public class PinningUtils
    {
        public static string GetSecondaryTileIdByEventId(string eventId)
        {
            return String.Format("Event_{0}", eventId);
        }

        public static string GetSecondaryTileIdByExhibitId(string exhibitId)
        {
            return String.Format("Exhibit_{0}", exhibitId);
        }

        public static PinningContext ParseArguments(string arguments)
        {
            if (String.IsNullOrEmpty(arguments))
            {
                return new PinningContext();
            }
            var args = arguments.Split(new string[] { @"_" }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != 2)
                return new PinningContext();
            if (args[0].Equals("Event"))
                return new PinningContext(StartupItemType.Event, args[1]);
            if (args[0].Equals("Exhibit"))
                return new PinningContext(StartupItemType.Exhibit, args[1]);
            return new PinningContext();
        }
    }
}
