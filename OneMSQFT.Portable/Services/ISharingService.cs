using System;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.Services
{
    public interface ISharingService
    {
        bool TryGetVideoShareUri(MediaContentSource media, out Uri uri);
        bool TryGetEventShareUri(Event ev, out Uri uri);
        bool TryGetExhibitShareUri(Event ev, Exhibit exhibit, out Uri uri);
        /// <summary>
        /// Replaces a string with only word characters and replaces spaces with -
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetSlug(string name);

        //Task<IStorageFile> GetSharingThumbnail(Event ev);        
        //Task<IStorageFile> GetSharingThumbnail(Exhibit ex);
        //Task<IStorageFile> GetSharingThumbnail(MediaContentSource media);

        //RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(Event ev);
        //RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(Exhibit ex);
        //RandomAccessStreamReference GetSharingThumbnailRandomAccessStreamReference(MediaContentSource media);

        bool TryGetSharingThumbnailUri(Event ev, out Uri uri);
        bool TryGetSharingThumbnailUri(Exhibit ex, out Uri uri);
        bool TryGetSharingThumbnailUri(MediaContentSource media, out Uri uri);        
    }
}