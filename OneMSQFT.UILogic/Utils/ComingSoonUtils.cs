using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Utils
{
    public class ComingSoonUtils
    {
        public static readonly Dictionary<int, int[]> SlotsDictionaryByPageSize = new Dictionary<int, int[]>()
        {
           {12, new []{2,3,8,10}}
        };

        public static List<EventItemViewModel> InsertComingSoonItemsFillingPage(int pageSize, List<EventItemViewModel> events)
        {
            if (!SlotsDictionaryByPageSize.ContainsKey(pageSize))
            {
                throw new ArgumentOutOfRangeException("pageSize");
            }
            var eventsPerPage = pageSize - SlotsDictionaryByPageSize[pageSize].Length;
            var pages = Math.Ceiling((decimal)events.Count / eventsPerPage);
            var slots = pages * pageSize;
            for (var i = 0; i < slots; i++)
            {
                var isEmpty = SlotsDictionaryByPageSize[pageSize].Contains(i % pageSize);
                if (isEmpty)
                {
                    //If index is equal to Count, item is added to the end of List<T>.
                    events.Insert(i, new ComingSoonFakeEventItemViewModel());
                }
                else if (i > events.Count - 1)
                {
                    events.Add(new ComingSoonFakeEventItemViewModel());
                }
                else
                {
                    continue;
                }
            }
            return events;
        }

        public static List<EventItemViewModel> InsertComingSoonItems(int pageSize, List<EventItemViewModel> events)
        {
            if (!SlotsDictionaryByPageSize.ContainsKey(pageSize))
            {
                throw new ArgumentOutOfRangeException("pageSize");
            }
            var eventsPerPage = pageSize - SlotsDictionaryByPageSize[pageSize].Length;
            var pages = Math.Ceiling((decimal)events.Count / eventsPerPage);
            var slots = pages * pageSize;
            for (var i = 0; i < slots; i++)
            {
                if (i > events.Count - 1)
                {
                    continue;
                }
                var isEmpty = SlotsDictionaryByPageSize[pageSize].Contains(i % pageSize);
                if (isEmpty)
                {
                    //If index is equal to Count, item is added to the end of List<T>.
                    events.Insert(i, new ComingSoonFakeEventItemViewModel());
                }
                else
                {
                    continue;
                }
            }
            return events;
        }
    }
}
