namespace Ratio.Showcase.Shared.Models
{
    public class SecondaryTileArgs
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// Deep Link
        /// </summary>
        public string ArgumentsName { get; set; }
        /// <summary>
        /// ARGB Color String ex. #FF123456
        /// Use Windows.UI.Color.ToString()
        /// </summary>
        public string BackgroundColor { get; set; }
    }
}