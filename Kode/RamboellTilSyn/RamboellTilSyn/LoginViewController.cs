﻿using System;
using System.ComponentModel;
using Firebase.Auth;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    [DesignTimeVisible(false)]
    public partial class LoginViewController : UIViewController
    {
        NSObject listenerHandle;

        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public void CheckIfUserSignedIn()
        {
            listenerHandle = Auth.DefaultInstance.AddAuthStateDidChangeListener((auth, user) =>
            {
                if (user != null)
                {
                    Console.WriteLine("User Currently Logged In: {0}", user.Email);

                    if (Storyboard.InstantiateViewController("ProjectListViewController") is ProjectListViewController projectList)
                    {
                        NavigationController.RemoveFromParentViewController();
                        NavigationController.PushViewController(projectList, true);
                    }
                }
                else
                {
                    Console.WriteLine("No Users logged In");
                }
            });
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoginBtn.TouchUpInside += LoginEventHandler;
            //LogOutBtn.TouchUpInside += LogOutBtn_TouchUpInside;

            LoginEmailTxt.Placeholder = "Indtast Email";

            LoginPasswordTxt.SecureTextEntry = true;
            LoginPasswordTxt.Placeholder = "Indtast kodeord";

            CheckIfUserSignedIn();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            LoginBtn.TouchUpInside -= LoginEventHandler;
        }

        private void LoginEventHandler(object sender, EventArgs e)
        {
            if (!Validator.EmailIsValid(LoginEmailTxt.Text) || !Validator.PasswordIsValid(LoginPasswordTxt.Text))
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Invalid email or password"
                };
                alert.AddButton("OK");
                alert.Show();
            }
            else
                Auth.DefaultInstance.SignIn(LoginEmailTxt.Text, LoginPasswordTxt.Text, (user, error) =>
            {
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
                    if (Storyboard.InstantiateViewController("ProjectListViewController") is ProjectListViewController projectList)
                    {
                        NavigationController.PushViewController(projectList, true);
                    }
                }
            });

        }

        private void SignOut()
        {
            var signedOut = Auth.DefaultInstance.SignOut(out var error);

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
    }
}