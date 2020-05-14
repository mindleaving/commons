using System;
using System.IO;
using System.Linq;
using Commons.Extensions;
using Commons.IO;
using Commons.Mathematics;
using Commons.Physics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitValueSerializationTest
    {
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void NullOrEmptyIsDeserializedToNull(string str)
        {
            UnitValue unitValue = null;
            Assert.That(() => unitValue = UnitValue.Parse(str), Throws.Nothing);
            Assert.That(unitValue, Is.Null);
        }


        private static object[] SerializationRoundtripSimpleUnitTestCases =
        {
            new object[] {1.3, Unit.Second},
            new object[] {0.013, Unit.Second},
            new object[] {1030, Unit.Second},
            new object[] {1030, Unit.Kelvin}
        };
        [Test]
        [TestCaseSource(nameof(SerializationRoundtripSimpleUnitTestCases))]
        public void SerializationRoundtripSimpleUnit(double value, IUnitDefinition unit)
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

        private static object[] SerializationRoundtripDoubleSpecialValuesTestCases =
        {
            new object[] {double.PositiveInfinity, Unit.Second},
            new object[] {double.NegativeInfinity, Unit.Second},
            new object[] {double.NaN, Unit.Second}
        };
        [Test]
        [TestCaseSource(nameof(SerializationRoundtripDoubleSpecialValuesTestCases))]
        public void SerializationRoundtripDoubleSpecialValues(double value, IUnitDefinition unit)
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
            var expectedUnitString = "m kg/(s °K^2)";
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
            var expectedUnitString = "1/(s °K^2)";
            Assert.That(unitString, Is.EqualTo(expectedUnitString));

            var unitValueString = unitValue.ToString();
            var expctedUnitValueString = $"0.034 {expectedUnitString}";
            Assert.That(unitValueString, Is.EqualTo(expctedUnitValueString));
        }

        [Test]
        public void SerializationRoundtripCompoundUnit()
        {
            var unitValue = new UnitValue(new CompoundUnit(new []{ SIBaseUnit.Kilogram, SIBaseUnit.Meter}, new []{SIBaseUnit.Second, SIBaseUnit.Kelvin}), 1.3);
            var serializedUnitValue = unitValue.ToString();
            var roundTripUnitValue = UnitValue.Parse(serializedUnitValue);
            Assert.That(roundTripUnitValue.Unit, Is.EqualTo(unitValue.Unit));
            Assert.That(roundTripUnitValue.Value, Is.EqualTo(unitValue.Value).Within(1e-3*unitValue.Value));
        }

        [Test]
        public void SerializationRoundtripBatchTest()
        {
            var allValidUnits = Unit.Effective.AllUnits;
            var exponentsToTest = SequenceGeneration.FixedStep(-12, 12, 3).ToList();
            foreach (var unit in allValidUnits)
            {
                foreach (var exponent in exponentsToTest)
                {
                    var value = 1.3 * Math.Pow(10, (int)exponent);
                    var unitValue = new UnitValue(unit, value);
                    var unitValueString = unitValue.ToString();
                    var roundTripUnitValue = UnitValue.Parse(unitValueString);
                    Assert.That(roundTripUnitValue.Unit, Is.EqualTo(unitValue.Unit));
                    Assert.That(roundTripUnitValue.Value, Is.EqualTo(unitValue.Value).Within(1e-3*unitValue.Value));
                }
            }
        }

        [Test]
        [TestCase("1.1.25", "{\"Name\":\"Test\",\"Value\":{\"StringValue\":\"11.3 mm\"}}")]
        [TestCase("1.1.26", "{\"Name\":\"Test\",\"Value\":\"11.3 mm\"}")]
        public void DeserializationIsBackwardCompatible(string version, string json)
        {
            // Generate
            //var json = JsonConvert.SerializeObject(new ClassWithUnitValue("Test", 11.3.To(SIPrefix.Milli, Units.Meter)));
            //File.WriteAllText($@"C:\Temp\{nameof(ClassWithUnitValue)}.json", json);

            ClassWithUnitValue reconstructed = null;
            Assert.That(
                () => reconstructed = JsonConvert.DeserializeObject<ClassWithUnitValue>(json),
                Throws.Nothing,
                $"Failing version: {version}");
            Assert.That(reconstructed.Name, Is.EqualTo("Test"));
            Assert.That(reconstructed.Value.In(SIPrefix.Milli, Unit.Meter), Is.EqualTo(11.3).Within(1e-6));
        }

        [Test]
        [TestCase("1.1.25", "{\"Name\":\"Test\",\"Value\":{\"StringValue\":null}}")]
        [TestCase("1.1.26", "{\"Name\":\"Test\",\"Value\": null }")]
        public void DeserializationOfNullIsBackwardCompatible(string version, string json)
        {
            ClassWithUnitValue reconstructed = null;
            Assert.That(
                () => reconstructed = JsonConvert.DeserializeObject<ClassWithUnitValue>(json),
                Throws.Nothing,
                $"Failing version: {version}");
            Assert.That(reconstructed.Name, Is.EqualTo("Test"));
            Assert.That(reconstructed.Value, Is.Null);
        }

        [Test]
        public void CanDeserializeMathjsUnit()
        {
            const string MathjsUnitJson = "{ fixPrefix: false, mathjs: \"Unit\", unit: \"kg / m^3\", value: 34 }";
            UnitValue unitValue = null;
            Assert.That(() => unitValue = JsonConvert.DeserializeObject<UnitValue>(MathjsUnitJson), Throws.Nothing);
            Assert.That(unitValue.Value, Is.EqualTo(34));
            Assert.That(unitValue.Unit, Is.EqualTo(CompoundUnits.Kilogram / CompoundUnits.CubicMeters));
        }

        private class ClassWithUnitValue
        {
            [JsonConstructor]
            public ClassWithUnitValue(string name, UnitValue value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }
            public UnitValue Value { get; }
        }
    }
}
