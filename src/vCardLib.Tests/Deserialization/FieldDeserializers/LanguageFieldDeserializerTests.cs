using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class LanguageFieldDeserializerTests
{
    [Test]
    public void Read_V4_ValidLanguage_ReturnsExpectedValue()
    {
        var input = "LANG:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV4FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.Must().NotBeNull();
        result.Value.Locale.Must().Be("en-US");
    }

    [Test]
    public void Read_V4_WithMetadata_ReturnsExpectedValue()
    {
        var input = "LANG;TYPE=home;PREF=1:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV4FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.Must().NotBeNull();
        result.Value.Locale.Must().Be("en-US");
        result.Value.Type.Must().Be("home");
        result.Value.Preference.Must().Be(1);
    }

    [Test]
    public void Read_V2_ReturnsNull()
    {
        var input = "LANG:en-US";
        var deserializer = new LanguageFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<vCardLib.Models.Language?>).Read(input);

        result.Must().BeNull();
    }
}
