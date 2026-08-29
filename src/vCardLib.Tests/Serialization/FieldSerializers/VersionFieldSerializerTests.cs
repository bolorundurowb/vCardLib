using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Enums;
using vCardLib.Serialization.FieldSerializers;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class VersionFieldSerializerTests
{
    [Test]
    public void FieldKey_ReturnsVersion()
    {
        VersionFieldSerializer.FieldKey.Must().Be("VERSION");
    }

    [TestCase(vCardVersion.v2, "VERSION:2.1")]
    [TestCase(vCardVersion.v3, "VERSION:3.0")]
    [TestCase(vCardVersion.v4, "VERSION:4.0")]
    public void Write_ReturnsExpectedWireFormat(vCardVersion version, string expected)
    {
        var result = VersionFieldSerializer.Write(version);
        result.Must().Be(expected);
    }

    [Test]
    public void Write_UnsupportedVersion_ThrowsArgumentOutOfRangeException()
    {
        Ensure.Throws<ArgumentOutOfRangeException>(() => VersionFieldSerializer.Write((vCardVersion)99));
    }

    [TestCase("VERSION:2.1", vCardVersion.v2)]
    [TestCase("VERSION:3.0", vCardVersion.v3)]
    [TestCase("VERSION:4.0", vCardVersion.v4)]
    public void Write_RoundTripsThroughDeserializer(string wire, vCardVersion expectedVersion)
    {
        var roundTrip = VersionDeserializer.Read(wire);
        roundTrip.Must().Be(expectedVersion);
    }
}
