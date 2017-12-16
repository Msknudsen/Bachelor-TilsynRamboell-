using Foundation;
using System;
using AVFoundation;
using Firebase.Auth;
using Firebase.Database;
using UIKit;

namespace Ramboell.iOS
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class CreateUserViewController : UIViewController
    {
        DatabaseReference userNode;

        public CreateUserViewController(IntPtr handle) : base(handle)
        {
            userNode = Database.DefaultInstance.GetRootReference().GetChild(Global.User);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateUserBtn.TouchUpInside += CreateUserEventHandler;
        }

        private void CreateUserEventHandler(object sender, EventArgs e)
        {
            if (IsFieldsValid())
                Auth.DefaultInstance.CreateUser(EmailCreateUserTxt.Text, PasswordCreateUserTxt.Text, (user, error) => {
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
                                Console.WriteLine("Failed to Create to User");
                                //TODO ERROR HANDLING LOGIC
                                break;
                                    }
                        }
                    
                    else
                    {
                        Console.WriteLine("Succesfully Created A User");
                        userNode = userNode.GetChild(user.Uid);
                        object[] keys = {
                        "alias",
                        "email",
                        "firstName",
                        "lastName"
                    };
                        object[] values = {
                        PhoneCreateUserTxt.Text,
                        EmailCreateUserTxt.Text,
                        FirstnameCreateUserTxt.Text,
                        LastnameCreateUserTxt.Text
                    };
                        var data = NSDictionary.FromObjectsAndKeys(values, keys, keys.Length);
                        userNode.SetValue<NSDictionary>(data);
                    }
            });
        }

        private bool IsFieldsValid()
        {
            return IsValidEmail() && IsValidName() && IsValidPhoneNumber();
        }

        private bool IsValidPhoneNumber()
        {
            return PhoneCreateUserTxt.HasText;
        }

        private bool IsValidName()
        {
            return FirstnameCreateUserTxt.HasText && FirstnameCreateUserTxt.Text.Length > 2 && Validator.NameIsValid(FirstnameCreateUserTxt.Text);
        }


        private bool IsValidEmail()
        {
            return Validator.EmailIsValid(EmailCreateUserTxt.Text);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            CreateUserBtn.TouchUpInside -= CreateUserEventHandler;
        }
    }
}