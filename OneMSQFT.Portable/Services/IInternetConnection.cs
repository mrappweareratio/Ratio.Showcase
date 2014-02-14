﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.Common.Services
{
    public interface IInternetConnection
    {
        bool IsConnected();
        event EventHandler InternetConnectionChanged;
    }

    public class InternetConnectionChangedEventArgs : EventArgs
    {
        public bool IsConnected { get; set; }
    }
}
