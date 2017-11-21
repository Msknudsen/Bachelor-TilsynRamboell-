using System;
using Firebase.Auth;
using Foundation;
using Security;
using UIKit;

namespace Ramboell.iOS
{
    public partial class LoginViewController : UIViewController
    {
        NSObject listenerHandle;

        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public void CheckIfUserSignedIn()
        {
            listenerHandle = Auth.DefaultInstance.AddAuthStateDidChangeListener((auth, user) => {
                if (user != null)
                {
                    Console.WriteLine($"User Currently Logged In: {0}", user.Email.ToString());

                    if (Storyboard.InstantiateViewController("ProjectListViewController") is ProjectListViewController projectList)
                    {
                        NavigationController.PushViewController(projectList, true);
                    }
                }
                else
                {
                    Console.WriteLine("No Users logged In");
                    // No user is signed in.
                }
            });
        }

        SecRecord s = new SecRecord(SecKind.GenericPassword)
        {
            ValueData = NSData.FromString("data"),
            Generic = NSData.FromString("name")
        };
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoginBtn.TouchUpInside += LoginBtn_TouchUpInside;
            CheckIfUserSignedIn();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            LoginBtn.TouchUpInside -= LoginBtn_TouchUpInside;
            SecKeyChain.Remove(s);
        }

        private void LoginBtn_TouchUpInside(object sender, EventArgs e)
        {
            Auth.DefaultInstance.SignIn("pass@123456.dk", "123456", (user, error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that SignIn method with email and password could throw
                    // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                    switch (errorCode)
                    {
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.InvalidEmail:
                        case AuthErrorCode.UserDisabled:
                        case AuthErrorCode.WrongPassword:
                        default:
                            // Print error
                            Console.WriteLine(errorCode.ToString());
                            break;
                    }
                }
                else
                {
                    if (this.Storyboard.InstantiateViewController("ProjectListViewController") is ProjectListViewController projectList)
                    {
                        this.NavigationController.PushViewController(projectList, true);
                    }
                }
            });
            
        }

        public void SignOut()
        {
            NSError error;
            var signedOut = Auth.DefaultInstance.SignOut(out error);

            if (!signedOut)
            {
                AuthErrorCode errorCode;
                if (IntPtr.Size == 8) // 64 bits devices
                    errorCode = (AuthErrorCode)((long)error.Code);
                else // 32 bits devices
                    errorCode = (AuthErrorCode)((int)error.Code);

                // Posible error codes that SignOut method with credentials could throw
                // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                switch (errorCode)
                {
                    case AuthErrorCode.KeychainError:
                    default:
                        Console.WriteLine(errorCode.ToString());
                        // Print error
                        break;
                }
            }
            Console.WriteLine("UserSigned Out");
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void LogOutBtn_TouchUpInside(UIButton sender)
        {
            SignOut();
        }
   }
}