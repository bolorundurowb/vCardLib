using NUnit.Framework;
using Shouldly;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MailerFieldDeserializerTests
{
    [Test]
    public void ShouldReturnNullForV4()
    {
        const string input = "MAILER:PigeonMail 2.1";
        IV4FieldDeserializer<string?> deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnValueForV2OrV3()
    {
        const string input = "MAILER:PigeonMail 2.1";
        var deserializer = new MailerFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldNotBeNull();
        result.ShouldBe("PigeonMail 2.1");
    }
}