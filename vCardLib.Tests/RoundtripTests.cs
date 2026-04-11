using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;

namespace vCardLib.Tests;

[TestFixture]
public class RoundtripTests
{
    [Test]
    public void Roundtrip_V2Minimal_ShouldPreserveData()
    {
        var card = new vCard(vCardVersion.v2)
        {
            Name = new Name("Doe", "John", null, null, null),
            FormattedName = "John Doe",
        };
        card.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
        card.PhoneNumbers.Add(new TelephoneNumber("123456789"));

        var serialized = vCardSerializer.Serialize(card);
        var deserialized = vCardDeserializer.FromContent(serialized).First();

        deserialized.Version.ShouldBe(vCardVersion.v2);
        deserialized.Name.Value.FamilyName.ShouldBe("Doe");
        deserialized.Name.Value.GivenName.ShouldBe("John");
        deserialized.FormattedName.ShouldBe("John Doe");
        deserialized.EmailAddresses.Count.ShouldBe(1);
        deserialized.EmailAddresses[0].Value.ShouldBe("john.doe@example.com");
        deserialized.PhoneNumbers.Count.ShouldBe(1);
        deserialized.PhoneNumbers[0].Number.ShouldBe("123456789");
    }

    [Test]
    public void Roundtrip_V3Complex_ShouldPreserveData()
    {
        var card = new vCard(vCardVersion.v3)
        {
            Name = new Name("Doe", "Jane", null, null, null),
            FormattedName = "Jane Doe",
            Note = "A sample note",
            Title = "Engineer"
        };
        card.EmailAddresses.Add(new EmailAddress("jane.doe@example.org", EmailAddressType.Work | EmailAddressType.Internet, 1));
        card.PhoneNumbers.Add(new TelephoneNumber("987654321", TelephoneNumberType.Home));
        card.Url = new Url("http://janedoe.com");

        var serialized = vCardSerializer.Serialize(card);
        var deserialized = vCardDeserializer.FromContent(serialized).First();

        deserialized.Version.ShouldBe(vCardVersion.v3);
        deserialized.Name.Value.FamilyName.ShouldBe("Doe");
        deserialized.FormattedName.ShouldBe("Jane Doe");
        deserialized.Note.ShouldBe("A sample note");
        deserialized.Title.ShouldBe("Engineer");
        
        var email = deserialized.EmailAddresses.First();
        email.Value.ShouldBe("jane.doe@example.org");
        email.Type.ShouldBe(EmailAddressType.Work | EmailAddressType.Internet);
        email.Preference.ShouldBe(1);

        var phone = deserialized.PhoneNumbers.First();
        phone.Number.ShouldBe("987654321");
        phone.Type.ShouldBe(TelephoneNumberType.Home);

        deserialized.Url.Value.Value.ShouldBe("http://janedoe.com");
    }

    [Test]
    public void Roundtrip_V4Complex_ShouldPreserveData()
    {
        var card = new vCard(vCardVersion.v4)
        {
            Name = new Name("Smith", "Alice", null, null, null),
            FormattedName = "Alice Smith",
            Gender = new Gender(BiologicalSex.Female, null)
        };
        card.EmailAddresses.Add(new EmailAddress("alice@example.com", EmailAddressType.Work, 1));
        card.Url = new Url("http://alice.me", UrlType.Home | UrlType.Blog, 2, "Alice's Blog");

        var serialized = vCardSerializer.Serialize(card);
        var deserialized = vCardDeserializer.FromContent(serialized).First();

        deserialized.Version.ShouldBe(vCardVersion.v4);
        deserialized.FormattedName.ShouldBe("Alice Smith");
        deserialized.Gender.Value.Sex.ShouldBe(BiologicalSex.Female);

        var email = deserialized.EmailAddresses.First();
        email.Value.ShouldBe("alice@example.com");
        email.Type.ShouldBe(EmailAddressType.Work);
        email.Preference.ShouldBe(1);

        deserialized.Url.Value.Value.ShouldBe("http://alice.me");
        deserialized.Url.Value.Type.ShouldBe(UrlType.Home | UrlType.Blog);
        deserialized.Url.Value.Preference.ShouldBe(2);
        deserialized.Url.Value.Label.ShouldBe("Alice's Blog");
    }
}
