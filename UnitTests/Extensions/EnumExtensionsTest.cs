using Commons.Extensions;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        [Test]
        public void ToEnumParsesEnum()
        {
            var input = "Value1";
            Assert.That(input.ToEnum<TestEnum>(), Is.EqualTo(TestEnum.Value1));
        }

        [Test]
        public void ToEnumThrowsExceptionIfValueDoesntExist()
        {
            var input = "NotExisting";
            Assert.That(() => input.ToEnum<TestEnum>(), Throws.ArgumentException);
        }

        private enum TestEnum
        {
            Undefined = 0,
            Value1,
            Value2
        }
    }
}
