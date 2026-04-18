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
    public void Read_SimpleOrganization_ReturnsCorrectValue()
    {
        const string input = "ORG:ABC, Inc.";
        IV2FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Name.ShouldBe("ABC, Inc.");
        result.Value.PrimaryUnit.ShouldBeNull();
        result.Value.SecondaryUnit.ShouldBeNull();
    }

    [Test]
    public void Read_OrganizationWithUnits_ReturnsCorrectValue()
    {
        const string input = "ORG:ABC, Inc.;North American Division;Marketing";
        IV3FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Name.ShouldBe("ABC, Inc.");
        result.Value.PrimaryUnit.ShouldBe("North American Division");
        result.Value.SecondaryUnit.ShouldBe("Marketing");
    }

    [Test]
    public void Read_EscapedOrganizationName_ReturnsUnescapedValue()
    {
        const string input = @"ORG:ABC\, Inc.;North American Division";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Name.ShouldBe("ABC, Inc.");
        result.Value.PrimaryUnit.ShouldBe("North American Division");
    }

    [Test]
    public void Read_EmptyInput_ReturnsEmptyOrganization()
    {
        const string input = "ORG:";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Name.ShouldBe(string.Empty);
        result.Value.PrimaryUnit.ShouldBeNull();
        result.Value.SecondaryUnit.ShouldBeNull();
    }
}
