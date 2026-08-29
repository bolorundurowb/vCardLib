using NUnit.Framework;
using OmniAssert;
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
        serializer.FieldKey.Must().Be("ADR");
    }

    [Test]
    public void Write_DefaultVersion_UsesV3Format()
    {
        var address = SampleAddress();
        var serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V2_SimpleAddress_ReturnsExpectedWireFormat()
    {
        var address = SampleAddress();
        IV2FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V3_SimpleAddress_ReturnsExpectedWireFormat()
    {
        var address = SampleAddress();
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_SimpleAddress_ReturnsExpectedWireFormat()
    {
        var address = SampleAddress();
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V2_WithHomeType_UsesBareTypeToken()
    {
        var address = SampleAddress(AddressType.Home);
        IV2FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR;HOME:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V3_WithHomeType_UsesTypeParameter()
    {
        var address = SampleAddress(AddressType.Home);
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR;TYPE=home:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_WithMultipleTypes_JoinsTypes()
    {
        var address = SampleAddress(AddressType.Home | AddressType.Work);
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be("ADR;TYPE=home,work:PO Box 1;Suite 100;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_V4_WithLabelAndGeo_IncludesExtraParameters()
    {
        var address = SampleAddress(AddressType.Work, "Head Office", new Geo(37.386013f, -122.08293f));
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.Must().Be(
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

        roundTrip.PostOfficeBox.Must().Be(address.PostOfficeBox);
        roundTrip.ApartmentOrSuiteNumber.Must().Be(address.ApartmentOrSuiteNumber);
        roundTrip.StreetAddress.Must().Be(address.StreetAddress);
        roundTrip.CityOrLocality.Must().Be(address.CityOrLocality);
        roundTrip.StateOrProvinceOrRegion.Must().Be(address.StateOrProvinceOrRegion);
        roundTrip.PostalOrZipCode.Must().Be(address.PostalOrZipCode);
        roundTrip.Country.Must().Be(address.Country);
        roundTrip.Type.Must().Be(address.Type);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var address = SampleAddress(AddressType.Work, "HQ");
        IV3FieldSerializer<Address> serializer = new AddressFieldSerializer();
        IV3FieldDeserializer<Address> deserializer = new AddressFieldDeserializer();

        var wire = serializer.Write(address)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.StreetAddress.Must().Be(address.StreetAddress);
        roundTrip.Type.Must().Be(address.Type);
        roundTrip.Label.Must().Be(address.Label);
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var address = SampleAddress(AddressType.Work | AddressType.Postal, "Office", new Geo(10.5f, 20.25f));
        IV4FieldSerializer<Address> serializer = new AddressFieldSerializer();
        IV4FieldDeserializer<Address> deserializer = new AddressFieldDeserializer();

        var wire = serializer.Write(address)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.StreetAddress.Must().Be(address.StreetAddress);
        roundTrip.CityOrLocality.Must().Be(address.CityOrLocality);
        roundTrip.Type.Must().Be(address.Type);
        roundTrip.Label.Must().Be(address.Label);
        roundTrip.Geographic.Must().NotBeNull();
        roundTrip.Geographic!.Value.Latitude.Must().Be(address.Geographic!.Value.Latitude);
        roundTrip.Geographic.Value.Longitude.Must().Be(address.Geographic.Value.Longitude);
    }
}
