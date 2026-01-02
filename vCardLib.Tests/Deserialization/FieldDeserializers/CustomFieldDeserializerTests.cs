using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class CustomFieldDeserializerTests
{
    [Test]
    public void ShouldParseCustomField()
    {
        const string input = "X-CUSTOM:Value";
        var deserializer = new CustomFieldDeserializer();
        var result = ((IV2FieldDeserializer<KeyValuePair<string, string>>)deserializer).Read(input);

        result.Key.ShouldBe("X-CUSTOM");
        result.Value.ShouldBe("Value");
    }

    [Test]
    public void ShouldParseCustomFieldWithSpaces()
    {
        const string input = "X-CUSTOM : Value ";
        var deserializer = new CustomFieldDeserializer();
        var result = ((IV3FieldDeserializer<KeyValuePair<string, string>>)deserializer).Read(input);

        result.Key.ShouldBe("X-CUSTOM");
        result.Value.ShouldBe("Value");
    }

    [Test]
    public void ShouldParseCustomFieldWithMultipleColons()
    {
        const string input = "X-CUSTOM:Value:With:Colons";
        var deserializer = new CustomFieldDeserializer();
        var result = ((IV4FieldDeserializer<KeyValuePair<string, string>>)deserializer).Read(input);

        result.Key.ShouldBe("X-CUSTOM:Value:With");
        result.Value.ShouldBe("Colons");
    }
}
