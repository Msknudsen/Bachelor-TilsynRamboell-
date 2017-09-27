using System;

namespace Ramboell
{
    public class FireBaseCustomAuthEventHandler : EventArgs
    {
        public string Message { get; }
        public FireBaseCustomAuthEventHandler(string s)
        {
            Message = s;
        }
    }
}