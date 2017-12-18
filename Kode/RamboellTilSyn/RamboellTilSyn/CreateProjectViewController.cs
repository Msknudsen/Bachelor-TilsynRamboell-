using Foundation;
using System;
using UIKit;

namespace Ramboell.iOS
{
    public partial class CreateProjectViewController : UIViewController
    {
        public CreateProjectViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
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