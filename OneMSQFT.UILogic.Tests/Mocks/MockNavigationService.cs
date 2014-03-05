// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public Func<string, object, bool> NavigateDelegate { get; set; }
        public Action GoBackDelegate { get; set; }
        public Func<bool> CanGoBackDelegate { get; set; }
        public Action ClearHistoryDelegate { get; set; }

        public bool Navigate(string pageToken, object parameter)
        {
            if (NavigateDelegate == null) return true;
            return NavigateDelegate(pageToken, parameter);
        }

        public void GoBack()
        {
            if (GoBackDelegate == null) return;
            GoBackDelegate();
        }

        public bool CanGoBack()
        {
            if (CanGoBackDelegate == null) return true;
            return CanGoBackDelegate();
        }

        public void ClearHistory()
        {
            if (ClearHistoryDelegate == null) return;
            ClearHistoryDelegate();
        }

        public void RestoreSavedNavigation()
        {
        }

        public void Suspending()
        {
        }

        public int BackStackDepth { get; set; }
    }
}
