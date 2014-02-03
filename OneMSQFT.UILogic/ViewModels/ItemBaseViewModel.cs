using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ItemBaseViewModel : ViewModel
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String Id { get; set; }

    }
}
