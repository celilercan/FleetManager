using FleetManager.Data.Infrastructure;
using FleetManager.Common.Enums;
using FleetManager.Dto.Vehicle;
using FleetManager.Service.Vehicle;
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using FleetManager.Data.Constants;
using FleetManager.Common.Extension;

namespace FleetManager.Test.ServiceTests
{
    [TestFixture]
    public class VehicleServiceTest
    {
        private readonly Mock<IRedisProvider> _redisProviderMock;

        public VehicleServiceTest()
        {
            _redisProviderMock = new Mock<IRedisProvider>();
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Empty_Key()
        {
            var _vehicleService = new VehicleService(_redisProviderMock.Object);
            var result = await _vehicleService.AddAsync(new AddVehicleDto());
            Assert.AreEqual(ResultStatus.ValidationError, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_With_Valid_Dto()
        {
            const string plate = "34 TL 34";
            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, plate.ToKey());
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var vehicle = new AddVehicleDto { LicencePlate = plate };
            _redisProviderMock.Setup(x => x.SetAsync(key, vehicle));
            var _vehicleService = new VehicleService(_redisProviderMock.Object);
            var result = await _vehicleService.AddAsync(vehicle);
            Assert.AreEqual(ResultStatus.Success, result.Status);
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Duplicate_Key()
        {
            const string plate = "34 TL 34";
            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, plate.ToKey());
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var vehicle = new AddVehicleDto { LicencePlate = plate };
            _redisProviderMock.Setup(x => x.SetAsync(key, vehicle));
            var _vehicleService = new VehicleService(_redisProviderMock.Object);
            var result = await _vehicleService.AddAsync(vehicle);

            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(true));
            var result2 = await _vehicleService.AddAsync(vehicle);
            Assert.AreEqual(ResultStatus.ValidationError, result2.Status);
        }

        [Test]
        public async Task Should_Return_NotFound_With_NotExist_Key()
        {
            const string plate = "34 TL 34";
            var key = string.Format(Constant.Vehicle.VehicleByPlateKey, plate.ToKey());
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var vehicle = new UpdateVehicleDto { LicencePlate = plate };
            var _vehicleService = new VehicleService(_redisProviderMock.Object);
            var result = await _vehicleService.UpdateAsync(vehicle);
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }

    }
}
