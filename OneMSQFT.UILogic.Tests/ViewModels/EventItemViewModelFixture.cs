﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class EventItemViewModelFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        async public Task ShowMore_Disabled_4_Or_Less()
        {
            var ev = MockModelGenerator.NewEvent("0", "event");
            ev.Exhibits = new List<Exhibit>
            {
                MockModelGenerator.NewExhibit("0", "Exhibit"),
                MockModelGenerator.NewExhibit("1", "Exhibit"),
                MockModelGenerator.NewExhibit("2", "Exhibit"),
                MockModelGenerator.NewExhibit("3", "Exhibit")
            };
            EventItemViewModel vm = null;
            await ExecuteOnUIThread(() =>
            {
                vm = new EventItemViewModel(ev);
            });
            Assert.IsFalse(vm.ShowMoreCommand.CanExecute(), "CanExecute");
            Assert.AreEqual(vm.ShowMoreVisibility, Visibility.Collapsed, "Collapsed");
        }


        [TestMethod]
        async public Task ShowMore_Displays_4_Or_Less()
        {
            var ev = MockModelGenerator.NewEvent("0", "event");
            ev.Exhibits = new List<Exhibit>
            {
                MockModelGenerator.NewExhibit("0", "Exhibit"),
                MockModelGenerator.NewExhibit("1", "Exhibit"),
                MockModelGenerator.NewExhibit("2", "Exhibit"),
                MockModelGenerator.NewExhibit("3", "Exhibit")
            };
            EventItemViewModel vm = null;
            await ExecuteOnUIThread(() =>
            {
                vm = new EventItemViewModel(ev);
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4);
        }

        [TestMethod]
        async public Task ShowMore_Displays_3_with_ShowMore()
        {
            var ev = MockModelGenerator.NewEvent("0", "event");
            ev.Exhibits = new List<Exhibit>
            {
                MockModelGenerator.NewExhibit("0", "Exhibit"),
                MockModelGenerator.NewExhibit("1", "Exhibit"),
                MockModelGenerator.NewExhibit("2", "Exhibit"),
                MockModelGenerator.NewExhibit("3", "Exhibit"),
                MockModelGenerator.NewExhibit("4", "Exhibit")
            };
            EventItemViewModel vm = null;
            await ExecuteOnUIThread(() =>
            {
                vm = new EventItemViewModel(ev);
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3, "Displays 3");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            Assert.AreEqual(vm.ShowMoreVisibility, Visibility.Visible, "Visible");
        }

        [TestMethod]
        async public Task ShowMore_Displays_Next_after_ShowMore()
        {
            var ev = MockModelGenerator.NewEvent("0", "event");
            ev.Exhibits = new List<Exhibit>
            {
                MockModelGenerator.NewExhibit("0", "Exhibit"),
                MockModelGenerator.NewExhibit("1", "Exhibit"),
                MockModelGenerator.NewExhibit("2", "Exhibit"),
                MockModelGenerator.NewExhibit("3", "Exhibit"),
                MockModelGenerator.NewExhibit("4", "Exhibit")
            };
            EventItemViewModel vm = null;
            await ExecuteOnUIThread(() =>
            {
                vm = new EventItemViewModel(ev);
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3, "Displays 3");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "3", "Next Id Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 2);
        }
        [TestMethod]
        async public Task ShowMore_Loops()
        {
            var ev = MockModelGenerator.NewEvent("0", "event");
            ev.Exhibits = new List<Exhibit>
            {                
                MockModelGenerator.NewExhibit("1", "Exhibit"),
                MockModelGenerator.NewExhibit("2", "Exhibit"),
                MockModelGenerator.NewExhibit("3", "Exhibit"),
                MockModelGenerator.NewExhibit("4", "Exhibit"),
                MockModelGenerator.NewExhibit("5", "Exhibit"),
                MockModelGenerator.NewExhibit("6", "Exhibit"),
                MockModelGenerator.NewExhibit("7", "Exhibit")
            };
            EventItemViewModel vm = null;
            await ExecuteOnUIThread(() =>
            {
                vm = new EventItemViewModel(ev);
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3, "Displays 3");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "4", "Next Id 4 Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3);
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "7", "Next Id 7 Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 1);
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "1", "Back to Id 1");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3);
        }
    }
}