using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ViewModel
    {
        public EventItemViewModel()
        {
                
        }
        public Event Event { get; set; }

        public SolidColorBrush EventColor
        {
            get
            {
                string color = "FF" + Event.Color;
                var c = Color.FromArgb(
                Convert.ToByte(color.Substring(1, 2), 16),
                Convert.ToByte(color.Substring(3, 2), 16),
                Convert.ToByte(color.Substring(5, 2), 16),
                Convert.ToByte(color.Substring(5, 2), 16));

                return new SolidColorBrush(c);
            }
        }
    }
}
