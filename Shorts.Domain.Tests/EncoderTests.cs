using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shorts.Domain;

namespace Shorts.Domain.Tests
{
    [TestClass]
    public class EncoderTests
    {
        [TestMethod]
        public void Encode_Then_Decode_Should_Be_Equal_To_Original_Value()
        {
            var value = 123456789;

            Assert.AreEqual(value, Encoder.Decode(Encoder.Encode(value)));
        }

        [TestMethod]
        public void Decode_Then_Encode_Should_Be_Equal_To_Original_Value()
        {
            var value = "D4DcvE56tE";

            Assert.AreEqual(value, Encoder.Encode(Encoder.Decode(value)));
        }
    }
}
