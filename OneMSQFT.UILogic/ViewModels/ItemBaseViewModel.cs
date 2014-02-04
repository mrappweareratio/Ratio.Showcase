using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ItemBaseViewModel : BindableBase, ISquareFootageItem
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }
        public int SquareFootage { get; set; }
    }
}
