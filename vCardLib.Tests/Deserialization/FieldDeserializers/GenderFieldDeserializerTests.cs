using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class GenderFieldDeserializerTests
{
    [Test]
    public void Read_InputV2_ShouldReturnNull()
    {
        const string input = "GENDER:M";
        IV2FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_InputV3_ShouldReturnNull()
    {
        const string input = "GENDER:M";
        IV3FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_SexOnlyInput_ShouldReturnValue()
    {
        const string input = "GENDER:M";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.Male);
        result.GenderIdentity.ShouldBeNull();
    }

    [Test]
    public void Read_SexAndGenderIdentityInput_ShouldReturnValue()
    {
        const string input = "GENDER:F;grrrl";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.Female);
        result.GenderIdentity.ShouldNotBeNull();
        result.GenderIdentity.ShouldBe("grrrl");
    }

    [Test]
    public void Read_GenderIdentityOnlyInput_ShouldReturnValue()
    {
        const string input = "GENDER:;it's complicated";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.ShouldBeNull();
        result.GenderIdentity.ShouldNotBeNull();
        result.GenderIdentity.ShouldBe("it's complicated");
    }

    [Test]
    public void Read_InputWithSex_ShouldParseSexCorrectly()
    {
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var input = "GENDER:O";
        var result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.Other);

        input = "GENDER:U";
        result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.Unknown);

        input = "GENDER:N";
        result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.None);
    }
}