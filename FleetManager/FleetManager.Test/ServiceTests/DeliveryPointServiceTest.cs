using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.DeliveryPoint;
using FleetManager.Service.DeliveryPoint;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FleetManager.Test.ServiceTests
{
    [TestFixture]
    public class DeliveryPointServiceTest
    {
        private readonly Mock<IRedisProvider> _redisProviderMock;

        public DeliveryPointServiceTest()
        {
            _redisProviderMock = new Mock<IRedisProvider>();
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Empty_Key()
        {
            var _deliveryPointService = new DeliveryPointService(_redisProviderMock.Object);
            var result = await _deliveryPointService.AddAsync(new AddDeliveryPointDto());
            Assert.AreEqual(ResultStatus.ValidationError, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_With_Valid_Dto()
        {
            const string index = "1";
            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, index);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var deliveryPoint = new AddDeliveryPointDto { Index = 1, Name = "Branch" };
            _redisProviderMock.Setup(x => x.SetAsync(key, deliveryPoint));
            var _deliveryPointService = new DeliveryPointService(_redisProviderMock.Object);
            var result = await _deliveryPointService.AddAsync(deliveryPoint);
            Assert.AreEqual(ResultStatus.Success, result.Status);
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Duplicate_Key()
        {
            const string index = "1";
            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, index);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var deliveryPoint = new AddDeliveryPointDto { Index = 1, Name = "Branch" };
            _redisProviderMock.Setup(x => x.SetAsync(key, deliveryPoint));
            var _deliveryPointService = new DeliveryPointService(_redisProviderMock.Object);
            var result = await _deliveryPointService.AddAsync(deliveryPoint);

            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(true));
            var result2 = await _deliveryPointService.AddAsync(deliveryPoint);
            Assert.AreEqual(ResultStatus.ValidationError, result2.Status);
        }

        [Test]
        public async Task Should_Return_NotFound_With_NotExist_Key()
        {
            const string index = "1";
            var key = string.Format(Constant.DeliveryPoint.DeliveryPointByIndexKey, index);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var vehicle = new UpdateDeliveryPointDto { Index = 1, Name = "Branch" };
            var _deliveryPointService = new DeliveryPointService(_redisProviderMock.Object);
            var result = await _deliveryPointService.UpdateAsync(vehicle);
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }
    }
}
