using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class UidFieldDeserializerTests
{
    [Test]
    public void Read_V2_ReturnsExpectedValue()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV2FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void Read_V3_ReturnsExpectedValue()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV3FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void Read_V4_ReturnsExpectedValue()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void Read_ValueContainingUidSubstring_ReturnsExpectedValue()
    {
        const string input = "UID:urn:UID:12345";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("urn:UID:12345");
    }

    [Test]
    public void Read_WithUuidFormat_ReturnsExpectedValue()
    {
        const string input = "UID:urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("urn:uuid:f81d4fae-7dec-11d0-a765-00a0c91e6bf6");
    }

    [Test]
    public void Read_WithSimpleString_ReturnsExpectedValue()
    {
        const string input = "UID:12345";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("12345");
    }

    [Test]
    public void Read_WithUrl_ReturnsExpectedValue()
    {
        const string input = "UID:https://example.com/user/12345";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("https://example.com/user/12345");
    }

    [Test]
    public void Read_WithWhitespace_TrimsValue()
    {
        const string input = "UID:  12345  ";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("12345");
    }
}
