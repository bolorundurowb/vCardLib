using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TelephoneNumberFieldSerializerTests
{
    [Test]
    public void WriteV2_WithMultipleTypes_ReturnsCorrectString()
    {
        var tel = new TelephoneNumber { Number = "123456", Type = TelephoneNumberType.Home | TelephoneNumberType.Voice };
        var serializer = new TelephoneNumberFieldSerializer();
        var result = ((IV2FieldSerializer<TelephoneNumber>)serializer).Write(tel);
        
        result.ShouldContain("TEL");
        result.ShouldContain("TYPE=home");
        result.ShouldContain("TYPE=voice");
        result.ShouldEndWith(":123456");
    }

    [Test]
    public void WriteV3_WithPreference_ReturnsCorrectString()
    {
        var tel = new TelephoneNumber { Number = "123456", Preference = 1 };
        var serializer = new TelephoneNumberFieldSerializer();
        var result = serializer.Write(tel);
        
        result.ShouldContain("TEL;PREF=1:123456");
    }
}
