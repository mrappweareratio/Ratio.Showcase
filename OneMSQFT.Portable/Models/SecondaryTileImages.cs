using System;

namespace OneMSQFT.Common.Models
{
    public class SecondaryTileImages
    {
        public SecondaryTileImages()
        {
            Logo = new Uri("ms-appx:///Assets/Logo.scale-100.png");
            SmallLogo = new Uri("ms-appx:///Assets/SmallSquareLogo.scale-100.png");
            LargeLogo = new Uri("ms-appx:///Assets/LargeSquareLogo.scale-100.png");
            WideLogo = new Uri("ms-appx:///Assets/WideLogo.scale-100.png");
        }

        public Uri Logo { get; set; }
        public Uri SmallLogo { get; set; }
        public Uri LargeLogo { get; set; }
        public Uri WideLogo { get; set; }
    }
}