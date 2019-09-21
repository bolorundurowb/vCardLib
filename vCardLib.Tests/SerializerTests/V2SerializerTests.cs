using System;
using NUnit.Framework;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Tests.SerializerTests
{
    [TestFixture]
    public class V2SerializerTests
    {
        [Test]
        public void SerializeTest()
        {
            var vcardString = String.Empty;
            Assert.DoesNotThrow(delegate
            {
                var vcard = new vCard();
                vcardString = V2Serializer.Serialize(vcard);
            });
            Assert.IsEmpty(vcardString);
        }
    }
}