using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization;

// The structured (N, ORG, ADR) and list (CATEGORIES) fields must survive a public
// Serialize -> Deserialize round trip when their components contain the vCard delimiters
// or a backslash. Covers the v3/v4 codec added here; v2.1 (no escaping) is asserted separately.
[TestFixture]
public class StructuredFieldEscapingRoundTripTests
{
    private static vCard RoundTrip(vCard card, vCardVersion version)
    {
        var text = vCardSerializer.Serialize(card, version);
        return vCardDeserializer.FromContent(text).First();
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Categories_WithDelimitersAndBackslash_RoundTrips(vCardVersion version)
    {
        var input = new List<string> { "Food, Inc.", "R&D; Legal", @"C:\share", "plain" };
        var card = new vCard(version) { FormattedName = "X", Categories = input };

        var result = RoundTrip(card, version).Categories;

        Console.WriteLine($"in=[{string.Join("|", input)}] out=[{string.Join("|", result)}]");
        result.Must().BeSequenceEqual(input);
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Organization_WithSemicolonCommaAndBackslash_RoundTrips(vCardVersion version)
    {
        var org = new Organization("Ben & Jerry's; Homemade, Inc.", @"C:\temp Unit", @"Trailing\");
        var card = new vCard(version) { FormattedName = "X", Organization = org };

        var result = RoundTrip(card, version).Organization;

        Console.WriteLine($"in=[{org.Name}|{org.PrimaryUnit}|{org.SecondaryUnit}] " +
                          $"out=[{result?.Name}|{result?.PrimaryUnit}|{result?.SecondaryUnit}]");
        result.Must().NotBeNull();
        result.Value.Name.Must().Be(org.Name);
        result.Value.PrimaryUnit.Must().Be(org.PrimaryUnit);
        result.Value.SecondaryUnit.Must().Be(org.SecondaryUnit);
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Name_ComponentWithSemicolonAndComma_RoundTrips(vCardVersion version)
    {
        var name = new Name("Fam;ily,Name", "Giv\\en", "Add", "Mr.", "Esq.");
        var card = new vCard(version) { FormattedName = "X", Name = name };

        var result = RoundTrip(card, version).Name;

        Console.WriteLine($"in=[{name.FamilyName}] out=[{result?.FamilyName}] given=[{result?.GivenName}]");
        result.Must().NotBeNull();
        result.Value.FamilyName.Must().Be(name.FamilyName);
        result.Value.GivenName.Must().Be(name.GivenName);
        result.Value.AdditionalNames.Must().Be(name.AdditionalNames);
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Address_ComponentWithSemicolonCommaAndBackslash_RoundTrips(vCardVersion version)
    {
        var address = new Address
        {
            StreetAddress = @"12;3 Main, St\ Back",
            CityOrLocality = "Any;town",
            StateOrProvinceOrRegion = "ST",
            PostalOrZipCode = "12345",
            Country = "USA"
        };
        var card = new vCard(version) { FormattedName = "X", Addresses = new List<Address> { address } };

        var result = RoundTrip(card, version).Addresses.First();

        Console.WriteLine($"in=[{address.StreetAddress}] out=[{result.StreetAddress}] city=[{result.CityOrLocality}]");
        result.StreetAddress.Must().Be(address.StreetAddress);
        result.CityOrLocality.Must().Be(address.CityOrLocality);
        result.Country.Must().Be(address.Country);
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Organization_PlausibleWindowsPath_IsNotRegexUnescaped(vCardVersion version)
    {
        // Regex.Unescape used to decode \t here, turning C:\temp into C:<TAB>emp.
        var org = new Organization(@"C:\temp Corp", null, null);
        var card = new vCard(version) { FormattedName = "X", Organization = org };

        var result = RoundTrip(card, version).Organization;

        result.Must().NotBeNull();
        result.Value.Name.Must().Be(@"C:\temp Corp");
        result.Value.Name.Must().NotContain("\t");
    }

    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void Categories_WithNewline_RoundTrips(vCardVersion version)
    {
        var value = "Line one" + Environment.NewLine + "Line two";
        var card = new vCard(version) { FormattedName = "X", Categories = new List<string> { value } };

        var result = RoundTrip(card, version).Categories;

        result.Count.Must().Be(1);
        result[0].Must().Be(value);
    }

    // --- Escape-aware split truth table (deserializer level) ---

    [Test]
    public void OrgSplit_UnescapedSemicolon_SplitsComponents()
    {
        var result = ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read("ORG:a;b");
        result!.Value.Name.Must().Be("a");
        result.Value.PrimaryUnit.Must().Be("b");
    }

    [Test]
    public void OrgSplit_EscapedSemicolon_IsOneLiteralComponent()
    {
        var result = ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(@"ORG:a\;b");
        result!.Value.Name.Must().Be("a;b");
        result.Value.PrimaryUnit.Must().BeNull();
    }

    [Test]
    public void OrgSplit_EscapedBackslashThenSemicolon_SplitsAfterLiteralBackslash()
    {
        // \\ ; -> escaped backslash followed by a structural separator.
        var result = ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(@"ORG:a\\;b");
        result!.Value.Name.Must().Be(@"a\");
        result.Value.PrimaryUnit.Must().Be("b");
    }

    [Test]
    public void CategoriesSplit_EscapedComma_IsOneValue()
    {
        var result = new CategoriesFieldDeserializer().Read(@"CATEGORIES:a\,b,c");
        result.Must().BeSequenceEqual(new List<string> { "a,b", "c" });
    }

    // --- Lenient decode of loose input: undefined escapes keep the backslash (RFC 6350 3.4).
    //     Regex.Unescape used to apply .NET regex grammar here (\t -> TAB, trailing \ dropped).

    [Test]
    public void OrgDecode_RawWindowsPath_KeepsBackslashNotTab()
    {
        var result = ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(@"ORG:C:\temp");
        result!.Value.Name.Must().Be(@"C:\temp");
        result.Value.Name.Must().NotContain("\t");
    }

    [Test]
    public void OrgDecode_TrailingBackslash_IsPreserved()
    {
        var result = ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(@"ORG:Acme\");
        result!.Value.Name.Must().Be(@"Acme\");
    }

    // --- LABEL codec (Regex.Unescape misuse removed) ---

    [Test]
    public void Label_WindowsPath_UnitRoundTrips()
    {
        var label = new Label(@"C:\temp\report;final,v2", AddressType.Home);
        var line = ((IV3FieldSerializer<Label>)new LabelFieldSerializer()).Write(label)!;
        var result = new LabelFieldDeserializer().Read(line);

        Console.WriteLine($"line=[{line}] in=[{label.Text}] out=[{result.Text}]");
        result.Text.Must().Be(label.Text);
        result.Text.Must().NotContain("\t");
        result.Type.Must().Be(AddressType.Home);
    }

    // --- No regression ---

    [Test]
    public void PlainValues_SerializeWithoutEscaping()
    {
        var card = new vCard(vCardVersion.v3)
        {
            FormattedName = "X",
            Name = new Name("Doe", "John", "Middle", "Mr.", "Esq."),
            Organization = new Organization("ABC Inc", "Sales", null),
            Categories = new List<string> { "INTERNET", "IETF" }
        };

        var text = vCardSerializer.Serialize(card, vCardVersion.v3);

        text.Must().Contain("N:Doe;John;Middle;Mr.;Esq.");
        text.Must().Contain("ORG:ABC Inc;Sales");
        text.Must().Contain("CATEGORIES:INTERNET,IETF");
        text.Must().NotContain(@"\");
    }

    [Test]
    public void V2_DoesNotBackslashEscape()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "X",
            Organization = new Organization("A; B, C", null, null)
        };

        var text = vCardSerializer.Serialize(card, vCardVersion.v2);

        text.Must().Contain("ORG:A; B, C");
        text.Must().NotContain(@"\;");
        text.Must().NotContain(@"\,");
    }
}
