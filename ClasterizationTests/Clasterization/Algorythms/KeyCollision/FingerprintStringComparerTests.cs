using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

// ReSharper disable CheckNamespace

namespace Clasterization.Clasterization.Algorythms.KeyCollision.Tests
{
    [TestClass()]
    public class FingerprintStringComparerTests
    {
        private readonly FingerprintStringComparer _comparer;

        public FingerprintStringComparerTests()
        {
            _comparer = new FingerprintStringComparer();
        }

        [TestMethod()]
        public void EqualsTest()
        {
            const string baseStr =
                "Some test sentence with UPPER and lower case words, punctuation and" +
                "and other thingth!!!";

            Assert.IsFalse(_comparer.Equals(baseStr, string.Empty));

            Assert.IsTrue(_comparer.Equals(
                baseStr.Clone() as string,
                baseStr));

            Assert.IsTrue(_comparer.Equals(
                "\n\t  " + baseStr + "      \n",
                baseStr));

            Assert.IsTrue(_comparer.Equals(
                baseStr.Substring(0, 5) + "  \n\n\n\t    " + baseStr.Substring(5),
                baseStr));

            Assert.IsTrue(_comparer.Equals(
                baseStr.Substring(0, 5) + ",;!'':'" + baseStr.Substring(5),
                baseStr));

            Assert.IsTrue(_comparer.Equals(
                baseStr.ToUpper(),
                baseStr.ToLower()));

            var random = new Random();


            Assert.IsTrue(_comparer.Equals(
                baseStr,
                string.Concat(baseStr
                    .Split(' ')
                    .OrderBy(str => random.Next(0, 1000))
                    .Select(x => x + ' '))));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(_comparer.
                GetHashCode("qwerrty"),
                "qwerrty".GetHashCode());
        }
    }
}