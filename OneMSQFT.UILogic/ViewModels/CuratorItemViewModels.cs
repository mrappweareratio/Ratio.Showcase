﻿using System.Runtime.InteropServices.WindowsRuntime;
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
        public override string Id { get; set; }
        public string LogoImage { get; set; }
        public string ExternalUrl { get; set; }

        public CuratorItemViewModel(ICurator curatorModel)
        {
            if (curatorModel == null)
                return;

            Name = curatorModel.Name;
            Id = curatorModel.Id;
            Description = curatorModel.Description;
            LogoImage = curatorModel.WhiteLogoImage; 
            ExternalUrl = curatorModel.ExternalUrl;
        }        
    }
}
