using System.IO;
using System.Runtime.Serialization;
using Commons.IO;
using Commons.Physics;
using NUnit.Framework;

namespace Commons.UnitTests
{
    [TestFixture]
    public class UnitValueSerializationTest
    {
        [Test]
        [TestCase(1.3, Unit.Second)]
        [TestCase(0.013, Unit.Second)]
        [TestCase(1030, Unit.Second)]
        [TestCase(1030, Unit.Kelvin)]
        public void SerializationRoundtripSimpleUnit(double value, Unit unit)
        {
            var unitValue = new UnitValue(unit, value);
            var serializer = new Serializer<UnitValue>();

            UnitValue deserializedUnitValue;
            using (var stream = new MemoryStream())
            {
                serializer.Store(unitValue, stream);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedUnitValue = serializer.Load(stream);
            }
            Assert.That(deserializedUnitValue, Is.EqualTo(unitValue));
        }

        [Test]
        public void CompoundUnitStringAsSpecified()
        {
            var unitValue = new UnitValue(new CompoundUnit(
                new []{ SIBaseUnit.Kilogram, SIBaseUnit.Meter}, 
                new []{SIBaseUnit.Second, SIBaseUnit.Kelvin, SIBaseUnit.Kelvin}), 
                0.034);
            var unitString = unitValue.Unit.ToString();
            var expectedUnitString = "m kg/(s K^2)";
            Assert.That(unitString, Is.EqualTo(expectedUnitString));

            var unitValueString = unitValue.ToString();
            var expctedUnitValueString = $"0.034 {expectedUnitString}";
            Assert.That(unitValueString, Is.EqualTo(expctedUnitValueString));
        }

        [Test]
        public void CompoundUnitDenominatorOnlyStringAsSpecified()
        {
            var unitValue = new UnitValue(new CompoundUnit(
                    new SIBaseUnit[]{ }, 
                    new []{SIBaseUnit.Second, SIBaseUnit.Kelvin, SIBaseUnit.Kelvin}), 
                0.034);
            var unitString = unitValue.Unit.ToString();
            var expectedUnitString = "1/(s K^2)";
            Assert.That(unitString, Is.EqualTo(expectedUnitString));

            var unitValueString = unitValue.ToString();
            var expctedUnitValueString = $"0.034 {expectedUnitString}";
            Assert.That(unitValueString, Is.EqualTo(expctedUnitValueString));
        }

        [Test]
        public void SerializationRoundtripCompoundUnit()
        {
            var unitValue = new UnitValue(new CompoundUnit(new []{ SIBaseUnit.Kilogram, SIBaseUnit.Meter}, new []{SIBaseUnit.Second, SIBaseUnit.Kelvin}), 1.3);
            var serializer = new Serializer<UnitValue>();

            using (var stream = new MemoryStream())
            {
                serializer.Store(unitValue, stream);
                stream.Seek(0, SeekOrigin.Begin);
                var deserializedUnitValue = serializer.Load(stream);
                Assert.That(deserializedUnitValue, Is.EqualTo(unitValue));
            }
        }
    }
}
