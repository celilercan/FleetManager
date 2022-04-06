using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Service.Mapping;
using FleetManager.Service.Shipment.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class AddBagCommandHandler : IRequestHandler<AddBagCommand, ResultDto<bool>>
    {
        private readonly IRedisProvider _redisProvider;

        public AddBagCommandHandler(IRedisProvider redisProvider)
        {
            _redisProvider = redisProvider;
        }

        public async Task<ResultDto<bool>> Handle(AddBagCommand request, CancellationToken cancellationToken)
        {
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, request.AddBagRequest.Barcode);
            if (string.IsNullOrEmpty(request.AddBagRequest.Barcode) || await _redisProvider.IsExistAsync(shipmentKey))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var shipment = DtoMapper.Mapper.Map<Data.Entities.Shipment>(request.AddBagRequest);
            shipment.ShipmentState = (int)BagStatus.Created;
            shipment.ShipmentType = ShipmentType.Bag;

            await _redisProvider.SetAsync(shipmentKey, shipment);

            var bag = DtoMapper.Mapper.Map<Data.Entities.Bag>(request.AddBagRequest);
            var bagKey = string.Format(Constant.Shipment.BagByBarcodeKey, request.AddBagRequest.Barcode);
            
            await _redisProvider.SetAsync(bagKey, bag);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }
    }
}
