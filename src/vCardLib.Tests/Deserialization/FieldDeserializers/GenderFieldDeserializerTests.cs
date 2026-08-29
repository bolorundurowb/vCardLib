using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class GenderFieldDeserializerTests
{
    [Test]
    public void Read_V2_ReturnsNull()
    {
        const string input = "GENDER:M";
        IV2FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().BeNull();
    }

    [Test]
    public void Read_V3_ReturnsNull()
    {
        const string input = "GENDER:M";
        IV3FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().BeNull();
    }

    [Test]
    public void Read_SexOnly_ReturnsExpectedValue()
    {
        const string input = "GENDER:M";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.Must().NotBeNull();
        result.Sex.Must().Be(BiologicalSex.Male);
        result.GenderIdentity.Must().BeNull();
    }

    [Test]
    public void Read_SexAndGenderIdentity_ReturnsExpectedValue()
    {
        const string input = "GENDER:F;grrrl";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.Must().NotBeNull();
        result.Sex.Must().Be(BiologicalSex.Female);
        result.GenderIdentity.Must().NotBeNull();
        result.GenderIdentity.Must().Be("grrrl");
    }

    [Test]
    public void Read_GenderIdentityOnly_ReturnsExpectedValue()
    {
        const string input = "GENDER:;it's complicated";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.Must().BeNull();
        result.GenderIdentity.Must().NotBeNull();
        result.GenderIdentity.Must().Be("it's complicated");
    }

    [Test]
    public void Read_MultipleSexInputs_ParsesSexCorrectly()
    {
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var input = "GENDER:O";
        var result = deserializer.Read(input);

        result.Sex.Must().NotBeNull();
        result.Sex.Must().Be(BiologicalSex.Other);

        input = "GENDER:U";
        result = deserializer.Read(input);

        result.Sex.Must().NotBeNull();
        result.Sex.Must().Be(BiologicalSex.Unknown);

        input = "GENDER:N";
        result = deserializer.Read(input);

        result.Sex.Must().NotBeNull();
        result.Sex.Must().Be(BiologicalSex.None);
    }
}