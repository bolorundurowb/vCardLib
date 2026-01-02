using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class OrganizationFieldDeserializerTests
{
    [Test]
    public void ShouldParseSimpleOrganization()
    {
        const string input = "ORG:ABC, Inc.";
        IV2FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Name.ShouldBe("ABC, Inc.");
        result.PrimaryUnit.ShouldBeNull();
        result.SecondaryUnit.ShouldBeNull();
    }

    [Test]
    public void ShouldParseOrganizationWithUnits()
    {
        const string input = "ORG:ABC, Inc.;North American Division;Marketing";
        IV3FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Name.ShouldBe("ABC, Inc.");
        result.PrimaryUnit.ShouldBe("North American Division");
        result.SecondaryUnit.ShouldBe("Marketing");
    }

    [Test]
    public void ShouldUnescapeOrganizationName()
    {
        const string input = @"ORG:ABC\, Inc.;North American Division";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Name.ShouldBe("ABC, Inc.");
        result.PrimaryUnit.ShouldBe("North American Division");
    }

    [Test]
    public void ShouldHandleEmptyInput()
    {
        const string input = "ORG:";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(string.Empty);
        result.PrimaryUnit.ShouldBeNull();
        result.SecondaryUnit.ShouldBeNull();
    }
}
