using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.ViewModels
{
    public class LinkItemViewModel : BindableBase
    {
        private ILink Link { get; set; }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public LinkItemViewModel(ILink linkModel)
        {
            Id = linkModel.Id;
            Title = linkModel.Title;
            Url = linkModel.Url;
            Link = linkModel;
        }
    }
}
