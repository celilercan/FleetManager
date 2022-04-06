using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Mapping;
using System.Threading.Tasks;

namespace FleetManager.Service.Shipment
{
    public class WrongDeliveryService : IWrongDeliveryService
    {
        private readonly IRedisProvider _redisProvider;

        public WrongDeliveryService(IRedisProvider redisProvider)
        {
            _redisProvider = redisProvider;
        }

        public async Task<ResultDto<bool>> AddAsync(AddWrongDeliveryDto dto)
        {
            var key = string.Format(Constant.Shipment.WrongDeliveryByBarcodeKey, dto.Barcode);
            if (string.IsNullOrEmpty(dto.Barcode) || await _redisProvider.IsExistAsync(key))
                return new ResultDto<bool>(ResultStatus.ValidationError);

            var wrongDelivery = DtoMapper.Mapper.Map<Data.Entities.WrongDelivery>(dto);

            await _redisProvider.SetAsync(key, wrongDelivery);

            return new ResultDto<bool>(ResultStatus.Success, true);
        }

        public async Task<ResultDto<WrongDeliveryDetailDto>> GetDetail(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return new ResultDto<WrongDeliveryDetailDto>(ResultStatus.ValidationError);

            var key = string.Format(Constant.Shipment.WrongDeliveryByBarcodeKey, barcode);
            var wrongDelivery = await _redisProvider.GetAsync<Data.Entities.WrongDelivery>(key);
            if(wrongDelivery == null)
                return new ResultDto<WrongDeliveryDetailDto>(ResultStatus.NotFound);

            var result = DtoMapper.Mapper.Map<WrongDeliveryDetailDto>(wrongDelivery);

            return new ResultDto<WrongDeliveryDetailDto>(ResultStatus.Success, result);
        }
    }
}
