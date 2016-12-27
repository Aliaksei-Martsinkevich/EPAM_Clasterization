using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;
//TDD
// ReSharper disable once CheckNamespace
namespace Clasterization.Clasterization.Algorythms.Tests
{
    [TestClass()]
    public class StringOperationsExtensionsTests
    {
        [TestMethod()]
        public void ShouldRemovePunctuation()
        {
            string.Empty.RemovePunctuation();

            var builder = new StringBuilder();
            for (var c = char.MinValue; c < char.MaxValue; c++)
                builder.Append(c);

            var withoutPunctuation = builder.ToString().RemovePunctuation();

            Assert.IsFalse(withoutPunctuation.Any(char.IsPunctuation));

            Assert.AreEqual(withoutPunctuation,
                            withoutPunctuation.RemovePunctuation());
        }

        [TestMethod()]
        public void SplitToNGrammsTest()
        {
            Assert.IsTrue(!"".SplitToNGramms().Any());
            const string str = "Some string, that going to be separated";

            var twoGrams = str.SplitToNGramms(2).ToArray();
            Assert.AreEqual(twoGrams.Count(), str.Length - 1);

            for (var i = 0; i < str.Length - 1; i++)
            {
                Assert.AreEqual(str.Substring(i, 2), twoGrams.ElementAt(i));
            }

            var threeGrams = str.SplitToNGramms(3).ToArray();
            Assert.AreEqual(threeGrams.Count(), str.Length - 2);

            for (var i = 0; i < str.Length - 2; i++)
            {
                Assert.AreEqual(str.Substring(i, 3), threeGrams.ElementAt(i));
            }
        }

        [TestMethod()]
        public void RemoveLongWhiteSpacesTest()
        {
            string.Empty.RemoveLongWhiteSpaces();

            var builder = new StringBuilder();
            for (var c = char.MinValue; c < char.MaxValue; c++)
                builder.Append(c);

            var noLongWidespaces = builder.ToString().RemoveLongWhiteSpaces();

            Assert.IsFalse(ContainsLongWidespace(noLongWidespaces));
            Assert.AreEqual(noLongWidespaces, noLongWidespaces.RemoveLongWhiteSpaces());
        }

        private static bool ContainsLongWidespace(string str)
        {
            for (var i = 0; i < str.Length - 1; i++)
            {
                if (!char.IsWhiteSpace(str.ElementAt(i))) continue;
                if (char.IsWhiteSpace(str.ElementAt(i + 1)))
                    return true;
            }
            return false;
        }
    }
}