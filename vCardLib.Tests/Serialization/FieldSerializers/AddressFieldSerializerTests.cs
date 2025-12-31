using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class AddressFieldSerializerTests
{
    [Test]
    public void Write_SimpleAddress_ReturnsCorrectString()
    {
        var address = new Address("", "", "123 Main St", "Anytown", "State", "12345", "USA");
        var serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        result.ShouldBe("ADR:;;123 Main St;Anytown;State;12345;USA");
    }

    [Test]
    public void Write_AddressWithType_ReturnsCorrectString()
    {
        var address = new Address("", "", "123 Main St", "Anytown", "State", "12345", "USA", AddressType.Home);
        var serializer = new AddressFieldSerializer();
        var result = serializer.Write(address);

        // Based on the implementation: builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, addressType.DecomposeAddressType());
        // It might be ADR;TYPE=HOME:;;123 Main St;Anytown;State;12345;USA
        result.ShouldContain("TYPE=HOME");
        result.ShouldContain("123 Main St");
    }
}
