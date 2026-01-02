using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
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

        result.ShouldContain("BEGIN:VCARD");
        result.ShouldContain($"VERSION:{expectedVersion}");
        result.ShouldContain("FN:John Doe");
        result.ShouldContain("N:Doe;John;Robert;Mr.;Jr.");
        
        if (version != vCardVersion.v2) 
            result.ShouldContain("NICKNAME:Johnny");
        
        result.ShouldContain("TITLE:Software Engineer");
        result.ShouldContain("ORG:Tech Corp;Development;Backend");
        result.ShouldContain("NOTE:Important contact");
        result.ShouldContain("URL");
        result.ShouldContain("https://example.com");
        result.ShouldContain("TZ:America/New_York");
        result.ShouldContain("UID:urn:uuid:12345");
        result.ShouldContain("BDAY:19900515");
        
        if (version == vCardVersion.v4) 
            result.ShouldContain("ANNIVERSARY:");
        
        result.ShouldContain("TEL");
        result.ShouldContain("+1234567890");
        result.ShouldContain("+0987654321");
        result.ShouldContain("EMAIL");
        result.ShouldContain("john.doe@example.com");
        result.ShouldContain("johnny@personal.com");
        result.ShouldContain("ADR");
        result.ShouldContain("123 Main St");
        result.ShouldContain("Springfield");
        result.ShouldContain("CATEGORIES");
        result.ShouldContain("Friends,Work");
        result.ShouldContain("X-CUSTOM: CustomValue");
        result.ShouldContain("END:VCARD");
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

        result.ShouldContain("BEGIN:VCARD");
        result.ShouldContain($"VERSION:{expectedVersion}");
        result.ShouldContain("FN:John Doe");
        result.ShouldContain("FN:Jane Doe");
        result.ShouldContain("END:VCARD");

        // Count occurrences of BEGIN:VCARD
        var count = System.Text.RegularExpressions.Regex.Matches(result, "BEGIN:VCARD").Count;
        count.ShouldBe(2);
    }

    [Test]
    public void Serialize_EmptyCollection_ReturnsEmptyString()
    {
        var result = vCardSerializer.Serialize(Enumerable.Empty<vCard>());
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void Serialize_OverrideVersion_UsesSpecifiedVersion()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card, vCardVersion.v4);

        result.ShouldContain("VERSION:4.0");
    }

    [Test]
    public void Serialize_V3_ReturnsCorrectVersion()
    {
        var card = new vCard(vCardVersion.v3)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card);

        result.ShouldContain("VERSION:3.0");
    }
}