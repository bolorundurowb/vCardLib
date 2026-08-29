using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class OrganizationFieldDeserializerTests
{
    [Test]
    public void Read_SimpleOrganization_ReturnsExpectedValue()
    {
        const string input = "ORG:ABC, Inc.";
        IV2FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Name.Must().Be("ABC, Inc.");
        result.Value.PrimaryUnit.Must().BeNull();
        result.Value.SecondaryUnit.Must().BeNull();
    }

    [Test]
    public void Read_OrganizationWithUnits_ReturnsExpectedValue()
    {
        const string input = "ORG:ABC, Inc.;North American Division;Marketing";
        IV3FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Name.Must().Be("ABC, Inc.");
        result.Value.PrimaryUnit.Must().Be("North American Division");
        result.Value.SecondaryUnit.Must().Be("Marketing");
    }

    [Test]
    public void Read_EscapedOrganizationName_ReturnsUnescapedValue()
    {
        const string input = @"ORG:ABC\, Inc.;North American Division";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Name.Must().Be("ABC, Inc.");
        result.Value.PrimaryUnit.Must().Be("North American Division");
    }

    [Test]
    public void Read_EmptyInput_ReturnsEmptyOrganization()
    {
        const string input = "ORG:";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Name.Must().Be(string.Empty);
        result.Value.PrimaryUnit.Must().BeNull();
        result.Value.SecondaryUnit.Must().BeNull();
    }

    [Test]
    public void Read_ValueContainingOrgSubstring_ReturnsExpectedValue()
    {
        const string input = "ORG:ORG:Tech;Division";
        IV4FieldDeserializer<Organization?> deserializer = new OrganizationFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Name.Must().Be("ORG:Tech");
        result.Value.PrimaryUnit.Must().Be("Division");
    }
}
