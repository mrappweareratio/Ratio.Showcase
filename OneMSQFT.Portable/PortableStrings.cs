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
            get
            {
                if (_strings == null)
                {
                    _strings = new Strings();
                }
                return _strings;
            }
        }
    }
}
