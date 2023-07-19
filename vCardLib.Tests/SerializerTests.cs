using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Tests;

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

        data.ShouldNotBeEmpty();
    }

    [Test]
    public async Task SerializeToStream_Should_SerializeCard()
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

        var ms = new MemoryStream();
        ms.Length.ShouldBe(0);

        await Serializer.SerializeToStream(card, ms);

        ms.Length.ShouldBeGreaterThan(0);
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

        data.ShouldNotBeEmpty();
    }


    [Test]
    public void MultipleV3CardsShouldHaveCorrectRevisionAndStartEndFrameFormat()
    {
        var cards = new List<vCard>
        {
            new()
            {
                Version = vCardVersion.V3,
                Revision = new DateTime(2022, 01, 01),
                FormattedName = "Card1",
            },
            new()
            {
                Version = vCardVersion.V3,
                Revision = new DateTime(2022, 01, 01),
                FormattedName = "Card2",
            }
        };
        var data = Serializer.Serialize(cards);

        // we need to use Environment.NewLine to pass unit-tests also in Unix environments. VS hardcodes \n\r due to Windows newline.
        data.ShouldBe("BEGIN:VCARD" + Environment.NewLine +
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
    public void V2CardShouldSerializeCustomTelephoneType()
    {
        var card = new vCard
        {
            Version = vCardVersion.V2,
            Revision = new DateTime(2022, 01, 01),
            FamilyName = "Card1",
            FormattedName = "Card1",
            PhoneNumbers = new List<TelephoneNumber>
            {
                new()
                {
                    Type = TelephoneNumberType.Custom,
                    CustomTypeName = "CUSTOM,VALUE",
                    Value = "+1 234",
                },
                new()
                {
                    Type = TelephoneNumberType.Custom,
                    CustomTypeName = "CUSTOM1",
                    Value = "+1 234",
                }
            }
        };
        var data = Serializer.Serialize(card);

        // we need to use Environment.NewLine to pass unit-tests also in Unix environments. VS hardcodes \n\r due to Windows newline.
        data.ShouldBe("BEGIN:VCARD" + Environment.NewLine +
                      "VERSION:2.1" + Environment.NewLine +
                      "REV:20220101T000000Z" + Environment.NewLine +
                      "N:Card1;;;;" + Environment.NewLine +
                      "FN:Card1" + Environment.NewLine +
                      "KIND:Individual" + Environment.NewLine +
                      "GENDER:None" + Environment.NewLine +
                      "TEL;TYPE=\"CUSTOM,VALUE\":+1 234" + Environment.NewLine +
                      "TEL;TYPE=\"CUSTOM1\":+1 234" + Environment.NewLine +
                      "END:VCARD");
    }

    [Test]
    public void V3CardShouldSerializeCustomTelephoneType()
    {
        var card = new vCard
        {
            Version = vCardVersion.V3,
            Revision = new DateTime(2022, 01, 01),
            FamilyName = "Card1",
            FormattedName = "Card1",
            PhoneNumbers = new List<TelephoneNumber>
            {
                new()
                {
                    Type = TelephoneNumberType.Custom,
                    CustomTypeName = "CUSTOM,VALUE",
                    Value = "+1 234",
                },
                new()
                {
                    Type = TelephoneNumberType.Custom,
                    CustomTypeName = "CUSTOM1",
                    Value = "+1 234",
                }
            }
        };
        var data = Serializer.Serialize(card);

        // we need to use Environment.NewLine to pass unit-tests also in Unix environments. VS hardcodes \n\r due to Windows newline.
        data.ShouldBe("BEGIN:VCARD" + Environment.NewLine +
                      "VERSION:3.0" + Environment.NewLine +
                      "REV:20220101T000000Z" + Environment.NewLine +
                      "N:Card1;;;;" + Environment.NewLine +
                      "FN:Card1" + Environment.NewLine +
                      "KIND:Individual" + Environment.NewLine +
                      "GENDER:None" + Environment.NewLine +
                      "TEL;TYPE=\"CUSTOM,VALUE\":+1 234" + Environment.NewLine +
                      "TEL;TYPE=\"CUSTOM1\":+1 234" + Environment.NewLine +
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
        Should.Throw<NotImplementedException>(() => { Serializer.Serialize(card); });
    }
}
