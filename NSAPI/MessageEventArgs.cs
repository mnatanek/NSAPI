using System;

namespace NSAPI
{
    public class MessageEA : EventArgs
    {
        public string Message { get; }

        public MessageEA(string msg)
        {
            Message = msg;
        }
    }
}
