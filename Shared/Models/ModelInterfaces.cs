using System;
using System.Collections.Generic;

namespace Ratio.Showcase.Shared.Models
{
    public interface ISquareFootageItem
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int SquareFootage { get; set; }
    }

    public interface IEvent<out TExhibit> : ISocialMedia, ILocation, ISquareFootageItem, IDatedItem, IColor, IHasMediaContent, ITimeStampedItem where TExhibit : IExhibit<ICurator>
    {        
        IEnumerable<TExhibit> Exhibits { get; }
        string ThemeId { get; }
    }

    public interface ILocation
    {
        string Location { get; }
        string Longitude { get; }
        string Lattitude { get; }
    }

    public interface IDatedItem
    {
        string DisplayDate { get; }
        DateTime? DateStart { get; }
        DateTime? DateEnd { get; }
    }

    public interface ITimeStampedItem
    {
        DateTime? CreatedAt { get; }
        DateTime? UpdatedAt { get; }
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

    public interface IExhibit<out TCurator> : ISquareFootageItem, ITaggable, IColor, IHasMediaContent, IDatedItem, ISocialMedia, ITimeStampedItem 
        where TCurator : ICurator
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
        TCurator Curator { get; }
        string EventId { get; }
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
        string Img { get; }
        string VideoId { get; }
        string VideoUrlHd { get; }
        string VideoUrlSd { get; }
        string VideoUrlMobile { get; }
        ContentSourceType ContentSourceType { get; }
        int SortOrder { get; }
    }

    public interface IBaseResponse<TResult, TResultData, TError>
        where TError : IError
        where TResult : IBaseResult<TResultData>
        where TResultData : class
    {
        string SystemResponse { get; set; }                
        bool IsSuccess { get; set; }
        TError Error { get; set; }
        TResult Result { get; set; }
    }

    public interface IBaseResult<TResult> where TResult : class
    {
        TResult Data { get; set; }
    }

    public interface IError
    {
        string Code { get; }
    }
}