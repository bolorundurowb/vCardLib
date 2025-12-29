using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TitleFieldDeserializerTests
{
    [Test]
    public void Write_Should_SerializeV2()
    {
        const string input = "TITLE:Web & UI/UX Designer";
        IV2FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        string result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("Web & UI/UX Designer");
    }

    [Test]
    public void Write_Should_SerializeV3()
    {
        const string input = "TITLE:Web & UI/UX Designer";
        IV3FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        string result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("Web & UI/UX Designer");
    }

    [Test]
    public void Write_Should_SerializeV4()
    {
        const string input = "TITLE:Web & UI/UX Designer";
        IV4FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        string result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("Web & UI/UX Designer");
    }
}