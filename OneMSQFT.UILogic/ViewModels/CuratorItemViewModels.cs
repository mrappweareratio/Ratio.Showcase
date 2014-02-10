﻿using Microsoft.Practices.Prism.StoreApps;
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
        public override string Id { get; set; }

        public CuratorItemViewModel(ICurator curatorModel)
        {
            Curator = curatorModel;
            Name = curatorModel.Name;
            Id = curatorModel.Id;
            Description = curatorModel.Description;
        }
        private ICurator Curator { get; set; }
    }
}
