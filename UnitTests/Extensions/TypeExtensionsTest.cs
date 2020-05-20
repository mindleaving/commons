using System;
using System.Drawing;
using Commons.Extensions;
using Commons.Misc;
using Commons.Optimization;
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

        [Test]
        public void AsReturnsDefaultIfNonMatchingType()
        {
            Assert.That(new UnitValue(CompoundUnits.Kilogram, 10).As<ParameterSetting>(), Is.EqualTo(default(ParameterSetting)));
            Assert.That(new UnitValue(CompoundUnits.Kilogram, 10).As<Annotation>(), Is.EqualTo(default(Annotation)));
            Assert.That(5.As<Annotation>(), Is.EqualTo(default(Annotation)));
        }

        [Test]
        public void AsReturnsObjectIfSameType()
        {
            var unitValue = new UnitValue(CompoundUnits.Kilogram, 10);
            Assert.That(unitValue.As<UnitValue>(), Is.EqualTo(unitValue));
        }

        [Test]
        public void AsReturnsCastedValueIfMatchingType()
        {
            var unitValue = new UnitValue(CompoundUnits.Kilogram, 10);
            Assert.That(unitValue.As<object>(), Is.InstanceOf<object>());
            Assert.That(unitValue.As<object>(), Is.EqualTo(unitValue));
        }

        [Test]
        public void CastThrowsExceptionIfNonMatchingType()
        {
            Assert.That(() => new UnitValue(CompoundUnits.Kilogram, 10).Cast<ParameterSetting>(), Throws.TypeOf<InvalidCastException>());
            Assert.That(() => new UnitValue(CompoundUnits.Kilogram, 10).Cast<Annotation>(), Throws.TypeOf<InvalidCastException>());
            Assert.That(() => 5.Cast<Annotation>(), Throws.TypeOf<InvalidCastException>());
        }

        [Test]
        public void CastReturnsCastedValueIfMatchingType()
        {
            var unitValue = new UnitValue(CompoundUnits.Kilogram, 10);
            Assert.That(unitValue.Cast<object>(), Is.InstanceOf<object>());
            Assert.That(unitValue.Cast<object>(), Is.EqualTo(unitValue));
        }
    }
}
