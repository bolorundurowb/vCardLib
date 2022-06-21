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

            // we need to use Environment.NewLine to pass unit-tests also in Unix environments. VS hardcodes \n\r due to Windows newline.
            Assert.AreEqual(data, "BEGIN:VCARD" + Environment.NewLine +
                                  "VERSION:3.0" + Environment.NewLine +
                                  "REV:20220101T000000Z" + Environment.NewLine +
                                  "N:;;;;" + Environment.NewLine +
                                  "FN:Card1" + Environment.NewLine +
                                  "KIND:Individual" + Environment.NewLine +
                                  "GENDER:None" + Environment.NewLine +
                                  "END:VCARD" + Environment.NewLine +
                                  "BEGIN:VCARD" + Environment.NewLine +
                                  "VERSION:3.0" + Environment.NewLine +
                                  "REV:20220101T000000Z" + Environment.NewLine +
                                  "N:;;;;" + Environment.NewLine +
                                  "FN:Card2" + Environment.NewLine +
                                  "KIND:Individual" + Environment.NewLine +
                                  "GENDER:None" + Environment.NewLine +
                                  "END:VCARD");
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