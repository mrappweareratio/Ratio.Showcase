using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;
using System.Globalization;

namespace OneMSQFT.UILogic.ViewModels
{
    public abstract class ItemBaseViewModel : BindableBase, ISquareFootageItem
    {
        /// <summary>
        /// Inherited classes must declare Id
        /// http://stackoverflow.com/questions/15834403/get-properties-from-derived-class-in-base-class
        /// </summary>
        public abstract String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }       
        public int SquareFootage { get; set; }
        public String SquareFootageString  
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", SquareFootage)).Trim() + " sqft";
            }
        }
    }
}
