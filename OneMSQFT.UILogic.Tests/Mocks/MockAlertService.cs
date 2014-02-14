// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockAlertMessageService : IAlertMessageService
    {
        public Func<string, string, Task> ShowAsyncDelegate { get; set; }

        public Func<string, string, IEnumerable<DialogCommand>, Task> ShowAsyncWithCommandsDelegate { get; set; }

        public Task ShowAsync(string message, string title)
        {
            if (ShowAsyncDelegate != null)
                return ShowAsyncDelegate(message, title);
            return Task.FromResult(0);
        }

        public Task ShowAsync(string message, string title, IEnumerable<DialogCommand> dialogCommands)
        {
            if (ShowAsyncWithCommandsDelegate != null)
                return ShowAsyncWithCommandsDelegate(message, title, dialogCommands);
            return Task.FromResult(0);
        }
    }
}
