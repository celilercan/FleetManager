using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Commands
{
    public class AddPackageCommand : IRequest<ResultDto<bool>>
    {
        public AddPackageDto AddPackageRequest { get; set; }

        public AddPackageCommand(AddPackageDto addPackageRequest)
        {
            AddPackageRequest = addPackageRequest;
        }
    }
}
