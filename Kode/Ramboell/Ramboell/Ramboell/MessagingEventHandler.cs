using System;

namespace Ramboell
{
    public class MessagingEventHandler : EventArgs
    {
        public string Message { get; set; }
        public MessagingEventHandler(string s) => Message = s;
    }
}