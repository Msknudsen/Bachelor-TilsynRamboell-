using System;
using Firebase.Auth;
using Foundation;
using Xamarin.Forms;
using Ramboell.iOS;
[assembly: Dependency(typeof(FireBaseAuthService))]
namespace Ramboell.iOS
{
    public class FireBaseAuthService : IFireBaseAuthService
    {
        
        public event EventHandler<MessagingEventHandler> FirebaseEvent;
        private const string PasswordUpdatedEventMsg = "PasswordUpdated";
        private const string UserCreatedEventMsg = "UserCreated";
        private const string UserSignedInEventMsg = "UserSignedIn";
        private const string UserSignedOutEventMsg = "UserSignedOut";

        public void CreateUser(string email, string password)
        {
            Auth.DefaultInstance.CreateUser(email, password, (user, error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that CreateUser method could throw
                    switch (errorCode)
                    {
                        case AuthErrorCode.InvalidEmail:
                        case AuthErrorCode.EmailAlreadyInUse:
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.WeakPassword:
                        default:
                            RaiseEvent(UserCreatedEventMsg);
                            // Print error
                            break;
                    }
                }
                else
                {
                    // Do your magic to handle authentication result
                }
            });
        }

        public void SignIn(string email, string password)
        {
            Auth.DefaultInstance.SignIn(email, password, (user, error) => {
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
                            RaiseEvent(errorCode.ToString("g"));
                            break;
                    }
                }
                else
                {
                    RaiseEvent(UserSignedInEventMsg);
                }
            });
        }

        private void RaiseEvent(string msg)
        {
            FirebaseEvent?.Invoke(this, new MessagingEventHandler(msg));
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
                        RaiseEvent(errorCode.ToString("g"));
                        break;
                }
            }
            RaiseEvent(UserSignedOutEventMsg);
            // Do your magic to handle successful signout
        }

        public void DeleteUser(string password)
        {
            var user = Auth.DefaultInstance.CurrentUser;
            user.UpdatePassword(password, (error) => {
                if (error != null)
                {
                    AuthErrorCode errorCode;
                    if (IntPtr.Size == 8) // 64 bits devices
                        errorCode = (AuthErrorCode)((long)error.Code);
                    else // 32 bits devices
                        errorCode = (AuthErrorCode)((int)error.Code);

                    // Posible error codes that UpdatePassword method could throw
                    // Visit https://firebase.google.com/docs/auth/ios/errors for more information
                    switch (errorCode)
                    {
                        case AuthErrorCode.OperationNotAllowed:
                        case AuthErrorCode.RequiresRecentLogin:
                        case AuthErrorCode.WeakPassword:
                        default:
                            RaiseEvent(errorCode.ToString("g"));
                            break;
                    }
                }
                else
                {
                    RaiseEvent("Password Updated Succesfully");
                }
            });
        }
    }
}