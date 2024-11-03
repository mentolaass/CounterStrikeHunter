using System;

namespace CounterStrikeHunter.Core.Service.Notification
{
    public class Notification
    {
        public string Message { get; set; }

        public Type MessageType { get; set; }

        public long Timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public int IntMessageType
        {
            get => (int) MessageType;
            set { }
        }

        public enum Type
        {
            ERROR = 0,
            SUCCESS = 1,
            INFO = 2
        }
    }
}
