using Windows.UI;
using Windows.UI.Xaml.Media;
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


        public SolidColorBrush EventColor
        {
            get
            {
                string color = "FF" + Exhibit.Color;
                var c = Color.FromArgb(
                Convert.ToByte(color.Substring(0, 2), 16),
                Convert.ToByte(color.Substring(2, 2), 16),
                Convert.ToByte(color.Substring(4, 2), 16),
                Convert.ToByte(color.Substring(6, 2), 16));

                return new SolidColorBrush(c);
            }
        }
    }
}
