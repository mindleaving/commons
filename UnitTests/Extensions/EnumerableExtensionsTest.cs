using System.Collections.Generic;
using Commons.Extensions;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void IsEquivalentTrueForSameList()
        {
            var list = new List<int> {1, 2, 4, 5};
            Assert.That(list.IsEquivalent(list), Is.True);
        }

        [Test]
        public void IsEquivalentTrueForListsWithSameElements()
        {
            var list1 = new List<int> {1, 2, 4, 5};
            var list2 = new List<int> {5, 2, 1, 4};
            Assert.That(list1.IsEquivalent(list2), Is.True);
            Assert.That(list2.IsEquivalent(list1), Is.True);
        }

        [Test]
        public void IsEquivalentTrueForListsWithMultipleInstancesOfSameValue()
        {
            var list1 = new List<int> {1, 2, 4, 5, 5, 5};
            var list2 = new List<int> {5, 2, 1, 5, 4, 5};
            Assert.That(list1.IsEquivalent(list2), Is.True);
            Assert.That(list2.IsEquivalent(list1), Is.True);
        }
    }
}
