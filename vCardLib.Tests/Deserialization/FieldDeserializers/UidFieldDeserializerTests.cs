using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class UidFieldDeserializerTests
{
    [Test]
    public void Read_InputV2_ShouldParseCorrectly()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV2FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void Read_InputV3_ShouldParseCorrectly()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV3FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void Read_InputV4_ShouldParseCorrectly()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }
}
