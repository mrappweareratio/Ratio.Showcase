using System;
using System.Text.RegularExpressions;

namespace Ratio.Showcase.UILogic.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// Replaces spaces instead of commas
        /// </summary>
        public static String ToSquareFeet(int squareFeet)
        {
            return String.Format("{0:# ### ###}", squareFeet);
        }

        public static string BuildDescription(string description)
        {
            if (description == null)
                return String.Empty;
            var retVal = Regex.Replace(description, @"<br*>", Environment.NewLine);
            return retVal;
        }
    }
}
