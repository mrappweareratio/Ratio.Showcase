using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel
    {
        public ExhibitItemViewModel(IExhibit exhibitModel)
        {
            Exhibit = exhibitModel;
            Name = exhibitModel.Name;
            Description = exhibitModel.Description;
            Id = exhibitModel.Id;
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
        public String SquareFootageString
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", Exhibit.SquareFootage)).Trim();
            }
        }
        public double ExhibitItemHeight { get { return Window.Current.Bounds.Height / 4; } }
        public double ExhibitItemWidth { get { return Window.Current.Bounds.Width / 3; } }
    }
}
