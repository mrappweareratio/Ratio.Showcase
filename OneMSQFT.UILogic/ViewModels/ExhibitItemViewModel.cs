using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel
    {
        public override string Id { get; set; }

        public ExhibitItemViewModel(IExhibit exhibitModel)
        {
            Exhibit = exhibitModel;
            Name = exhibitModel.Name;
            Description = exhibitModel.Description;
            Id = exhibitModel.Id;
            SquareFootage = exhibitModel.SquareFootage;
        }


        private IExhibit Exhibit { get; set; } 

        public Uri PhotoFilePath
        {
            get
            {
                if (Exhibit.PhotoFilePath != null)
                {
                    return new Uri(Exhibit.PhotoFilePath, UriKind.Absolute);
                }
                return new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
            }
        }
    }
}
