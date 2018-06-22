using System.Linq;
using Commons.Extensions;
using NUnit.Framework;

namespace Commons.UnitTests.Extensions
{
    [TestFixture]
    public class StringSearchExtensionsTest
    {
        [Test]
        public void AllIndicesOfFindsAllOccurences()
        {
            var input = "The fox chased the rabbit, while another fox chased an owl";
            var searchValue = "fox";
            var expected = new[] {4, 41};
            var actual = input.AllIndicesOf(searchValue);
            Assert.That(actual.Count, Is.EqualTo(expected.Length));
            Assert.That(actual.SequenceEqual(expected));
        }

        [Test]
        public void AllIndicesOfFindsSearchValueAtStart()
        {
            var input = "fox and dog";
            var searchValue = "fox";
            var expected = new[] {0};
            var actual = input.AllIndicesOf(searchValue);
            Assert.That(actual.Count, Is.EqualTo(expected.Length));
            Assert.That(actual.SequenceEqual(expected));
        }

        [Test]
        public void AllIndicesOfThrowsIfInputNull()
        {
            string input = null;
            var searchValue = "fox";
            Assert.That(() => StringSearchExtensions.AllIndicesOf(input, searchValue), Throws.ArgumentNullException);
        }

        [Test]
        public void AllIndicesOfReturnsEmptyListIfNoMatch()
        {
            var input = "The fox chased the rabbit, while another fox chased an owl";
            var searchValue = "dog";
            var actual = input.AllIndicesOf(searchValue);
            Assert.That(actual.Count, Is.EqualTo(0));
        }
    }
}
