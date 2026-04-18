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
    public void Read_V2SimpleInput_ReturnsCorrectValue()
    {
        const string input = "EMAIL:johndoe@hotmail.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(EmailAddressType.None);
        result.Value.ShouldBe("johndoe@hotmail.com");
    }

    [Test]
    public void Read_V3ComplexInput_ReturnsCorrectValue()
    {
        const string input = "EMAIL;type=INTERNET;type=WORK;pref:johnDoe@example.org";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.HasFlag(EmailAddressType.Internet).ShouldBeTrue();
        result.Type.HasFlag(EmailAddressType.Work).ShouldBeTrue();
        result.Value.ShouldBe("johnDoe@example.org");
    }

    [Test]
    public void Read_V4ComplexInput_ReturnsCorrectValue()
    {
        const string input = "EMAIL;type=Aol;type=HOME;pref=1:johnDoe@example.org";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.HasFlag(EmailAddressType.Aol).ShouldBeTrue();
        result.Type.HasFlag(EmailAddressType.Home).ShouldBeTrue();
        result.Value.ShouldBe("johnDoe@example.org");
    }
}
