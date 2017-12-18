using Foundation;
using System;
using UIKit;

namespace Ramboell.iOS
{
    /// <summary>
    /// Not implemented yet, to be added
    /// </summary>
    public partial class CreateProjectViewController : UIViewController
    {
        public CreateProjectViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //ChoosePdfBtn.TouchUpInside += ChoosePdf;
            
        }

        private void ChoosePdf(object sender, EventArgs eventArgs)
        {
            this.Storyboard.InstantiateViewController("ChoosePdfViewController");
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
        }
    }
}