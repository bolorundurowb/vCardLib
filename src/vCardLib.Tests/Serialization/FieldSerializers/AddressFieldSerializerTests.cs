using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class AddressFieldSerializerTests
{
    private static Address SampleAddress(AddressType type = AddressType.None, string? label = null, Geo? geo = null) =>
        new("PO Box 1", "Suite 100", "123 Main St", "Anytown", "State", "12345", "USA", type, label, geo);

    [Test]
    public void FieldKey_ReturnsAdr()
    {
        var serializer = new AddressFieldSerializer();
        serializer.FieldKey.ShouldBe("ADR");
    }

    [Test]
    public void Write_DefaultVersion_UsesV3Format()
    {
        var address = SampleAddress();
        var serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V2_SimpleAddress_ReturnsCorrectString()
    {
        var address = SampleAddress();
        IV2FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V3_SimpleAddress_ReturnsCorrectString()
    {
        var address = SampleAddress();
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_SimpleAddress_ReturnsCorrectString()
    {
        var address = SampleAddress();
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V2_WithHomeType_UsesBareTypeToken()
    {
        var address = SampleAddress(AddressType.Home);
        IV2FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR;HOME:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V3_WithHomeType_UsesTypeParameter()
    {
        var address = SampleAddress(AddressType.Home);
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR;TYPE=home:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_WithMultipleTypes_JoinsTypes()
    {
        var address = SampleAddress(AddressType.Home | AddressType.Work);
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR;TYPE=home,work:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_WithLabelAndGeo_IncludesExtraParameters()
    {
        var address = SampleAddress(AddressType.Work, "Head Office", new Geo(37.386013f, -122.08293f));
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe(
            "ADR;TYPE=work;LABEL=Head Office;GEO=37.386013,-122.08293:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V2_RoundTripsThroughDeserializer()
    {
        var address = SampleAddress(AddressType.Home);
        IV2FieldSerializer<Address> serializer = new AddressFieldSerializer();
        IV2FieldDeserializer<Address> deserializer = new AddressFieldDeserializer();

        var wire = serializer.Write(address)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.PostOfficeBox.ShouldBe(address.PostOfficeBox);
        roundTrip.ApartmentOrSuiteNumber.ShouldBe(address.ApartmentOrSuiteNumber);
        roundTrip.StreetAddress.ShouldBe(address.StreetAddress);
        roundTrip.CityOrLocality.ShouldBe(address.CityOrLocality);
        roundTrip.StateOrProvinceOrRegion.ShouldBe(address.StateOrProvinceOrRegion);
        roundTrip.PostalOrZipCode.ShouldBe(address.PostalOrZipCode);
        roundTrip.Country.ShouldBe(address.Country);
        roundTrip.Type.ShouldBe(address.Type);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var address = SampleAddress(AddressType.Work, "HQ");
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        IV3FieldDeserializer<Address> deserializer = new AddressFieldDeserializer();

        var wire = serializer.Write(address)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.StreetAddress.ShouldBe(address.StreetAddress);
        roundTrip.Type.ShouldBe(address.Type);
        roundTrip.Label.ShouldBe(address.Label);
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var address = SampleAddress(AddressType.Work | AddressType.Postal, "Office", new Geo(10.5f, 20.25f));
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        IV4FieldDeserializer<Address> deserializer = new AddressFieldDeserializer();

        var wire = serializer.Write(address)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.StreetAddress.ShouldBe(address.StreetAddress);
        roundTrip.CityOrLocality.ShouldBe(address.CityOrLocality);
        roundTrip.Type.ShouldBe(address.Type);
        roundTrip.Label.ShouldBe(address.Label);
        roundTrip.Geographic.ShouldNotBeNull();
        roundTrip.Geographic!.Value.Latitude.ShouldBe(address.Geographic!.Value.Latitude);
        roundTrip.Geographic.Value.Longitude.ShouldBe(address.Geographic.Value.Longitude);
    }
}
