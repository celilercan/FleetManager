using FleetManager.Common.Extension;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Vehicle;
using FleetManager.Common.Enums;
using FleetManager.Service.Mapping;
using System.Threading.Tasks;

namespace FleetManager.Service.Vehicle
{
    public class VehicleService : IVehicleService
    {
        private readonly IRedisProvider _redisProvider;
        
        public VehicleService(IRedisProvider redisProvider)
        {
            _redisProvider = redisProvider;
        }

        public async Task<ResultDto<bool>> AddAsync(AddVehicleDto dto)
        {
            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, dto.LicencePlate.ToKey());
            if (string.IsNullOrEmpty(dto.LicencePlate) || await _redisProvider.IsExistAsync(key))
            {
                return new ResultDto<bool>(ResultStatus.ValidationError);
            }

            var entity = DtoMapper.Mapper.Map<Data.Entities.Vehicle>(dto);
            await _redisProvider.SetAsync(key, entity);
            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public async Task<ResultDto<bool>> DeleteAsync(string licencePlate)
        {
            if (string.IsNullOrEmpty(licencePlate))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, licencePlate.ToKey());

            if (!await _redisProvider.IsExistAsync(key))
                return new ResultDto<bool>(ResultStatus.NotFound);

            await _redisProvider.RemoveAsync(key);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public async Task<ResultDto<VehicleDetailDto>> GetByLicencePlateAsync(string licencePlate)
        {
            var entity = await _redisProvider.GetAsync<Data.Entities.Vehicle>(string.Format(Constant.Vehicle.VehicleByPlateKey, licencePlate));
            if (entity == null)
                return new ResultDto<VehicleDetailDto>(ResultStatus.NotFound);

            var result = DtoMapper.Mapper.Map<VehicleDetailDto>(entity);

            return new ResultDto<VehicleDetailDto>(ResultStatus.Success, result);
        }

        public async Task<ResultDto<bool>> UpdateAsync(UpdateVehicleDto dto)
        {
            if (string.IsNullOrEmpty(dto.LicencePlate))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, dto.LicencePlate.ToKey());
            var entity = await _redisProvider.GetAsync<Data.Entities.Vehicle>(key);
            if (entity == null)
                return new ResultDto<bool>(ResultStatus.NotFound);

            entity = DtoMapper.Mapper.Map<Data.Entities.Vehicle>(dto);
            await _redisProvider.SetAsync(key, entity);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }
    }
}
