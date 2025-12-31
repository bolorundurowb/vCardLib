using NUnit.Framework;
using Shouldly;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class LanguageFieldSerializerTests
{
    [Test]
    public void Write_V4_ValidLanguage_ReturnsCorrectString()
    {
        var language = new Language("en-US", 1, "home");
        var serializer = new LanguageFieldSerializer();
        var result = (serializer as IV4FieldSerializer<Language>).Write(language);

        result.ShouldBe("LANG;TYPE=home;PREF=1:en-US");
    }

    [Test]
    public void Write_V4_NoMetadata_ReturnsCorrectString()
    {
        var language = new Language("en-US");
        var serializer = new LanguageFieldSerializer();
        var result = (serializer as IV4FieldSerializer<Language>).Write(language);

        result.ShouldBe("LANG:en-US");
    }

    [Test]
    public void Write_V2_ReturnsNull()
    {
        var language = new Language("en-US");
        var serializer = new LanguageFieldSerializer();
        var result = (serializer as IV2FieldSerializer<Language>).Write(language);

        result.ShouldBeNull();
    }
}
