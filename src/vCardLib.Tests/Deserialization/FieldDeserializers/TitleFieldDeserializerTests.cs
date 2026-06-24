using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TitleFieldDeserializerTests
{
    private const string Input = "TITLE:Web & UI/UX Designer";

    [Test]
    public void Read_V2_ReturnsExpectedValue()
    {
        IV2FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        deserializer.Read(Input).ShouldBe("Web & UI/UX Designer");
    }

    [Test]
    public void Read_V3_ReturnsExpectedValue()
    {
        IV3FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        deserializer.Read(Input).ShouldBe("Web & UI/UX Designer");
    }

    [Test]
    public void Read_V4_ReturnsExpectedValue()
    {
        IV4FieldDeserializer<string> deserializer = new TitleFieldDeserializer();
        deserializer.Read(Input).ShouldBe("Web & UI/UX Designer");
    }
}
