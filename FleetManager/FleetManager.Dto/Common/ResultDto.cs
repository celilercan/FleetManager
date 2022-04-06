using FleetManager.Common.Enums;
using FleetManager.Common.Extension;

namespace FleetManager.Dto.Common
{
    public class ResultDto<T>
    {
        public bool IsSuccess => this.Status == ResultStatus.Success;
        public ResultStatus Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public ResultDto()
        {

        }

        public ResultDto(ResultStatus status)
        {
            this.Status = status;
            this.Message = status.GetMessage();
        }

        public ResultDto(ResultStatus status, T data)
        {
            this.Status = status;
            this.Data = data;
        }

        public ResultDto(ResultStatus status, T data, string message)
        {
            this.Status = status;
            this.Data = data;
            this.Message = message;
        }
    }
}
