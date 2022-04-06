using FleetManager.Common.Constants;
using FleetManager.Common.Enums;
using FleetManager.Common.Extension;
using NUnit.Framework;

namespace FleetManager.Test.CommonTests
{
    [TestFixture]
    public class GenericExtensionTests
    {
        [Test]
        public void Should_Return_EnumText_When_Have_Not_Message_Attr()
        {
            var enu = ShipmentType.Package;
            var result = enu.GetMessage();
            Assert.AreEqual(result, enu.ToString());
        }

        [Test]
        public void Should_Return_SuccessMessage_When_ResultStatus_Success()
        {
            var enu = ResultStatus.Success;
            var result = enu.GetMessage();
            Assert.AreEqual(result, MessageConstant.Success);
        }
    }
}
