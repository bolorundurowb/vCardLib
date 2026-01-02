using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MemberFieldDeserializerTests
{
    [Test]
    public void V2ShouldReturnNull()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV2FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void V3ShouldReturnNull()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV3FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void V4ShouldParseValue()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("mailto:subscriber1@example.com");
    }

    [Test]
    public void V4ShouldParseWithSpaces()
    {
        const string input = "MEMBER : mailto:subscriber1@example.com ";
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("mailto:subscriber1@example.com");
    }
}
