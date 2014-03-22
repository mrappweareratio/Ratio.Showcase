namespace Ratio.Showcase.Shared
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
