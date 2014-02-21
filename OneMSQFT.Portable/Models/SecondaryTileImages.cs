using System;

namespace OneMSQFT.Common.Models
{
    public class SecondaryTileImages
    {
        public SecondaryTileImages()
        {
            Logo = new Uri("ms-appx:///Assets/squareTile-sdk.png");
            SmallLogo = new Uri("ms-appx:///images/smallLogoSecondaryTile-sdk.png");
            WideLogo = new Uri("ms-appx:///images/smallLogoSecondaryTile-sdk.png");
        }

        public Uri Logo { get; set; }
        public Uri SmallLogo { get; set; }
        public Uri WideLogo { get; set; }
    }
}