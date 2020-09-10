// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace StringProcessors.Tests
{
    [TestFixture]
    public class BinaryStringProcessorTests
    {
        [Test]
        public void Goodness_GoodEnoughString_ReturnsTrue()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsTrue(stringProc.IsGood("110010"), "Binary string processor goes mad!");
            Assert.IsTrue(stringProc.IsGood("1010101110010010110111001011001000"), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_BadString_ReturnsFalse()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsFalse(stringProc.IsGood("1100101"), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_BadPrefixInGoodString_ReturnsFalse()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsFalse(stringProc.IsGood("11001001"), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_BadPrefixInBadString_ReturnsFalse()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsFalse(stringProc.IsGood("110010011"), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_WrongCharactersInString_ReturnsFalse()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsFalse(stringProc.IsGood("11ABC0010011"), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_EmptyString_ReturnsTrue()
        {
            // Not sure is an empty string good binary string or not, behavior could be changed later on.
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.IsTrue(stringProc.IsGood(""), "Binary string processor goes mad!");
        }

        [Test]
        public void Goodness_NullString_ThrowsException()
        {
            BinaryStringProcessor stringProc = new BinaryStringProcessor();
            Assert.Throws<ArgumentNullException>(() => stringProc.IsGood(null), "Binary string processor goes mad!");
        }
    }
}
