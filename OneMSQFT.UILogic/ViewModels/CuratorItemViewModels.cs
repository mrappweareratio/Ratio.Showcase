using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.ViewModels
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
