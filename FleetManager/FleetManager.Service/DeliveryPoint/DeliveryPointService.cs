using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.DeliveryPoint;
using FleetManager.Common.Enums;
using FleetManager.Service.Mapping;
using System.Threading.Tasks;

namespace FleetManager.Service.DeliveryPoint
{
    public class DeliveryPointService : IDeliveryPointService
    {
        private readonly IRedisProvider _redisProvider;

        public DeliveryPointService(IRedisProvider redisProvider)
        {
            _redisProvider = redisProvider;
        }

        public async Task<ResultDto<bool>> AddAsync(AddDeliveryPointDto dto)
        {
            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, dto.Index);
            if (string.IsNullOrEmpty(dto.Name) || await _redisProvider.IsExistAsync(key))
            {
                return new ResultDto<bool>(ResultStatus.ValidationError);
            }

            var entity = DtoMapper.Mapper.Map<Data.Entities.DeliveryPoint>(dto);
            await _redisProvider.SetAsync(key, entity);
            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public async Task<ResultDto<bool>> DeleteAsync(int index)
        {
            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, index);

            if (!await _redisProvider.IsExistAsync(key))
                return new ResultDto<bool>(ResultStatus.NotFound);

            await _redisProvider.RemoveAsync(key);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public async Task<ResultDto<DeliveryPointDetailDto>> GetByIndexAsync(int index)
        {
            var entity = await _redisProvider.GetAsync<Data.Entities.DeliveryPoint>(string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, index));
            if (entity == null)
                return new ResultDto<DeliveryPointDetailDto>(ResultStatus.NotFound);

            var result = DtoMapper.Mapper.Map<DeliveryPointDetailDto>(entity);

            return new ResultDto<DeliveryPointDetailDto>(ResultStatus.Success, result);
        }

        public async Task<ResultDto<bool>> UpdateAsync(UpdateDeliveryPointDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, dto.Index);
            var entity = await _redisProvider.GetAsync<Data.Entities.DeliveryPoint>(key);
            if (entity == null)
                return new ResultDto<bool>(ResultStatus.NotFound);

            entity = DtoMapper.Mapper.Map<Data.Entities.DeliveryPoint>(dto);
            await _redisProvider.SetAsync(key, entity);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }
    }
}
