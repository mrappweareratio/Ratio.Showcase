﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.Utils
{
    public class ColorUtils
    {
        public static Color GetEventColor(IEvent<IExhibit> e)
        {
            var defaultColor = Colors.Blue;
            return GetColor(e.Color, defaultColor);
        }

        public static Color GetExhibitColor(IExhibit e)
        {
            var defaultColor = Colors.Orange;
            return GetColor(e.Color, defaultColor);
        }        

        public static Color GetColor(string colorHex, Color defaultColor)
        {
            if (String.IsNullOrEmpty(colorHex) || colorHex.Length != 6)
            {
                return defaultColor;  
            }
            string color = "FF" + colorHex;
            var c = Color.FromArgb(
            Convert.ToByte(color.Substring(0, 2), 16),
            Convert.ToByte(color.Substring(2, 2), 16),
            Convert.ToByte(color.Substring(4, 2), 16),
            Convert.ToByte(color.Substring(6, 2), 16));
            return c;
        }        
    }
}
