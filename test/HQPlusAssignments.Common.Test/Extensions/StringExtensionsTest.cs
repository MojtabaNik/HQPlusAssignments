using HQPlusAssignments.Common.Extensions;
using NUnit.Framework;

namespace HQPlusAssignments.Common.Test.Extensions
{
    public class StringExtensionsTest
    {
        [Test]
        public void ExtractNumbers_ValidInput_Test()
        {
            var result = "Test123".ExtractNumbers();
            Assert.AreEqual(123, result);
        }

        [Test]
        public void ExtractNumbers_EmptyInput_Test()
        {
            var result = "".ExtractNumbers();
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ExtractNumbers_InvalidInput_Test()
        {
            var result = "Test".ExtractNumbers();
            Assert.AreEqual(0, result);
        }
    }
}
