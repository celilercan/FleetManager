using FleetManager.Common.Enums;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment
{
    public delegate ShipmentService ServiceResolver(ShipmentType type);
 
    public abstract class ShipmentService
    {
        public abstract Task<ResultDto<bool>> Add(BaseShipmentDto dto);

        public abstract Task<ResultDto<ShipmentDeliveryResponseDto>> Delivery(ShipmentDeliveryRequestDto dto);

        public abstract Task<ResultDto<ShipmentDetailDto>> GetDetail(string barcode);
    }
}
