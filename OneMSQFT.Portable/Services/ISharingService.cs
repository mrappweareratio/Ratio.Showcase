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
    }
}