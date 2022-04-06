using FleetManager.Common.Enums;
using FleetManager.Dto.Common;
using FleetManager.Service.Shipment.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment.Handlers
{
    public class AddPackageToBagCommandHandler : IRequestHandler<AddPackageToBagCommand, ResultDto<bool>>
    {
        private readonly ShipmentService _bagService;

        public AddPackageToBagCommandHandler(ServiceResolver serviceResolver)
        {
            _bagService = serviceResolver(ShipmentType.Bag);
        }

        public async Task<ResultDto<bool>> Handle(AddPackageToBagCommand request, CancellationToken cancellationToken)
        {
            return await ((BagService)_bagService).AddPackageToBag(request.AddPackageToBagRequest);
        }
    }
}
