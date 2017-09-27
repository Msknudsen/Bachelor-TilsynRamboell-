﻿using System;

namespace Ramboell.iOS
{
    public interface IFireBaseAuthService
    {
        event EventHandler<MessagingEventHandler> FirebaseEvent;
        void CreateUser(string email, string password);
        void DeleteUser(string password);
        void SignIn(string email, string password);
        void SignOut();
    }
}