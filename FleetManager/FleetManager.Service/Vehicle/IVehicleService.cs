using FleetManager.Dto.Common;
using FleetManager.Dto.Vehicle;
using System;
using System.Threading.Tasks;

namespace FleetManager.Service.Vehicle
{
    public interface IVehicleService
    {
        Task<ResultDto<bool>> AddAsync(AddVehicleDto dto);

        Task<ResultDto<bool>> UpdateAsync(UpdateVehicleDto dto);

        Task<ResultDto<VehicleDetailDto>> GetByLicencePlateAsync(string licencePlate);

        Task<ResultDto<bool>> DeleteAsync(string licencePlate);
    }
}
