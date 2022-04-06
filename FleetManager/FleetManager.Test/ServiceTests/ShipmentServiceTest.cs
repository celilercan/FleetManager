using FleetManager.Common.Enums;
using FleetManager.Data.Constants;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Shipment;
using FleetManager.Service.Shipment;
using FleetManager.Service.Shipment.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FleetManager.Test.ServiceTests
{
    [TestFixture]
    public class ShipmentServiceTest
    {
        private readonly Mock<IRedisProvider> _redisProviderMock;
        private readonly Mock<ServiceResolver> _serviceResolver;
        private readonly Mock<ILogger<BagService>> _bagloggerMock;
        private readonly Mock<ILogger<PackageService>> _packageloggerMock;
        private readonly Mock<IWrongDeliveryService> _wrongDeliveryServiceMock;

        public ShipmentServiceTest()
        {
            _redisProviderMock = new Mock<IRedisProvider>();
            _serviceResolver = new Mock<ServiceResolver>();
            _bagloggerMock = new Mock<ILogger<BagService>>();
            _packageloggerMock = new Mock<ILogger<PackageService>>();
            _wrongDeliveryServiceMock = new Mock<IWrongDeliveryService>();
        }

        [Test]
        public async Task Should_Return_ValidationError_AddBag_With_Empty_Key()
        {
            var _addBagHandler = new AddBagCommandHandler(_redisProviderMock.Object);
            var addBagRequest = new AddBagDto();
            var result = await _addBagHandler.Handle(new Service.Shipment.Commands.AddBagCommand(addBagRequest), System.Threading.CancellationToken.None);
            Assert.AreEqual(ResultStatus.ValidationError, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_AddBag_With_Valid_Dto()
        {
            const string barcode = "C725799";
            var key = string.Format(Constant.Shipment.BagByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var addBagRequest = new AddBagDto { Barcode = barcode, DeliveryPointId = 1 };
            _redisProviderMock.Setup(x => x.SetAsync(key, addBagRequest));
            var _addBagHandler = new AddBagCommandHandler(_redisProviderMock.Object);
           
            var result = await _addBagHandler.Handle(new Service.Shipment.Commands.AddBagCommand(addBagRequest), System.Threading.CancellationToken.None);

            Assert.AreEqual(ResultStatus.Success, result.Status);
        }

        [Test]
        public async Task Should_Return_ValidationError_With_Duplicate_Key()
        {
            const string barcode = "C725799";
            var key = string.Format(Constant.Shipment.ShipmentByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var addBagRequest = new AddBagDto { Barcode = barcode, DeliveryPointId = 1 };
            _redisProviderMock.Setup(x => x.SetAsync(key, addBagRequest));
            var _addBagHandler = new AddBagCommandHandler(_redisProviderMock.Object);
            var result = await _addBagHandler.Handle(new Service.Shipment.Commands.AddBagCommand(addBagRequest), System.Threading.CancellationToken.None);

            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(true));
            var result2 = await _addBagHandler.Handle(new Service.Shipment.Commands.AddBagCommand(addBagRequest), System.Threading.CancellationToken.None);
            Assert.AreEqual(ResultStatus.ValidationError, result2.Status);
        }

        [Test]
        public async Task Should_Return_NotFound_With_NotExist_Key()
        {
            const string barcode = "C725799";
            var key = string.Format(Constant.Shipment.ShipmentByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            _serviceResolver.Setup(x => x(ShipmentType.Bag)).Returns(new BagService(_redisProviderMock.Object, _bagloggerMock.Object, _wrongDeliveryServiceMock.Object));
            var _getShipmentHandler = new GetShipmentDetailByBarcodeHandler(_redisProviderMock.Object, _serviceResolver.Object);
            var result = await _getShipmentHandler.Handle(new Service.Shipment.Queries.GetShipmentDetailByBarcodeQuery(barcode), System.Threading.CancellationToken.None);
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }

        [Test]
        public async Task Should_Return_ValidationError_AddPackage_With_Empty_Key()
        {
            var _addPackageHandler = new AddPackageCommandHandler(_redisProviderMock.Object);
            var addPackageRequest = new AddPackageDto();
            var result = await _addPackageHandler.Handle(new Service.Shipment.Commands.AddPackageCommand(addPackageRequest), System.Threading.CancellationToken.None);
            Assert.AreEqual(ResultStatus.ValidationError, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_AddPackage_With_Valid_Dto()
        {
            const string barcode = "P9988000126";
            var key = string.Format(Constant.Shipment.PackageByBarcodeKey, barcode);
            _redisProviderMock.Setup(x => x.IsExistAsync(key)).Returns(Task.FromResult(false));

            var addPackageRequest = new AddPackageDto { Barcode = barcode, DeliveryPointId = 1, Weight = 5 };
            _redisProviderMock.Setup(x => x.SetAsync(key, addPackageRequest));
            var _addBagHandler = new AddPackageCommandHandler(_redisProviderMock.Object);

            var result = await _addBagHandler.Handle(new Service.Shipment.Commands.AddPackageCommand(addPackageRequest), System.Threading.CancellationToken.None);

            Assert.AreEqual(ResultStatus.Success, result.Status);
        }

        [Test]
        public async Task Should_Return_Success_Delivery_With_Valid_Dto()
        {
            const string barcode = "P9988000126";
            var shipmentKey = string.Format(Constant.Shipment.ShipmentByBarcodeKey, barcode);
            var shipment = new Data.Entities.Shipment { DeliveryPointId = 1, Barcode = barcode, ShipmentState = 1, ShipmentType = ShipmentType.Package };
            _redisProviderMock.Setup(x => x.GetAsync<Data.Entities.Shipment>(shipmentKey)).Returns(Task.FromResult(shipment));

            var key = string.Format(Constant.Shipment.PackageByBarcodeKey, barcode);
            var package = new Data.Entities.Package { Barcode = barcode, DeliveryPointId = 1, Weight = 5 };
            var deliveryRequest = new ShipmentDeliveryRequestDto { Barcode = barcode, DeliveryPointId = 1 };

            _redisProviderMock.Setup(x => x.GetAsync<Data.Entities.Package>(key)).Returns(Task.FromResult(package));
            _serviceResolver.Setup(x => x(ShipmentType.Package)).Returns(new PackageService(_redisProviderMock.Object, _packageloggerMock.Object, _wrongDeliveryServiceMock.Object));
            var _deliveryHandler = new ShipmentDeliveryCommandHandler(_redisProviderMock.Object, _serviceResolver.Object);

            var result = await _deliveryHandler.Handle(new Service.Shipment.Commands.ShipmentDeliveryRequestCommand(deliveryRequest), System.Threading.CancellationToken.None);

            Assert.AreEqual(ResultStatus.Success, result.Status);
        }
    }
}
