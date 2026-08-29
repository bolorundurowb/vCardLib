using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class GeoFieldDeserializerTests
{
    [Test]
    public void Read_V2_Format_ReturnsExpectedValue()
    {
        const string input = "GEO:37.386013;-122.082932";
        IV2FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();
        var result = deserializer.Read(input);

        result.Latitude.Must().Be(37.386013f);
        result.Longitude.Must().Be(-122.082932f);
    }

    [Test]
    public void Read_V3_Format_ReturnsExpectedValue()
    {
        const string input = "GEO:37.386013;-122.082932";
        IV3FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();
        var result = deserializer.Read(input);

        result.Latitude.Must().Be(37.386013f);
        result.Longitude.Must().Be(-122.082932f);
    }

    [Test]
    public void Read_V4_Format_ReturnsExpectedValue()
    {
        const string input = "GEO:geo:37.386013,-122.082932";
        IV4FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();
        var result = deserializer.Read(input);

        result.Latitude.Must().Be(37.386013f);
        result.Longitude.Must().Be(-122.082932f);
    }
}