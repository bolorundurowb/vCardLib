using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class EmailAddressFieldDeserializerTests
{
    [Test]
    public void V2ShouldParseSimple()
    {
        const string input = @"EMAIL:johndoe@hotmail.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(EmailAddressType.None);
        result.Value.ShouldBe("johndoe@hotmail.com");
    }

    [Test]
    public void V3ShouldParseComplex()
    {
        const string input = @"EMAIL;type=INTERNET;type=WORK;pref:johnDoe@example.org";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.ShouldBe(EmailAddressType.Internet | EmailAddressType.Work);
        result.Value.ShouldBe("johnDoe@example.org");
    }

    [Test]
    public void V4ShouldParseComplex()
    {
        const string input = @"EMAIL;type=Aol;type=HOME;pref=1:johnDoe@example.org";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.ShouldBe(EmailAddressType.Aol | EmailAddressType.Home);
        result.Value.ShouldBe("johnDoe@example.org");
    }
}
