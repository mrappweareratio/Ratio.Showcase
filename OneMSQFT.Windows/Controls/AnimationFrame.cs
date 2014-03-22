using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Ratio.Showcase.Win8.Controls
{
    [TemplatePart(Name = "AnimationElement", Type = typeof(UIElement))]
    public class AnimationFrame : Frame
    {
        public UIElement AnimationElement { get; protected set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.AnimationElement = base.GetTemplateChild("AnimationElement") as UIElement;

        }
    }
}
