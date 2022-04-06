using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FleetManager.Test.ServiceTests
{
    [TestFixture]
    public class WrongDeliveryServiceTest
    {
        private readonly Mock<IRedisProvider> _redisProviderMock;

        public WrongDeliveryServiceTest()
        {
            _redisProviderMock = new Mock<IRedisProvider>();
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Empty_Key()
        {
            var _wrongDeliveryService = new WrongDeliveryService(_redisProviderMock.Object);
            var result = await _wrongDeliveryService.AddAsync(new AddWrongDeliveryDto());
            Assert.AreEqual(ResultStatus.ValidationError, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_With_Valid_Dto()
        {
            const string barcode = "P8988000121";
            const int deliveryPoint = 1;
            var key = string.Format(Constant.Shipment.WrongDeliveryByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var wrongDelivery = new AddWrongDeliveryDto { Barcode = barcode, DeliveryPointId = deliveryPoint };
            _redisProviderMock.Setup(x => x.SetAsync(key, wrongDelivery));
            var _wrongDeliveryService = new WrongDeliveryService(_redisProviderMock.Object);
            var result = await _wrongDeliveryService.AddAsync(wrongDelivery);
            Assert.AreEqual(ResultStatus.Success, result.Status);
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Duplicate_Key()
        {
            const string barcode = "P8988000121";
            const int deliveryPoint = 1;
            var key = string.Format(Constant.Shipment.WrongDeliveryByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var wrongDelivery = new AddWrongDeliveryDto { Barcode = barcode, DeliveryPointId = deliveryPoint };
            _redisProviderMock.Setup(x => x.SetAsync(key, wrongDelivery));
            var _wrongDeliveryService = new WrongDeliveryService(_redisProviderMock.Object);
            var result = await _wrongDeliveryService.AddAsync(wrongDelivery);

            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(true));
            var result2 = await _wrongDeliveryService.AddAsync(wrongDelivery);
            Assert.AreEqual(ResultStatus.ValidationError, result2.Status);
        }

        [Test]
        public async Task Should_Return_NotFound_With_NotExist_Key()
        {
            const string barcode = "P8988000121";
            var key = string.Format(Constant.Shipment.WrongDeliveryByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var _wrongDeliveryService = new WrongDeliveryService(_redisProviderMock.Object);
            var result = await _wrongDeliveryService.GetDetail(barcode);
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }
    }
}
