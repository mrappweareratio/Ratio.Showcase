using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneMSQFT.WindowsStore
{
    public class CustomFrame : Frame
    {
        Grid mainGrid;
        Grid contentContainer;
        Grid overlayContainer;
        UIElement overlay;

        public CustomFrame()
        {
            contentContainer = new Grid();
            overlayContainer = new Grid();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            /* The first time the content is set will be when frame.Navigate() is called from App.xaml.cs.
             * When that happens, we will stick the requested page into a grid that is behind the animation.
             * After your navigation is complete, just call frame.HideOverlay() to hide the overlay.
             * Future calls to frame.Navigate() will operate just like normal.
             * */

            if (oldContent == null && Overlay != null)
            {
                contentContainer.Children.Add(newContent as UIElement);
                mainGrid = new Grid();
                mainGrid.Children.Add(contentContainer);
                mainGrid.Children.Add(overlayContainer);
                this.Content = mainGrid;
            }
            else
            {
                base.OnContentChanged(oldContent, newContent);
            }
        }

        public UIElement Overlay
        {
            get { return overlay; }
            set 
            {
                this.overlay = value;
                this.overlayContainer.Children.Clear();
                this.overlayContainer.Children.Add(overlay);
            }
        }

        public void HideOverlay()
        {
            overlayContainer.Children.Clear();
        }
    }
}
