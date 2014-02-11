using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.Utils
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
    }
}
