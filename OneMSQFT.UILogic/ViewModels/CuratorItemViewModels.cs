using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.ViewModels
{
    public class CuratorItemViewModel : ItemBaseViewModel
    {
        public CuratorItemViewModel(Curator curatorModel)
        {
            Curator = curatorModel;
            Name = curatorModel.Name;
            Id = curatorModel.Id;
            Description = curatorModel.Description;
        }
        private Curator Curator { get; set; }
    }
}
