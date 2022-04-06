using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using MediatR;

namespace FleetManager.Service.Shipment.Commands
{
    public class AddBagCommand : IRequest<ResultDto<bool>>
    {
        public AddBagDto AddBagRequest { get; set; }

        public AddBagCommand(AddBagDto addBagRequest)
        {
            AddBagRequest = addBagRequest;
        }
    }
}
