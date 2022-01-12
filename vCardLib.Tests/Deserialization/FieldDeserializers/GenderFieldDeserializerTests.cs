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
    public void ShouldReturnNullForV2()
    {
        const string input = "GENDER:M";
        IV2FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnNullForV3()
    {
        const string input = "GENDER:M";
        IV3FieldDeserializer<Gender?> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnValueForSexOnly()
    {
        const string input = "GENDER:M";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.ShouldNotBeNull();
        result.Sex.ShouldBe(BiologicalSex.Male);
        result.GenderIdentity.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnValueForSexAndGenderIdentity()
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
    public void ShouldReturnValueForGenderIdentityOnly()
    {
        const string input = "GENDER:;it's complicated";
        IV4FieldDeserializer<Gender> deserializer = new GenderFieldDeserializer();
        var result = deserializer.Read(input);

        result.Sex.ShouldBeNull();
        result.GenderIdentity.ShouldNotBeNull();
        result.GenderIdentity.ShouldBe("it's complicated");
    }
    
    [Test]
    public void ShouldReturnParseSexCorrectly()
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