﻿using System;
using Windows.UI;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.Utils
{
    public class ColorUtils
    {
        public static Color GetEventColor(IEvent<IExhibit<ICurator>> e)
        {
            var defaultColor = Colors.Blue;
            return GetColorFromHexString(e.Color, defaultColor);
        }

        public static Color GetExhibitColor(IExhibit<ICurator> e)
        {
            var defaultColor = Colors.Orange;
            return GetColorFromHexString(e.Color, defaultColor);
        }        

        public static Color GetColorFromHexString(string colorHex, Color defaultColor)
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

        public static Color GetColorFromARGBString(string colorARGB, Color defaultColor)
        {
            if (String.IsNullOrEmpty(colorARGB) || colorARGB.Length != 9)
            {
                return defaultColor;
            }
            var c = Color.FromArgb(
            Convert.ToByte(colorARGB.Substring(1, 2), 16),
            Convert.ToByte(colorARGB.Substring(3, 2), 16),
            Convert.ToByte(colorARGB.Substring(5, 2), 16),
            Convert.ToByte(colorARGB.Substring(7, 2), 16));
            return c;
        }     
    }
}
