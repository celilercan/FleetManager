using FleetManager.Common.Attributes;
using System;
using System.Linq;

namespace FleetManager.Common.Extension
{
    public static class GenericExtensions
    {
        public static string GetMessage(this Enum val)
        {
            var messageAttr = GetAttributeOfType<MessageAttribute>(val);

            return messageAttr?.Message ?? val.ToString();
        }

        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            if (memInfo.Length <= 0) return null;
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();
        }
    }
}
