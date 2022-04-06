using AutoMapper;
using FleetManager.Dto.DeliveryPoint;
using FleetManager.Dto.Shipment;
using FleetManager.Dto.Vehicle;

namespace FleetManager.Service.Mapping
{
    public class ServiceMapping : Profile
    {
        public ServiceMapping()
        {
            CreateMap<Data.Entities.Vehicle, VehicleDetailDto>().ReverseMap();
            CreateMap<AddVehicleDto, Data.Entities.Vehicle>().ReverseMap();
            CreateMap<UpdateVehicleDto, Data.Entities.Vehicle>().ReverseMap();

            CreateMap<Data.Entities.DeliveryPoint, DeliveryPointDetailDto>().ReverseMap();
            CreateMap<AddDeliveryPointDto, Data.Entities.DeliveryPoint>().ReverseMap();
            CreateMap<UpdateDeliveryPointDto, Data.Entities.DeliveryPoint>().ReverseMap();

            CreateMap<AddBagDto, Data.Entities.Shipment>().ReverseMap();
            CreateMap<AddPackageDto, Data.Entities.Shipment>().ReverseMap();
            CreateMap<AddBagDto, Data.Entities.Bag>().ReverseMap();
            CreateMap<AddPackageDto, Data.Entities.Package>().ReverseMap();
            CreateMap<AddWrongDeliveryDto, Data.Entities.WrongDelivery>().ReverseMap();
            CreateMap<Data.Entities.Bag, ShipmentDetailDto>()
                .ForMember(x => x.ShipmentState, x => x.MapFrom(s => (int)s.BagStatus))
                .ReverseMap();
            CreateMap<Data.Entities.Package, ShipmentDetailDto>()
                .ForMember(x => x.ShipmentState, x => x.MapFrom(s => (int)s.PackageStatus))
                .ReverseMap();
            CreateMap<Data.Entities.WrongDelivery, WrongDeliveryDetailDto>().ReverseMap();
        }
    }

}
