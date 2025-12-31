using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class EmailAddressFieldSerializerTests
{
    [Test]
    public void Write_V2_SimpleEmail_ReturnsCorrectString()
    {
        var email = new EmailAddress("john@example.com");
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV2FieldSerializer<EmailAddress>).Write(email);

        result.ShouldBe("EMAIL:john@example.com");
    }

    [Test]
    public void Write_V2_WithTypes_ReturnsCorrectString()
    {
        var email = new EmailAddress("john@example.com", EmailAddressType.Home | EmailAddressType.Internet);
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV2FieldSerializer<EmailAddress>).Write(email);

        // DecomposeEmailAddressType is called for each type
        result.ShouldContain("TYPE=HOME");
        result.ShouldContain("TYPE=INTERNET");
    }

    [Test]
    public void Write_V4_WithPreference_ReturnsCorrectString()
    {
        var email = new EmailAddress("john@example.com", EmailAddressType.Work, 2);
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV4FieldSerializer<EmailAddress>).Write(email);

        result.ShouldBe("EMAIL;TYPE=work;PREF=2:john@example.com");
    }
}
