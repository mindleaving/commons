using Commons.Extensions;
using Commons.IO;
using Commons.Mathematics;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest
{
    [TestFixture]
    public class DeepClonerTest
    {
        [Test]
        public void CanCloneTestClass()
        {
            var testObject = new DeepClonerTestObject
            {
                Name = "Joe",
                Weight = 79.To(Unit.Kilogram),
                BMI = 24.8,
                Position = new Vector(4, -3.5, 58.4)
            };
            DeepClonerTestObject clone = null;
            Assert.That(() => clone = DeepCloner<DeepClonerTestObject>.Clone(testObject), Throws.Nothing);
            Assert.That(clone.Name, Is.EqualTo(testObject.Name));
            Assert.That(clone.Weight.In(Unit.Kilogram), Is.EqualTo(testObject.Weight.In(Unit.Kilogram)).Within(1e-5));
            Assert.That(clone.BMI, Is.EqualTo(testObject.BMI).Within(1e-5));
        }

        private class DeepClonerTestObject
        {
            public string Name { get; set; }
            public UnitValue Weight { get; set; }
            public double BMI { get; set; }
            public Vector Position { get; set; }
        }
    }
}
