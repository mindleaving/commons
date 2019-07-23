using System.Drawing;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        private static readonly object[] IsNumberTestCases = {
            (int) 1, (int) -1, (double) 1.3d, (float) 2.4f, (byte) 0x45,
        };

        [TestCaseSource(nameof(IsNumberTestCases))]
        public void IsNumber(object o)
        {
            Assert.That(o.IsNumber(), Is.True);
        }

        private static readonly object[] IsNotANumberTestCases =
        {
            null, new UnitValue(Unit.Liter, 2.3), Color.Gray, "3.2", "1"
        };

        [TestCaseSource(nameof(IsNotANumberTestCases))]
        public void IsNotANumber(object o)
        {
            Assert.That(o.IsNumber(), Is.False);
        }
    }
}
