using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MailerFieldDeserializerTests
{
    [Test]
    public void Read_InputV4_ShouldReturnNull()
    {
        const string input = "MAILER:PigeonMail 2.1";
        IV4FieldDeserializer<string?> deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_InputV2OrV3_ShouldReturnValue()
    {
        const string input = "MAILER:PigeonMail 2.1";
        var deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("PigeonMail 2.1");
    }
}