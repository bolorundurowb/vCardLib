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
            Assert.IsEmpty(vcardString);
        }
    }
}