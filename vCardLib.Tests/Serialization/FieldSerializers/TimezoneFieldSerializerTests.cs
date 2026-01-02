using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TimezoneFieldSerializerTests
{
    [Test]
    public void WriteV2_ReturnsCorrectString()
    {
        var serializer = new TimezoneFieldSerializer();
        var result = ((IV2FieldSerializer<string>)serializer).Write("-05:00");
        result.ShouldBe("TZ:-05:00");
    }

    [Test]
    public void WriteV3_ReturnsCorrectString()
    {
        var serializer = new TimezoneFieldSerializer();
        var result = ((IV3FieldSerializer<string>)serializer).Write("-05:00");
        result.ShouldBe("TZ:-05:00");
    }

    [Test]
    public void WriteV4_ReturnsCorrectString()
    {
        var serializer = new TimezoneFieldSerializer();
        var result = ((IV4FieldSerializer<string>)serializer).Write("America/New_York");
        result.ShouldBe("TZ:America/New_York");
    }
}
