using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.Utils
{
    [TestClass]
    public class ComingSoonUtilsFixture
    {
        [TestMethod]
        public void InsertComingSoonItems_Slots_Few_No_Coming_Soon()
        {
            var events = new List<EventItemViewModel> {new EventItemViewModel(null), new EventItemViewModel(null)};
            events = ComingSoonUtils.InsertComingSoonItems(12, events);
            Assert.AreEqual(events.Count, 12, "Fills Page");
            Assert.AreEqual(events.Count(x => x is ComingSoonFakeEventItemViewModel), 10, "Coming Soon");
        }

        [TestMethod]
        public void InsertComingSoonItems_Slots_Coming_Soon_One_Page()
        {
            var events = new List<EventItemViewModel>
            {
                //8
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null)
            };
            events = ComingSoonUtils.InsertComingSoonItems(12, events);
            Assert.AreEqual(events.Count, 12, "Fills Page");
            Assert.AreEqual(events.Count(x => x is ComingSoonFakeEventItemViewModel), 4, "Coming Soon");
            foreach (var index in ComingSoonUtils.SlotsDictionaryByPageSize[12])
            {
                Assert.IsTrue(events[index] is ComingSoonFakeEventItemViewModel, String.Format("Coming Soon @ {0}", index));
            }
        }

        [TestMethod]
        public void InsertComingSoonItems_Slots_Coming_Soon_Two_Pages()
        {
            var events = new List<EventItemViewModel>
            {
                //16
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null),
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null)
            };
            events = ComingSoonUtils.InsertComingSoonItems(12, events);
            Assert.AreEqual(events.Count, 24, "Two Pages");
            Assert.AreEqual(events.Count(x => x is ComingSoonFakeEventItemViewModel), 8, "Coming Soon Count");
            foreach (var index in ComingSoonUtils.SlotsDictionaryByPageSize[12])
            {
                Assert.IsTrue(events[index] is ComingSoonFakeEventItemViewModel, String.Format("Coming Soon @ {0}", index));
            }
            foreach (var index in ComingSoonUtils.SlotsDictionaryByPageSize[12])
            {
                Assert.IsTrue(events[index + 12] is ComingSoonFakeEventItemViewModel, String.Format("Coming Soon @ {0}", index + 12));
            }
        }

        [TestMethod]
        public void InsertComingSoonItems_Slots_Coming_Soon_Two_Pages_Near_Full()
        {
            var events = new List<EventItemViewModel>
            {
                //14
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null),
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null), 
                new EventItemViewModel(null)                 
            };
            events = ComingSoonUtils.InsertComingSoonItems(12, events);
            Assert.AreEqual(events.Count, 24, "Two Pages");
            Assert.AreEqual(events.Count(x => x is ComingSoonFakeEventItemViewModel), 10, "Coming Soon Count");
            foreach (var index in ComingSoonUtils.SlotsDictionaryByPageSize[12])
            {
                Assert.IsTrue(events[index] is ComingSoonFakeEventItemViewModel, String.Format("Coming Soon @ {0}", index));
            }
            foreach (var index in ComingSoonUtils.SlotsDictionaryByPageSize[12])
            {
                Assert.IsTrue(events[index + 12] is ComingSoonFakeEventItemViewModel, String.Format("Coming Soon @ {0}", index + 12));
            }
        }
    }
}
