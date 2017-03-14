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
            string vcardString = String.Empty;
            Assert.DoesNotThrow(delegate
            {
                vCard vcard = new vCard();
                vcardString = V3Serializer.Serialize(vcard);
            });
            Assert.IsNotEmpty(vcardString);
            Assert.IsTrue(vcardString.Contains("BEGIN:VCARD"));
            Assert.IsTrue(vcardString.Contains("VERSION:3.0"));
            Assert.IsTrue(vcardString.Contains("END:VCARD"));
        }
    }
}