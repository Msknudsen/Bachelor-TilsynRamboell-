using System;
using Foundation;
using UIKit;

namespace RamboellView
{
    public partial class LoginViewController : UIViewController
    {
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

       
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoginBtn.TouchUpInside += LoginBtn_TouchUpInside;
            // Perform any additional setup after loading the view, typically from a nib.


        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            LoginBtn.TouchUpInside -= LoginBtn_TouchUpInside;

        }

        private void LoginBtn_TouchUpInside(object sender, EventArgs e)
        {
            //TODO add logic
            if (this.Storyboard.InstantiateViewController("ProjectOverviewViewController") is ProjectListViewController projectList)
            {
                this.NavigationController.PushViewController(projectList, true);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}