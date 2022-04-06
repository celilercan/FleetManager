using System;

namespace FleetManager.Common.Attributes
{
    public class MessageAttribute : Attribute
    {
        public MessageAttribute(string message)
        {
            this.Message = message;
        }

        public MessageAttribute()
        {
        }

        public string Message { get; set; }
    }
}
