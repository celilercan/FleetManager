using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment
{
    public interface IWrongDeliveryService
    {
        Task<ResultDto<bool>> AddAsync(AddWrongDeliveryDto dto);

        Task<ResultDto<WrongDeliveryDetailDto>> GetDetail(string barcode);
    }
}
