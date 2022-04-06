using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Commands
{
    public class AddPackageToBagCommand : IRequest<ResultDto<bool>>
    {
        public AddPackageToBagDto AddPackageToBagRequest { get; set; }

        public AddPackageToBagCommand(AddPackageToBagDto addPackageToBagRequest)
        {
            AddPackageToBagRequest = addPackageToBagRequest;
        }
    }
}
