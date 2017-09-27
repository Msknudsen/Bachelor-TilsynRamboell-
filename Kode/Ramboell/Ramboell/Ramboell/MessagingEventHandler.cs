using System;

namespace Ramboell.iOS
{
    public class MessagingEventHandler : EventArgs
    {
        public string Message { get; set; }
        public MessagingEventHandler(string s) => Message = s;
    }
}