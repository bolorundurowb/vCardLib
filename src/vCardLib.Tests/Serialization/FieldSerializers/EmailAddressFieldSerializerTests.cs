using NUnit.Framework;
using OmniAssert;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class EmailAddressFieldSerializerTests
{
    [Test]
    public void Write_V2_SimpleEmail_ReturnsExpectedWireFormat()
    {
        var email = new EmailAddress("john@example.com");
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV2FieldSerializer<EmailAddress>).Write(email);

        result.Must().Be("EMAIL:john@example.com");
    }

    [Test]
    public void Write_V2_WithTypes_ReturnsExpectedWireFormat()
    {
        var email = new EmailAddress("john@example.com", EmailAddressType.Home | EmailAddressType.Internet);
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV2FieldSerializer<EmailAddress>).Write(email);

        result.Must().Contain(";HOME");
        result.Must().Contain(";INTERNET");
    }

    [Test]
    public void Write_V4_WithPreference_ReturnsExpectedWireFormat()
    {
        var email = new EmailAddress("john@example.com", EmailAddressType.Work, 2);
        var serializer = new EmailAddressFieldSerializer();
        var result = (serializer as IV4FieldSerializer<EmailAddress>).Write(email);

        result.Must().Be("EMAIL;TYPE=work;PREF=2:john@example.com");
    }
}
