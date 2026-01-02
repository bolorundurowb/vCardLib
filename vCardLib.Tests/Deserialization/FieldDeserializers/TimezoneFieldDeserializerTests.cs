using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TimezoneFieldDeserializerTests
{
    [Test]
    public void ShouldParseTimezoneV2()
    {
        const string input = "TZ:-05:00";
        IV2FieldDeserializer<string> deserializer = new TimezoneFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("-05:00");
    }

    [Test]
    public void ShouldParseTimezoneV3()
    {
        const string input = "TZ:-05:00";
        IV3FieldDeserializer<string> deserializer = new TimezoneFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("-05:00");
    }

    [Test]
    public void ShouldParseTimezoneV4()
    {
        const string input = "TZ:Raleigh/North America";
        IV4FieldDeserializer<string> deserializer = new TimezoneFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("Raleigh/North America");
    }
}
