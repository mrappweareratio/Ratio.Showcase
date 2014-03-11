using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.VisualBasic;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic.Services
{
    public class SharingService : ISharingService
    {
        private readonly string _webSiteUrl;
        private readonly string _videoSiteUrl;

        public SharingService(IApiConfiguration configuration)
        {
            _webSiteUrl = configuration.WebSiteUrl;
            _videoSiteUrl = configuration.VideoSiteUrl;
        }

        public bool TryGetVideoShareUri(MediaContentSource media, out Uri uri)
        {
            uri = null;
            try
            {
                if (media == null)
                    throw new ArgumentNullException("MediaContentSource");
                if (String.IsNullOrEmpty(media.VideoId))
                    throw new ArgumentOutOfRangeException("VideoId");
                var uriString = String.Format("{0}/{1}", _videoSiteUrl, media.VideoId);
                return Uri.TryCreate(uriString, UriKind.Absolute, out uri);
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetEventShareUri(Event ev, out Uri uri)
        {
            uri = null;
            try
            {
                if (ev == null)
                    throw new ArgumentNullException("Event");
                if (String.IsNullOrWhiteSpace(ev.Name))
                    throw new ArgumentNullException("Event Name");
                var uriString = String.Format(Strings.ShareEventUrlFormatRootSlugId, _webSiteUrl, GetSlug(ev.Name), ev.Id);
                return Uri.TryCreate(uriString, UriKind.Absolute, out uri);
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetExhibitShareUri(Event ev, Exhibit exhibit, out Uri uri)
        {
            uri = null;
            try
            {
                if (exhibit == null)
                    throw new ArgumentNullException("Exhibit");
                var uriString = String.Format(Strings.ShareEventExhibitUrlFormatRootSlugId, _webSiteUrl, GetSlug(ev.Name), ev.Id, GetSlug(exhibit.Name), exhibit.Id);
                return Uri.TryCreate(uriString, UriKind.Absolute, out uri);
            }
            catch
            {
                return false;
            }
        }

        public string GetSlug(string name)
        {
            var str = name.ToLower().Trim();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");
            return str;
        }

        async public Task<IStorageFile> GetSharingThumbnail(Event ev)
        {
            var reference = GetSharingThumbnailRandomAccessStreamReference(ev);
            Uri uri;
            TryGetSharingThumbnailUri(ev, out uri);
            var sf = await StorageFile.CreateStreamedFileFromUriAsync(String.Format("{0}.jpg", GetSlug(ev.Name)), uri, reference);
            return sf;
        }

        public bool TryGetSharingThumbnailUri(Event ev, out Uri uri)
        {
            uri = null;
            if (ev == null)
                return false;
            if (ev.MediaContent == null || !ev.MediaContent.Any())
                return false;            
            //return Uri.TryCreate(ev.MediaContent.FirstOrDefault().Img, UriKind.Absolute, out uri);
            return Uri.TryCreate("http://google.com", UriKind.Absolute, out uri);
        }

        public Task<IStorageFile> GetSharingThumbnail(Exhibit ex)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageFile> GetSharingThumbnail(MediaContentSource media)
        {
            throw new NotImplementedException();
        }

        public RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(Event ev)
        {
            Uri uri;
            TryGetSharingThumbnailUri(ev, out uri);
            return RandomAccessStreamReference.CreateFromUri(uri);
        }

        public RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(Exhibit ex)
        {
            throw new NotImplementedException();
        }

        public RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(MediaContentSource media)
        {
            throw new NotImplementedException();
        }
    }
}
