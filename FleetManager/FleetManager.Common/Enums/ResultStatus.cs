using FleetManager.Common.Attributes;
using FleetManager.Common.Constants;

namespace FleetManager.Common.Enums
{
    public enum ResultStatus
    {
        [Message(Message = MessageConstant.Success)]
        Success = 1,

        [Message(Message = MessageConstant.NotFound)]
        NotFound = 2,

        [Message(Message = MessageConstant.ValidationError)]
        ValidationError = 3,

        [Message(Message = MessageConstant.GenericError)]
        Exception = 4
    }
}
