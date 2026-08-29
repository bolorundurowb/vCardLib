using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;

namespace vCardLib.Tests.Serialization;

[TestFixture]
public class vCardSerializerTests
{
    [TestCase(vCardVersion.v2, "2.1")]
    [TestCase(vCardVersion.v3, "3.0")]
    [TestCase(vCardVersion.v4, "4.0")]
    public void Serialize_SingleCard_ReturnsVCardString(vCardVersion version, string expectedVersion)
    {
        var card = new vCard(version)
        {
            FormattedName = "John Doe",
            Name = new Name
            {
                FamilyName = "Doe",
                GivenName = "John",
                AdditionalNames = "Robert",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "Jr."
            },
            NickName = "Johnny",
            Title = "Software Engineer",
            Organization = new Organization
            {
                Name = "Tech Corp",
                PrimaryUnit = "Development",
                SecondaryUnit = "Backend"
            },
            Note = "Important contact",
            Url = new Url
            {
                Value = "https://example.com",
                Type = UrlType.Work,
                Preference = 1,
                Label = "Website",
                Charset = "UTF-8"
            },
            Timezone = "America/New_York",
            Uid = "urn:uuid:12345",
            BirthDay = new DateTime(1990, 5, 15),
            Anniversary = new DateTime(2015, 6, 20),
            Gender = new Gender { Sex = BiologicalSex.Male, GenderIdentity = "Non-binary" },
            Kind = ContactKind.Individual,
            Language = new Language { Locale = "en-US", Preference = 0, Type = "speech" },
            Mailer = "Outlook",
            Agent = "Agent Smith",
            PhoneNumbers = new List<TelephoneNumber>
            {
                new() { Number = "+1234567890", Type = TelephoneNumberType.Cell | TelephoneNumberType.Preferred },
                new() { Number = "+0987654321", Type = TelephoneNumberType.Work, Preference = 0 }
            },
            EmailAddresses = new List<EmailAddress>
            {
                new() { Value = "john.doe@example.com", Type = EmailAddressType.Work, Preference = 1 },
                new() { Value = "johnny@personal.com", Type = EmailAddressType.Home | EmailAddressType.Preferred }
            },
            Addresses = new List<Address>
            {
                new()
                {
                    StreetAddress = "123 Main St",
                    CityOrLocality = "Springfield",
                    StateOrProvinceOrRegion = "IL",
                    PostalOrZipCode = "62701",
                    Country = "USA",
                    Type = AddressType.Home
                }
            },
            Categories = new List<string> { "Friends", "Work" },
            CustomFields = new List<KeyValuePair<string, string>>
            {
                new("X-CUSTOM", "CustomValue")
            }
        };

        var result = vCardSerializer.Serialize(card);

        result.Must().Contain("BEGIN:VCARD");
        result.Must().Contain($"VERSION:{expectedVersion}");
        result.Must().Contain("FN:John Doe");
        result.Must().Contain("N:Doe;John;Robert;Mr.;Jr.");

        if (version != vCardVersion.v2)
            result.Must().Contain("NICKNAME:Johnny");

        result.Must().Contain("TITLE:Software Engineer");
        result.Must().Contain("ORG:Tech Corp;Development;Backend");
        result.Must().Contain("NOTE:Important contact");
        result.Must().Contain("URL");
        result.Must().Contain("https://example.com");
        result.Must().Contain("TZ:America/New_York");
        result.Must().Contain("UID:urn:uuid:12345");
        result.Must().Contain("BDAY:19900515");

        if (version == vCardVersion.v4)
            result.Must().Contain("ANNIVERSARY:");

        result.Must().Contain("TEL");
        result.Must().Contain("+1234567890");
        result.Must().Contain("+0987654321");
        result.Must().Contain("EMAIL");
        result.Must().Contain("john.doe@example.com");
        result.Must().Contain("johnny@personal.com");
        result.Must().Contain("ADR");
        result.Must().Contain("123 Main St");
        result.Must().Contain("Springfield");
        result.Must().Contain("CATEGORIES");
        result.Must().Contain("Friends,Work");
        result.Must().Contain("X-CUSTOM:CustomValue");
        result.Must().Contain("END:VCARD");
    }

    [TestCase(vCardVersion.v2, "2.1")]
    [TestCase(vCardVersion.v3, "3.0")]
    [TestCase(vCardVersion.v4, "4.0")]
    public void Serialize_MultipleCards_ReturnsVCardString(vCardVersion version, string expectedVersion)
    {
        var cards = new List<vCard>
        {
            new(version) { FormattedName = "John Doe" },
            new(version) { FormattedName = "Jane Doe" }
        };

        var result = vCardSerializer.Serialize(cards);

        result.Must().Contain("BEGIN:VCARD");
        result.Must().Contain($"VERSION:{expectedVersion}");
        result.Must().Contain("FN:John Doe");
        result.Must().Contain("FN:Jane Doe");
        result.Must().Contain("END:VCARD");

        // Count occurrences of BEGIN:VCARD
        var count = System.Text.RegularExpressions.Regex.Matches(result, "BEGIN:VCARD").Count;
        count.Must().Be(2);
    }

    [Test]
    public void Serialize_EmptyCollection_ReturnsEmptyString()
    {
        var result = vCardSerializer.Serialize(Enumerable.Empty<vCard>());
        result.Must().Be(string.Empty);
    }

    [Test]
    public void Serialize_OverrideVersion_UsesSpecifiedVersion()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card, vCardVersion.v4);

        result.Must().Contain("VERSION:4.0");
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Serialize_UsesStrictCrlfLineEndings(vCardVersion version)
    {
        var card = new vCard(version) { FormattedName = "A", Note = "Short" };
        var result = vCardSerializer.Serialize(card);

        for (var i = 0; i < result.Length; i++)
        {
            if (result[i] == '\n' && (i == 0 || result[i - 1] != '\r'))
                Ensure.Fail("Serialized vCard must not contain bare LF; use CRLF per RFC 6350 §3.2.");
        }

        result.Must().EndWith("\r\n");
    }

    [Test]
    public void Serialize_V4_LongNote_IsFoldedAndRoundTrips()
    {
        var longNote = new string('z', 120);
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Fold Test",
            Note = longNote
        };

        var wire = vCardSerializer.Serialize(card);
        wire.Must().Contain("\r\n ");
        foreach (var line in wire.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            (Encoding.UTF8.GetByteCount(line) <= 75).Must().BeTrue();

        var roundTrip = vCardDeserializer.FromContent(wire).Single();
        roundTrip.Note.Must().Be(longNote);
    }

    [Test]
    public void Serialize_V2_LongNote_IsFoldedAndRoundTrips()
    {
        var longNote = new string('y', 130);
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "V2 Fold",
            Note = longNote
        };

        var wire = vCardSerializer.Serialize(card);
        wire.Must().Contain("\r\n ");
        foreach (var line in wire.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            (Encoding.UTF8.GetByteCount(line) <= 75).Must().BeTrue();

        var roundTrip = vCardDeserializer.FromContent(wire).Single();
        roundTrip.Note.Must().Be(longNote);
    }

    [Test]
    public void Serialize_MultipleCards_UsesStrictCrlfBetweenCards()
    {
        var cards = new List<vCard>
        {
            new(vCardVersion.v4) { FormattedName = "First" },
            new(vCardVersion.v4) { FormattedName = "Second" }
        };

        var result = vCardSerializer.Serialize(cards);

        for (var i = 0; i < result.Length; i++)
        {
            if (result[i] == '\n' && (i == 0 || result[i - 1] != '\r'))
                Ensure.Fail("Serialized vCards must use CRLF only.");
        }

        result.Must().EndWith("\r\n");
        result.Must().Contain("FN:First");
        result.Must().Contain("FN:Second");
        System.Text.RegularExpressions.Regex.Matches(result, "BEGIN:VCARD").Count.Must().Be(2);
    }

    [Test]
    public void Serialize_WithGeo_IncludesGeoLine()
    {
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Geo Test",
            Geo = new Geo(37.386013f, -122.08293f)
        };

        var result = vCardSerializer.Serialize(card);

        result.Must().Contain("GEO:geo:37.386013,-122.08293");
    }

    [Test]
    public void Serialize_WithLogo_IncludesLogoLine()
    {
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Logo Test",
            Logo = new Photo("SGVsbG8=", "base64", null, "image/png", "SGVsbG8=")
        };

        var result = vCardSerializer.Serialize(card);

        result.Must().Contain("LOGO;");
        result.Must().Contain("SGVsbG8=");
    }

    [Test]
    public void Serialize_WithPhoto_UsesDataProperty()
    {
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Photo Test",
            Photos = new List<Photo>
            {
                new Photo("actual-image-data", "base64", null, "image/jpeg", null)
            }
        };

        var result = vCardSerializer.Serialize(card);

        result.Must().Contain("actual-image-data");
    }
}