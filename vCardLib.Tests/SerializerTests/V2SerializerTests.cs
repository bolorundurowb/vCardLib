using System;
using NUnit.Framework;
using vCardLib.Serializers;

namespace vCardLib.Tests.SerializerTests
{
    [TestFixture]
    public class V2SerializerTests
    {
        [Test]
        public void SerializeTest()
        {
            string vcardString = String.Empty;
            Assert.DoesNotThrow(delegate
            {
                vCard vcard = new vCard();
                vcardString = V2Serializer.Serialize(vcard);
            });
            Assert.IsNotEmpty(vcardString);
            Assert.IsTrue(vcardString.Contains("BEGIN:VCARD"));
            Assert.IsTrue(vcardString.Contains("VERSION:2.1"));
            Assert.IsTrue(vcardString.Contains("END:VCARD"));
        }
    }
}