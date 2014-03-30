using System;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.Shared;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.Utils;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public abstract class ItemBaseViewModel : BindableBase, IRatioItem
    {
        /// <summary>
        /// Inherited classes must declare Id
        /// http://stackoverflow.com/questions/15834403/get-properties-from-derived-class-in-base-class
        /// </summary>
        public abstract String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }                      
    }
}
