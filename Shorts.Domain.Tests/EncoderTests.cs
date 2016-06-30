using System;
using FluentAssertions;
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

        [TestMethod]
        public void Decode_Throws_For_Invalid_Input()
        {
            var value = "D4Dcv-E56tE";

            Action act = () => Encoder.Decode(value);

            act.ShouldThrow<ArgumentException>().Where(_ => _.Message.Contains("Invalid input"));
        }
    }
}
