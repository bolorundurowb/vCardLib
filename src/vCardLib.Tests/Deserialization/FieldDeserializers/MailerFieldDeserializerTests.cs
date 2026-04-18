using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MailerFieldDeserializerTests
{
    [Test]
    public void Read_V4Version_ReturnsNull()
    {
        const string input = "MAILER:PigeonMail 2.1";
        IV4FieldDeserializer<string?> deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_V2OrV3Version_ReturnsCorrectValue()
    {
        const string input = "MAILER:PigeonMail 2.1";
        var deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("PigeonMail 2.1");
    }
}