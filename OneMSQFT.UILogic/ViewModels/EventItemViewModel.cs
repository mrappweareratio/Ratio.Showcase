﻿using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System;
using System.Collections;
using Windows.UI.Xaml;
using System.Globalization;
using System.Collections.ObjectModel;

namespace OneMSQFT.UILogic.ViewModels
{
    public class EventItemViewModel : ItemBaseViewModel
    {
        public override string Id { get; set; }

        public EventItemViewModel(IEvent<IExhibit> eventModel)
        {
            Event = eventModel;
            Name = eventModel.Name;
            Description = eventModel.Description;
            Id = eventModel.Id;
            SquareFootage = eventModel.SquareFootage;
            Exhibits = new ObservableCollection<ExhibitItemViewModel>();
        }
        
        private IEvent<IExhibit> Event { get; set; }
    
        public ObservableCollection<ExhibitItemViewModel> Exhibits { get; private set; }
        
        public DateTime DateStart { get { return Event.DateStart; } }

        public Uri EventHeroVideoUri
        {
            get
            {
                if (Event.EventHeroVideoPath != null)
                {
                    return new Uri(Event.EventHeroVideoPath, UriKind.Absolute);
                }
                return new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
            }
        }

        public Uri PhotoFilePath
        {
            get
            {
                if (Event.PhotoFilePath != null)
                {
                    return new Uri(Event.PhotoFilePath, UriKind.Absolute);
                }
                return new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
            }
        }
        public bool IsInTheFuture
        {
            get
            {
                return (Event.DateStart > DateTime.Now);
            }
        }

        public SolidColorBrush EventColor
        {
            get
            {
                string color = "FF" + Event.Color;
                var c = Color.FromArgb(
                Convert.ToByte(color.Substring(0, 2), 16),
                Convert.ToByte(color.Substring(2, 2), 16),
                Convert.ToByte(color.Substring(4, 2), 16),
                Convert.ToByte(color.Substring(6, 2), 16));

                return new SolidColorBrush(c);
            }
        }

        public double ItemWidth { get { return Window.Current.Bounds.Width; } }
        public double ItemHeight { get { return Window.Current.Bounds.Width; } }
    }
}
