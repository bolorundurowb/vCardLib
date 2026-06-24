using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TelephoneNumberFieldDeserializerTests
{
    [Test]
    public void Read_V2_SimpleInput_ReturnsExpectedValue()
    {
        const string input = "TEL;TYPE=cell:(123) 555-5832";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Cell);
        result.Number.ShouldBe("(123) 555-5832");
    }

    [Test]
    public void Read_V3_ComplexInput_ReturnsExpectedValue()
    {
        const string input = "TEL;TYPE=work,voice,pref,msg:+1-213-555-1234";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.HasFlag(TelephoneNumberType.Work).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Voice).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Preferred).ShouldBeTrue();
        result.Number.ShouldBe("+1-213-555-1234");
    }

    [Test]
    public void Read_V4_ComplexInput_ReturnsExpectedValue()
    {
        const string input = @" TEL;VALUE=uri;PREF=1;TYPE=""voice,home"":tel:+1-555-555-5555;ext=5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.HasFlag(TelephoneNumberType.Voice).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Home).ShouldBeTrue();
        result.Number.ShouldBe("+1-555-555-5555");
        result.Extension.ShouldBe("5555");
    }

    [Test]
    public void Read_V2_WithBarePref_ReturnsPreferenceOne()
    {
        const string input = "TEL;PREF:+1-555-555-5555";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V3_WithBarePref_ReturnsPreferenceOne()
    {
        const string input = "TEL;PREF:+1-555-555-5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V3_WithExplicitPrefValue_ReturnsExpectedPreference()
    {
        const string input = "TEL;PREF=2:+1-555-555-5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(2);
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V4_WithExplicitPrefValue_ReturnsExpectedPreference()
    {
        const string input = "TEL;PREF=3:+1-555-555-5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(3);
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V3_WithInvalidPrefValue_ReturnsNull()
    {
        const string input = "TEL;PREF=invalid:+1-555-555-5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V2_WithQuotedPrintable_DecodesValue()
    {
        const string input = "TEL;ENCODING=QUOTED-PRINTABLE:+1=2D555=2D5555";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-5555");
    }

    [Test]
    public void Read_V3_WithQuotedPrintable_DecodesValue()
    {
        const string input = "TEL;ENCODING=QUOTED-PRINTABLE:+1=2D555=2D5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-5555");
    }

    [Test]
    public void Read_V4_WithQuotedPrintable_DecodesValue()
    {
        const string input = "TEL;ENCODING=QUOTED-PRINTABLE:+1=2D555=2D5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-5555");
    }

    [Test]
    public void Read_V2_WithMultipleTypes_CombinesTypes()
    {
        const string input = "TEL;VOICE;FAX;CELL:+1-555-555-5555";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(TelephoneNumberType.Voice).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Fax).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Cell).ShouldBeTrue();
    }

    [Test]
    public void Read_V3_WithMultipleTypes_CombinesTypes()
    {
        const string input = "TEL;TYPE=VOICE;TYPE=FAX;TYPE=CELL:+1-555-555-5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(TelephoneNumberType.Voice).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Fax).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Cell).ShouldBeTrue();
    }

    [Test]
    public void Read_V4_WithMultipleTypes_CombinesTypes()
    {
        const string input = "TEL;TYPE=VOICE;TYPE=FAX;TYPE=CELL:+1-555-555-5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(TelephoneNumberType.Voice).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Fax).ShouldBeTrue();
        result.Type.HasFlag(TelephoneNumberType.Cell).ShouldBeTrue();
    }

    [Test]
    public void Read_V2_WithUnknownType_IgnoresType()
    {
        const string input = "TEL;UNKNOWN:+1-555-555-5555";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.ShouldBe(TelephoneNumberType.None);
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V2_WithExtension_ParsesExtension()
    {
        const string input = "TEL:+1-555-555-5555;ext=1234";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-555-5555");
        result.Extension.ShouldBe("1234");
    }

    [Test]
    public void Read_V3_WithExtension_ParsesExtension()
    {
        const string input = "TEL:+1-555-555-5555;ext=1234";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-555-5555");
        result.Extension.ShouldBe("1234");
    }

    [Test]
    public void Read_V4_WithUriFormat_ParsesNumber()
    {
        const string input = "TEL;VALUE=uri:tel:+1-555-555-5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V4_WithValueParameter_StoresValue()
    {
        const string input = "TEL;VALUE=text:+1-555-555-5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V3_WithNoParameters_ReturnsSimpleNumber()
    {
        const string input = "TEL:+1-555-555-5555";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.ShouldBe(TelephoneNumberType.None);
        result.Preference.ShouldBeNull();
        result.Extension.ShouldBeNull();
        result.Number.ShouldBe("+1-555-555-5555");
    }

    [Test]
    public void Read_V2_WithNoParameters_ReturnsSimpleNumber()
    {
        const string input = "TEL:+1-555-555-5555";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.ShouldBe(TelephoneNumberType.None);
        result.Preference.ShouldBeNull();
        result.Extension.ShouldBeNull();
        result.Number.ShouldBe("+1-555-555-5555");
    }
}
