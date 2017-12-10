// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Ramboell.iOS
{
    [Register ("CreateProjectViewController")]
    partial class CreateProjectViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChosePDFBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CustomernameCreateProjectLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CustomernameCreateProjectTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProjectadressCreateProjectLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ProjectadressCreateProjectTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProjectnameCreateProjectLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ProjectnameCreateProjectTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProjectnumberCreateProjectLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ProjectnumberCreateProjectTxt { get; set; }

        [Action ("UIButton28370_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton28370_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ChosePDFBtn != null) {
                ChosePDFBtn.Dispose ();
                ChosePDFBtn = null;
            }

            if (CustomernameCreateProjectLabel != null) {
                CustomernameCreateProjectLabel.Dispose ();
                CustomernameCreateProjectLabel = null;
            }

            if (CustomernameCreateProjectTxt != null) {
                CustomernameCreateProjectTxt.Dispose ();
                CustomernameCreateProjectTxt = null;
            }

            if (ProjectadressCreateProjectLabel != null) {
                ProjectadressCreateProjectLabel.Dispose ();
                ProjectadressCreateProjectLabel = null;
            }

            if (ProjectadressCreateProjectTxt != null) {
                ProjectadressCreateProjectTxt.Dispose ();
                ProjectadressCreateProjectTxt = null;
            }

            if (ProjectnameCreateProjectLabel != null) {
                ProjectnameCreateProjectLabel.Dispose ();
                ProjectnameCreateProjectLabel = null;
            }

            if (ProjectnameCreateProjectTxt != null) {
                ProjectnameCreateProjectTxt.Dispose ();
                ProjectnameCreateProjectTxt = null;
            }

            if (ProjectnumberCreateProjectLabel != null) {
                ProjectnumberCreateProjectLabel.Dispose ();
                ProjectnumberCreateProjectLabel = null;
            }

            if (ProjectnumberCreateProjectTxt != null) {
                ProjectnumberCreateProjectTxt.Dispose ();
                ProjectnumberCreateProjectTxt = null;
            }
        }
    }
}