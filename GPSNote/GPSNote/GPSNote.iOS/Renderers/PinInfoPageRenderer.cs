using GPSNote.iOS;
using GPSNote.iOS.Renderers;
using GPSNote.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(PinInfoPopup), typeof(PinInfoPageRenderer))]
namespace GPSNote.iOS.Renderers
{
    class PinInfoPageRenderer : PageRenderer
    {
        /// <summary>
        /// The parent.
        /// </summary>
        private UIViewController _parentModalViewController;
        #region -- Overrides --
        /// <summary>
        /// Dids the move to parent view controller.
        /// </summary>
        /// <param name=“parent”>Parent.</param>
        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            // Save modal wrapper from Xamarin.Forms
            _parentModalViewController = parent;
            // Set custom to be able to set clear background!
            parent.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        }
        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name=“animated”>If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(false);
            // Clear background on parent modal wrapper!!
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }
        /// <summary>
        /// Views the did appear.
        /// </summary>
        /// <param name=“animated”>If set to <c>true</c> animated.</param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(false);
            // Clear background on parent modal wrapper!!
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }
        #endregion
    }
}
