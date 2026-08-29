using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class AddressFieldDeserializerTests
{
    [Test]
    public void Read_SimpleAddress_ReturnsExpectedAddress()
    {
        var input = "ADR:;;123 Main St;Anytown;State;12345;USA";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.Must().Be("123 Main St");
        result.CityOrLocality.Must().Be("Anytown");
        result.StateOrProvinceOrRegion.Must().Be("State");
        result.PostalOrZipCode.Must().Be("12345");
        result.Country.Must().Be("USA");
    }

    [Test]
    public void Read_AddressWithType_ReturnsExpectedAddress()
    {
        var input = "ADR;TYPE=home:;;123 Main St;Anytown;State;12345;USA";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.Must().Be("123 Main St");
        result.Type.Must().Be(AddressType.Home);
    }

    [Test]
    public void Read_IncompleteAddress_ThrowsException()
    {
        var input = "ADR:;;123 Main St;Anytown";
        var deserializer = new AddressFieldDeserializer();

        Ensure.Throws<Exception>(() => deserializer.Read(input))
            .WithMessage("Address parts incomplete");
    }

    [Test]
    public void Read_WithGeoAndLabel_ParsesComponents()
    {
        var input = "ADR;TYPE=work;GEO=10.5,20.25;LABEL=HQ:;;100 Rd;Town;ST;99999;US";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.Must().Be(AddressType.Work);
        result.Label.Must().Be("HQ");
        result.Geographic.Must().NotBeNull();
        result.Geographic!.Value.Latitude.Must().Be(10.5f);
        result.Geographic.Value.Longitude.Must().Be(20.25f);
        result.StreetAddress.Must().Be("100 Rd");
    }

    [Test]
    public void Read_WithMultipleTypes_CombinesFlags()
    {
        var input = "ADR;TYPE=home;TYPE=work:;;s;c;r;p;co";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        ((result.Type & AddressType.Home) != 0).Must().BeTrue();
        ((result.Type & AddressType.Work) != 0).Must().BeTrue();
    }

    [Test]
    public void Read_WithQuotedPrintableEncoding_DecodesValue()
    {
        var input = "ADR;ENCODING=QUOTED-PRINTABLE:;;=41=42=43;c;r;p;co";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.Must().Be("ABC");
    }
}
