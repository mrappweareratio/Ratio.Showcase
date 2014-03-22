using System;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockSharingService : ISharingService
    {
        public bool TryGetVideoShareUri(MediaContentSource media, out Uri uri)
        {
            uri = null;
            return false;
        }

        public bool TryGetEventShareUri(Event ev, out Uri uri)
        {
            uri = null;
            return false;
        }

        public bool TryGetExhibitShareUri(Event ev, Exhibit exhibit, out Uri uri)
        {
            uri = null;
            return false;
        }

        public string GetSlug(string name)
        {
            return String.Empty;
        }

        public bool TryGetSharingThumbnailUri(Event ev, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSharingThumbnailUri(MediaContentSource media, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSharingThumbnailUri(Exhibit ex, out Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}
