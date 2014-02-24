using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.Analytics
{
    public class AnalyticsEventsContext : HashSet<string>
    {
        public override string ToString()
        {
            return String.Join(",", this);
        }
    }
}
