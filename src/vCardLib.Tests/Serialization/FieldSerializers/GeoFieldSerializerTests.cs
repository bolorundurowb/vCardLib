using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class GeoFieldSerializerTests
{
    [Test]
    public void FieldKey_ReturnsGeo()
    {
        var serializer = new GeoFieldSerializer();
        serializer.FieldKey.ShouldBe("GEO");
    }

    [Test]
    public void Write_V2_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(37.386013f, -122.08293f);
        IV2FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:37.386013;-122.08293");
    }

    [Test]
    public void Write_V3_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(37.386013f, -122.08293f);
        IV3FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:37.386013;-122.08293");
    }

    [Test]
    public void Write_V4_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(37.386013f, -122.08293f);
        IV4FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:geo:37.386013,-122.08293");
    }

    [Test]
    public void Write_V2_NegativeCoordinates_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(-33.8688f, 151.2093f);
        IV2FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:-33.8688;151.2093");
    }

    [Test]
    public void Write_V3_NegativeCoordinates_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(-33.8688f, 151.2093f);
        IV3FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:-33.8688;151.2093");
    }

    [Test]
    public void Write_V4_NegativeCoordinates_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(-33.8688f, 151.2093f);
        IV4FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:geo:-33.8688,151.2093");
    }

    [Test]
    public void Write_V4_ZeroCoordinates_ReturnsExpectedWireFormat()
    {
        var geo = new Geo(0f, 0f);
        IV4FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        var result = serializer.Write(geo);

        result.ShouldBe("GEO:geo:0,0");
    }

    [Test]
    public void Write_V2_RoundTripsThroughDeserializer()
    {
        var geo = new Geo(37.386013f, -122.082932f);
        IV2FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        IV2FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();

        var wire = serializer.Write(geo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Latitude.ShouldBe(geo.Latitude);
        roundTrip.Longitude.ShouldBe(geo.Longitude);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var geo = new Geo(37.386013f, -122.082932f);
        IV3FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        IV3FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();

        var wire = serializer.Write(geo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Latitude.ShouldBe(geo.Latitude);
        roundTrip.Longitude.ShouldBe(geo.Longitude);
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var geo = new Geo(37.386013f, -122.082932f);
        IV4FieldSerializer<Geo> serializer = new GeoFieldSerializer();
        IV4FieldDeserializer<Geo> deserializer = new GeoFieldDeserializer();

        var wire = serializer.Write(geo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Latitude.ShouldBe(geo.Latitude);
        roundTrip.Longitude.ShouldBe(geo.Longitude);
    }
}
