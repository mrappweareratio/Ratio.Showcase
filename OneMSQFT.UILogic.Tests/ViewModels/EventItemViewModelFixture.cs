using System;
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
                vm = new EventItemViewModel(ev, new MockAnalyticsService());
            });
            Assert.IsFalse(vm.ShowMoreCommand.CanExecute(), "CanExecute");
            Assert.IsFalse(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel);
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
                vm = new EventItemViewModel(ev, new MockAnalyticsService());
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4);
            Assert.IsFalse(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "No Show More Item");
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
                vm = new EventItemViewModel(ev, new MockAnalyticsService());
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4, "Displays 4 Total");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
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
                vm = new EventItemViewModel(ev, new MockAnalyticsService());
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4, "Displays 4 Total");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "3", "Next Id Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 3, "Displays Next 2 with Show More");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
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
                vm = new EventItemViewModel(ev, new MockAnalyticsService());
            });
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4, "Displays 1st 3 with Show More");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "4", "Next Id 4 Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4, "Displays Next 3 with Show More");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "7", "Next Id 7 Loaded");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 2, "Displays 7th item with Show More");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
            Assert.IsTrue(vm.ShowMoreCommand.CanExecute(), "ShowMoreCanExecute");
            
            await ExecuteOnUIThread(() => vm.ShowMoreCommand.Execute());
            Assert.AreEqual(vm.DisplayedExhibits.First().Id, "1", "Back to Id 1");
            Assert.IsTrue(vm.DisplayedExhibits.Count == 4, "Displays 1st 3 with Show More");
            Assert.IsTrue(vm.DisplayedExhibits.Last() is ShowMoreFakeExhibitItemViewModel, "Last is Show More Item");
        }

        [TestMethod]
        public void StartDate_Comparison()
        {
            var evt = MockModelGenerator.NewEvent("0", "Event");
            evt.DateStart = DateTime.Now.AddDays(-1);
            evt.DateEnd = null;
            var vm = new EventItemViewModel(evt);
            Assert.IsNotNull(vm.IsInTheFuture);
            Assert.IsNotNull(vm.IsInThePast);
            Assert.IsTrue(vm.IsInThePast.GetValueOrDefault(), "IsInThePast");
        }

        [TestMethod]
        public void EndDate_Comparison()
        {
            var evt = MockModelGenerator.NewEvent("0", "Event");
            evt.DateStart = null;
            evt.DateEnd = DateTime.Now.AddDays(1);
            var vm = new EventItemViewModel(evt);
            Assert.IsNotNull(vm.IsInTheFuture);
            Assert.IsNotNull(vm.IsInThePast);
            Assert.IsTrue(vm.IsInTheFuture.GetValueOrDefault(), "IsInTheFuture");
        }

        [TestMethod]
        public void NullDate_Comparison()
        {
            var evt = MockModelGenerator.NewEvent("0", "Event");
            evt.DateStart = null;
            evt.DateEnd = null;
            var vm = new EventItemViewModel(evt);
            Assert.IsNull(vm.IsInTheFuture);
            Assert.IsNull(vm.IsInThePast);         
        }
    }
}
