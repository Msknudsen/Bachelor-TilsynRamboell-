using System;
using Firebase.Auth;
using Foundation;

namespace Ramboell.iOS
{
    public class FireBaseCustomAuthEventHandler : EventArgs
    {
        public string Message { get; }
        public FireBaseCustomAuthEventHandler(string s)
        {
            Message = s;
        }
    }
    public class FireBaseAuthService : FirebaseService
    {
        public event EventHandler<FireBaseCustomAuthEventHandler> CustomFirebaseAuthEvent;
        
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
                            RaiseEvent(errorCode.ToString("g"));
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
                    // Do your magic to handle authentication result
                }
            });
        }

        private void RaiseEvent(string msg)
        {
            CustomFirebaseAuthEvent?.Invoke(this, new FireBaseCustomAuthEventHandler(msg));

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
                    // Password updated.
                }
            });
        }
    }
}