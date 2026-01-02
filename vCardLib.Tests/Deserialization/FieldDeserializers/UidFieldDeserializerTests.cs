using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class UidFieldDeserializerTests
{
    [Test]
    public void ShouldParseUidV2()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV2FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void ShouldParseUidV3()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV3FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }

    [Test]
    public void ShouldParseUidV4()
    {
        const string input = "UID:19950401-080045-40000F192713";
        IV4FieldDeserializer<string> deserializer = new UidFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("19950401-080045-40000F192713");
    }
}
