using NUnit.Framework;
using OmniAssert;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TelephoneNumberFieldSerializerTests
{
    [Test]
    public void Write_V2_MultipleTypes_ReturnsExpectedWireFormat()
    {
        var tel = new TelephoneNumber { Number = "123456", Type = TelephoneNumberType.Home | TelephoneNumberType.Voice };
        var serializer = new TelephoneNumberFieldSerializer();
        var result = ((IV2FieldSerializer<TelephoneNumber>)serializer).Write(tel);

        result.Must().Contain("TEL");
        result.Must().Contain(";HOME");
        result.Must().Contain(";VOICE");
        result.Must().EndWith(":123456");
    }

    [Test]
    public void Write_V3_WithPreference_ReturnsExpectedWireFormat()
    {
        var tel = new TelephoneNumber { Number = "123456", Preference = 1 };
        var serializer = new TelephoneNumberFieldSerializer();
        var result = ((IV3FieldSerializer<TelephoneNumber>)serializer).Write(tel);

        result.Must().Contain("TEL;PREF:123456");
    }
}
