using System;
using NUnit.Framework;
using vCardLib.Serializers;

namespace vCardLib.Tests.SerializerTests
{
    [TestFixture]
    public class V3SerializerTests
    {
        [Test]
        public void SerializeTest()
        {
            var vcardString = String.Empty;
            Assert.DoesNotThrow(delegate
            {
                var vcard = new vCard();
                vcardString = V3Serializer.Serialize(vcard);
            });
            Assert.IsEmpty(vcardString);
        }
    }
}