using System;
using NUnit.Framework;
using vCardLib.Deserializers;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class V4DeserializerTests
    {
        [Test]
        public void ParseTest()
        {
            Assert.Throws<NotImplementedException>(
                delegate
                {
                    V4Deserializer.Parse(new[] {"test"}, new vCard());
                });
        }
    }
}