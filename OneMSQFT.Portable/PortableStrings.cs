using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneMSQFT.Common
{
    public class PortableStrings
    {
        private static Strings _strings;
        public static Strings Strings
        {
            get { return _strings ?? (_strings = new Strings()); }
        }
    }
}
