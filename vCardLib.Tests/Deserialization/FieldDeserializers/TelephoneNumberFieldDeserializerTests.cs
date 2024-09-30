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
    public void V2ShouldParseSimple()
    {
        const string input = "TEL;TYPE=cell:(123) 555-5832";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Cell);
        result.Number.ShouldBe("(123) 555-5832");
    }

    [Test]
    public void V2ShouldParseSimpleWithPref()
    {
        const string input = "TEL;TYPE=cell;pref:(123) 555-5832";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.ShouldBe(TelephoneNumberType.Cell);
        result.Number.ShouldBe("(123) 555-5832");
    }

    [Test]
    public void V2ShouldParseWhenTypeKeywordIsOmittedForMetadata()
    {
        const string input = "TEL;CELL:(123) 555-5832";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Cell);
        result.Number.ShouldBe("(123) 555-5832");
    }

    [Test]
    public void V2ShouldParseWhenTypeKeywordIsOmittedForMetadataAndHasMultipleValues()
    {
        const string input = "TEL;CELL;WORK:(123) 555-5832";
        IV2FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Cell | TelephoneNumberType.Work);
        result.Number.ShouldBe("(123) 555-5832");
    }

    [Test]
    public void V3ShouldParseComplex()
    {
        const string input = "TEL;TYPE=work,voice,pref,msg:+1-213-555-1234";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Work | TelephoneNumberType.Voice | TelephoneNumberType.Preferred);
        result.Number.ShouldBe("+1-213-555-1234");
    }

    [Test]
    public void V3ShouldParseWhenTypeKeywordIsOmittedForMetadata()
    {
        const string input = "TEL;WORK:+1-213-555-1234";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Work);
        result.Number.ShouldBe("+1-213-555-1234");
    }

    [Test]
    public void V3ShouldParseWhenTypeKeywordIsOmittedForMetadataAndHasMultipleValues()
    {
        const string input = "TEL;WORK;CELL:+1-213-555-1234";
        IV3FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBeNull();
        result.Type.ShouldBe(TelephoneNumberType.Work | TelephoneNumberType.Cell);
        result.Number.ShouldBe("+1-213-555-1234");
    }

    [Test]
    public void V4ShouldParseComplex()
    {
        const string input = @" TEL;VALUE=uri;PREF=1;TYPE=""voice,home"":tel:+1-555-555-5555;ext=5555";
        IV4FieldDeserializer<TelephoneNumber> deserializer = new TelephoneNumberFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.ShouldBe(1);
        result.Type.ShouldBe(TelephoneNumberType.Voice | TelephoneNumberType.Home);
        result.Number.ShouldBe("+1-555-555-5555");
        result.Extension.ShouldBe("5555");
    }
}
