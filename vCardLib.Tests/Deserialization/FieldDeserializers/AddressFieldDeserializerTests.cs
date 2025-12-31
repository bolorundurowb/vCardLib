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
}
