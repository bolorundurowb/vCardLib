using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TimezoneFieldSerializerTests
{
    [TestCase("-05:00", "TZ:-05:00")]
    public void Write_V2_ReturnsExpectedWireFormat(string timezone, string expected)
    {
        var serializer = new TimezoneFieldSerializer();
        ((IV2FieldSerializer<string>)serializer).Write(timezone).ShouldBe(expected);
    }

    [TestCase("-05:00", "TZ:-05:00")]
    public void Write_V3_ReturnsExpectedWireFormat(string timezone, string expected)
    {
        var serializer = new TimezoneFieldSerializer();
        ((IV3FieldSerializer<string>)serializer).Write(timezone).ShouldBe(expected);
    }

    [Test]
    public void Write_V4_ReturnsExpectedWireFormat()
    {
        var serializer = new TimezoneFieldSerializer();
        ((IV4FieldSerializer<string>)serializer).Write("America/New_York").ShouldBe("TZ:America/New_York");
    }
}
