using System;
using System.Collections.Generic;

namespace OneMSQFT.Common.Models
{
    public interface ISquareFootageItem
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        int SquareFootage { get; }
    }

    public interface IEvent<TCurator> : ISocialMedia, ISquareFootageItem, IDatedItem, IColor where TCurator : ICurator
    {
        string Geolocation { get; }
        /// <summary>
        /// No specific hi-dpi paths will be in the database. 
        /// Instead the standard @2x suffix will be used where hi-dpi support is implemented. 
        /// This also applies to all subsequent image file paths in this document.
        /// </summary>
        string PhotoFilePath { get; }
        List<TCurator> Curators { get; }        
    }

    public interface IEvent : IEvent<Curator>
    {
        
    }

    public interface IDatedItem
    {
        string DateRange { get; }
        DateTime DateStart { get; }
        DateTime DateEnd { get; }
    }

    public interface ICurator<TExhibit> : ISquareFootageItem, ISocialMedia, IColor where TExhibit : IExhibit
    {
        string LogoFileName { get; }
        /// <summary>
        /// CMS to enforce protocol prefix, i.e. http:// or https:// 
        /// </summary>
        string ExternalUrl { get; }
        List<TExhibit> Exhibits { get; }
    }

    public interface ICurator : ICurator<Exhibit>
    {
        
    }

    public interface IExhibit : ISquareFootageItem, ITaggable
    {
        /// <summary>
        /// Optional field if name is not in the title
        /// </summary>
        string Exhibitor { get; }
        string FacebookEventUrl { get; }
        /// <summary>
        /// No specific hi-dpi paths will be in the database. Instead the standard @2x suffix will be used where hi-dpi support is implemented. This also applies to all subsequent image file paths in this document.
        /// </summary>
        string PhotoFilePath { get; }
        /// <summary>
        /// Not mandatory.
        /// </summary>
        string VideoFilePath { get; }
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
        string SeoMetaDescription { get; }
    }
}