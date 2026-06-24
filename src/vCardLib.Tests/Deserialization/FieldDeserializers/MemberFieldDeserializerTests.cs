using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MemberFieldDeserializerTests
{
    private const string Input = "MEMBER:mailto:subscriber1@example.com";

    [Test]
    public void Read_V2_ReturnsNull()
    {
        IV2FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        deserializer.Read(Input).ShouldBeNull();
    }

    [Test]
    public void Read_V3_ReturnsNull()
    {
        IV3FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        deserializer.Read(Input).ShouldBeNull();
    }

    [Test]
    public void Read_V4_ReturnsExpectedValue()
    {
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        deserializer.Read(Input).ShouldBe("mailto:subscriber1@example.com");
    }

    [Test]
    public void Read_V4_WhenLineHasExtraSpaces_ReturnsExpectedValue()
    {
        const string spacedInput = "MEMBER : mailto:subscriber1@example.com ";
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        deserializer.Read(spacedInput).ShouldBe("mailto:subscriber1@example.com");
    }
}
