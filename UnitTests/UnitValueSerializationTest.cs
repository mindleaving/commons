using System.IO;
using System.Runtime.Serialization;
using Commons.Physics;
using NUnit.Framework;

namespace Commons.UnitTests
{
    [TestFixture]
    public class UnitValueSerializationTest
    {
        [Test]
        public void SerializationRoundtrip()
        {
            var unitValue = new UnitValue(Unit.Second, 1.3);
            var serializer = new DataContractSerializer(typeof(UnitValue));

            UnitValue deserializedUnitValue;
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, unitValue);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedUnitValue = (UnitValue) serializer.ReadObject(stream);
            }
            Assert.That(deserializedUnitValue, Is.EqualTo(unitValue));
        }
    }
}
