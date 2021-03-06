﻿using System;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.Utils
{
    public class DateUtils
    {
        public static bool? IsInTheFuture(IDatedItem datedItem)
        {
            if (datedItem == null) return null;
            var start = datedItem.DateStart;
            if (start.HasValue)
            {
                return DateTime.Today <= start.GetValueOrDefault().Date;
            }
            var end = datedItem.DateEnd;
            if (end.HasValue)
            {
                return DateTime.Today <= end.GetValueOrDefault().Date;
            }
            return null;
        }

        public static bool? IsInThePast(IDatedItem datedItem)
        {
            if (datedItem == null) return null;
            var end = datedItem.DateEnd;
            if (end.HasValue)
            {
                return end.GetValueOrDefault().Date < DateTime.Today;
            }
            var start = datedItem.DateStart;
            if (start.HasValue)
            {
                return start.GetValueOrDefault().Date < DateTime.Today;
            }
            return null;
        }
    }
}
