using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class AddressFieldDeserializerTests
{
    [Test]
    public void Read_SimpleAddress_ReturnsCorrectAddress()
    {
        var input = "ADR:;;123 Main St;Anytown;State;12345;USA";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.ShouldBe("123 Main St");
        result.CityOrLocality.ShouldBe("Anytown");
        result.StateOrProvinceOrRegion.ShouldBe("State");
        result.PostalOrZipCode.ShouldBe("12345");
        result.Country.ShouldBe("USA");
    }

    [Test]
    public void Read_AddressWithType_ReturnsCorrectAddress()
    {
        var input = "ADR;TYPE=home:;;123 Main St;Anytown;State;12345;USA";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.ShouldBe("123 Main St");
        result.Type.ShouldBe(AddressType.Home);
    }

    [Test]
    public void Read_IncompleteAddress_ThrowsException()
    {
        var input = "ADR:;;123 Main St;Anytown";
        var deserializer = new AddressFieldDeserializer();

        Should.Throw<Exception>(() => deserializer.Read(input))
            .Message.ShouldBe("Address parts incomplete");
    }

    [Test]
    public void Read_WithGeoAndLabel_ParsesComponents()
    {
        var input = "ADR;TYPE=work;GEO=10.5,20.25;LABEL=HQ:;;100 Rd;Town;ST;99999;US";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.ShouldBe(AddressType.Work);
        result.Label.ShouldBe("HQ");
        result.Geographic.ShouldNotBeNull();
        result.Geographic!.Value.Latitude.ShouldBe(10.5f);
        result.Geographic.Value.Longitude.ShouldBe(20.25f);
        result.StreetAddress.ShouldBe("100 Rd");
    }

    [Test]
    public void Read_WithMultipleTypes_CombinesFlags()
    {
        var input = "ADR;TYPE=home;TYPE=work:;;s;c;r;p;co";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        ((result.Type & AddressType.Home) != 0).ShouldBeTrue();
        ((result.Type & AddressType.Work) != 0).ShouldBeTrue();
    }

    [Test]
    public void Read_WithQuotedPrintableEncoding_DecodesValue()
    {
        var input = "ADR;ENCODING=QUOTED-PRINTABLE:;;=41=42=43;c;r;p;co";
        var deserializer = new AddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.StreetAddress.ShouldBe("ABC");
    }
}
