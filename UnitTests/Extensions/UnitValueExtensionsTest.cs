using System;
using System.Linq;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class UnitValueExtensionsTest
    {
        [Test]
        public void AllUnitsHaveStringRepresentation()
        {
            var units = EnumExtensions.GetValues<Unit>().Except(new[] {Unit.Compound});
            foreach (var unit in units)
            {
                Assert.That(() => unit.StringRepresentation(), Throws.Nothing);
            }
        }
    }
}
