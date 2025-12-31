using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class LanguageFieldDeserializerTests
{
    [Test]
    public void Read_V4_ValidLanguage_ReturnsLanguage()
    {
        var input = "LANG:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV4FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.ShouldNotBeNull();
        result.Value.Locale.ShouldBe("en-US");
    }

    [Test]
    public void Read_V4_WithMetadata_ReturnsLanguageWithMetadata()
    {
        var input = "LANG;TYPE=home;PREF=1:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV4FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.ShouldNotBeNull();
        result.Value.Locale.ShouldBe("en-US");
        result.Value.Type.ShouldBe("home");
        result.Value.Preference.ShouldBe(1);
    }

    [Test]
    public void Read_V2_ReturnsNull()
    {
        var input = "LANG:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.ShouldBeNull();
    }
}
