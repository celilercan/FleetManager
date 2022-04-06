using FleetManager.Common.Extension;
using NUnit.Framework;

namespace FleetManager.Test.CommonTests
{
    [TestFixture]
    public class StringExtensionTest
    {
        [Test]
        public void Should_Return_String_Empty_ToKey_When_Empty_Input()
        {
            var str = string.Empty;
            var result = str.ToKey();
            Assert.AreEqual(result, string.Empty);
        }

        [Test]
        public void Should_NotContains_Space_ToKey_When_Valid_Input()
        {
            const string str = "34 TL 34";
            var result = str.ToKey();
            Assert.IsTrue(!result.Contains(" "));
        }
    }
}
