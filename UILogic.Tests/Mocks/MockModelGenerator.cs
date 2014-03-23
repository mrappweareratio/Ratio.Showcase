using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockModelGenerator
    {
        public static Event NewEvent(string id, string name)
        {
            return new Event()
            {
                Id = id,
                Name = name,
                Color = "AABBCC"
            };
        }

        public static Exhibit NewExhibit(string id, string name)
        {
            return new Exhibit()
            {
                Id = id,
                Name = name,
                Color = "AABBCC",
                ThumbImage = "http://url.com/image"
            };
        }
    }
}
