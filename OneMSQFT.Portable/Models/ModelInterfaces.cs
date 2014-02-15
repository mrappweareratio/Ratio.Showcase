using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public interface ISquareFootageItem
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int SquareFootage { get; set; }
    }

    public interface IEvent<out TExhibit> : ISocialMedia, ILocation, ISquareFootageItem, IDatedItem, IColor, IHasMediaContent, ITimeStampedItem where TExhibit : IExhibit
    {        
        IEnumerable<TExhibit> Exhibits { get; }
    }

    public interface ILocation
    {
        string Longitude { get; }
        string Lattitude { get; }
    }

    public interface IDatedItem
    {
        string DisplayDate { get; }
        DateTime DateStart { get; }
        DateTime DateEnd { get; }
    }

    public interface ITimeStampedItem
    {
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
    }

    public interface ICurator
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string WhiteLogoImage { get; }
        string LogoImageName { get; }
        /// <summary>
        /// CMS to enforce protocol prefix, i.e. http:// or https:// 
        /// </summary>
        string ExternalUrl { get; }
    }

    public interface IExhibit : ISquareFootageItem, ITaggable, IHasMediaContent, IColor, IDatedItem, ISocialMedia, ITimeStampedItem  
    {
        string Introduction { get; }
        /// <summary>
        /// Optional field if name is not in the title
        /// </summary>
        string Exhibitor { get; }
        string RsvpUrl { get; }
        IEnumerable<Link> Links { get; }
        string CuratorId { get; }
        string ThumbImage { get; }
        ICurator Curator { get; }
    }

    public interface IHasMediaContent
    {
        IEnumerable<MediaContentSource> MediaContent { get; }
    }

    public interface ITaggable
    {
        List<Tag> Tags { get; }
    }

    public interface IColor
    {
        /// <summary>
        /// Hex 6 Digits
        /// </summary>
        string Color { get; }
    }

    public interface ILink
    {
        string Id { get; }
        string ExhibitId { get; }
        string Title { get; }
        string Url { get; }
    }

    public interface ISocialMedia
    {
        /// <summary>
        /// 70 characters max, imposed by Twitter cards. This also applies to all subsequent social meta titles in this document.
        /// </summary>
        string SocialMediaTitle { get; }
        /// <summary>
        /// 200 characters max, imposed by Twitter cards. This also applies to all subsequent social meta descriptions in this document.
        /// </summary>
        string SocialMediaDescription { get; }
        string SocialMediaImagePath { get; }
    }    

    public interface IMediaContentSource
    {
        string Id { get; }
        string ParentId { get; }
        string Source { get; }
        ContentSourceType ContentSourceType { get; }
    }

    public interface IBaseResponse<TResult, TError>
        where TResult : class
        where TError : IError
    {
        string SystemResponse { get; set; }
        bool Success { get; set; }
        TError Error { get; set; }
        TResult Result { get; set; }
    }

    public interface IError
    {
        string Code { get; }
    }
}