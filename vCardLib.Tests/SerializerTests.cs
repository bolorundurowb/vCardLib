using System;
using System.Collections.Generic;
using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        [Test]
        public void ShouldSucceedWithV2Card()
        {
            var card = new vCard
            {
                Version = vCardVersion.V2,
                FamilyName = "Jane",
                GivenName = "John",
                MiddleName = "Janice",
                Prefix = "Sir",
                Suffix = "PhD"
            };
            var data = Serializer.Serialize(card);

            Assert.IsNotEmpty(data);
        }

        [Test]
        public void ShouldSucceedWithV3Card()
        {
            var card = new vCard
            {
                Version = vCardVersion.V3,
                FormattedName = "Jimmy Jane",
                NickName = "JJ",
                Note = "Do stuff",
                Url = "http://github.com",
                TimeZone = "GMT + 1",
                Geo = new Geo
                {
                    Latitude = 1.234,
                    Longitude = 4.321
                }
            };
            var data = Serializer.Serialize(card);

            Assert.IsNotEmpty(data);
        }


        [Test]
        public void MultipleV3CardsShouldHaveCorrectRevisionAndStartEndFrameFormat()
        {
            var cards = new List<vCard>
            {
                new vCard
                {
                    Version = vCardVersion.V3,
                    Revision = new DateTime(2022,01,01),
                    FormattedName = "Card1",
                },
                new vCard
                {
                    Version = vCardVersion.V3,
                    Revision = new DateTime(2022,01,01),
                    FormattedName = "Card2",
                }
            };
            var data = Serializer.Serialize(cards);

            Assert.AreEqual(data, @"BEGIN:VCARD
VERSION:3.0
REV:20220101T000000Z
N:;;;;
FN:Card1
KIND:Individual
GENDER:None
END:VCARD
BEGIN:VCARD
VERSION:3.0
REV:20220101T000000Z
N:;;;;
FN:Card2
KIND:Individual
GENDER:None
END:VCARD");
        }

        [Test]
        public void ShouldThrowWithV4Card()
        {
            var card = new vCard
            {
                Version = vCardVersion.V4,
                Organization = "Bing",
                Title = "Boss",
                Kind = ContactType.Group,
                Gender = GenderType.Female,
                Language = "en-GB",
                BirthDay = DateTime.Now
            };
            Assert.Throws<NotImplementedException>(delegate { Serializer.Serialize(card); });
        }
    }
}