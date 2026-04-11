using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class MemberFieldDeserializerTests
{
    [Test]
    public void Read_V2Input_ShouldReturnNull()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV2FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_V3Input_ShouldReturnNull()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV3FieldDeserializer<string?> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_V4Input_ShouldParseValue()
    {
        const string input = "MEMBER:mailto:subscriber1@example.com";
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("mailto:subscriber1@example.com");
    }

    [Test]
    public void Read_V4InputWithSpaces_ShouldParseValue()
    {
        const string input = "MEMBER : mailto:subscriber1@example.com ";
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("mailto:subscriber1@example.com");
    }
}
