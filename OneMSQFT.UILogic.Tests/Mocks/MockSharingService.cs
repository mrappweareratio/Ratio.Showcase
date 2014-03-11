using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
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
    }
}
