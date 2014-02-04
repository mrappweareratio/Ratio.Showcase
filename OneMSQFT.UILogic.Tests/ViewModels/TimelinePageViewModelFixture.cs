using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class TimelinePageViewModelFixture
    {
        [TestMethod]
        public void TimelinePageViewModel_Implements_Interface()
        {
            var vm = new TimelinePageViewModel() as ITimelinePageViewModel;
            Assert.IsNotNull(vm);            
        }

        [TestMethod]
        public void TimelinePageViewModel_Constructs()
        {
            var vm = new TimelinePageViewModel() as ITimelinePageViewModel;
            Assert.IsNotNull(vm.SquareFootFutureEvents);
            Assert.IsNotNull(vm.SquareFootPastEvents);
            Assert.IsNotNull(vm.TimeLineItems);
            Assert.IsNotNull(vm.TimeLineMenuItems);
        }
    }
}
