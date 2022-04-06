using FleetManager.Dto.Common;
using FleetManager.Dto.DeliveryPoint;
using System.Threading.Tasks;

namespace FleetManager.Service.DeliveryPoint
{
    public interface IDeliveryPointService
    {
        Task<ResultDto<bool>> AddAsync(AddDeliveryPointDto dto);

        Task<ResultDto<bool>> UpdateAsync(UpdateDeliveryPointDto dto);

        Task<ResultDto<DeliveryPointDetailDto>> GetByIndexAsync(int index);

        Task<ResultDto<bool>> DeleteAsync(int index);
    }
}
