using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization;

/// <summary>
/// Integration-style <see cref="vCardDeserializer.FromContent"/> cases to exercise many
/// <see cref="vCardDeserializer"/> and field-deserializer branches (especially v4).
/// </summary>
[TestFixture]
public class vCardDeserializerRichContentTests
{
    [Test]
    public void FromContent_V4_WithGeoKindGenderAndCategories_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Rich\nGEO:12.5,-45.25\nKIND:group\nGENDER:O;non-binary\nCATEGORIES:alpha,beta\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.ShouldBe(vCardVersion.v4);
        card.FormattedName.ShouldBe("Rich");
        card.Geo!.Value.Latitude.ShouldBe(12.5f);
        card.Geo.Value.Longitude.ShouldBe(-45.25f);
        card.Kind.ShouldBe(ContactKind.Group);
        card.Gender.ShouldNotBeNull();
        card.Gender!.Value.Sex.ShouldBe(BiologicalSex.Other);
        card.Gender.Value.GenderIdentity.ShouldBe("non-binary");
        card.Categories.Count.ShouldBe(2);
        card.Categories.ShouldContain("ALPHA");
    }

    [Test]
    public void FromContent_V4_WithTelEmailPhotoAdrAndCustom_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Contact\nTEL:+15551234567\nEMAIL:me@example.org\nPHOTO:https://example.org/p.png\nADR:;;100 Main;City;ST;00000;US\nX-APP-ID:12345\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.PhoneNumbers.Count.ShouldBe(1);
        card.EmailAddresses.Count.ShouldBe(1);
        card.Photos.Count.ShouldBe(1);
        card.Addresses.Count.ShouldBe(1);
        card.CustomFields.Count.ShouldBe(1);
        card.CustomFields[0].Key.ShouldBe("X-APP-ID");
    }

    [Test]
    public void FromContent_V4_KindOrgAndDefaultIndividual_Parses()
    {
        var org = "BEGIN:VCARD\nVERSION:4.0\nFN:Org\nKIND:org\nEND:VCARD";
        vCardDeserializer.FromContent(org).Single().Kind.ShouldBe(ContactKind.Organization);

        var unknownKind = "BEGIN:VCARD\nVERSION:4.0\nFN:Def\nKIND:unknown-value\nEND:VCARD";
        vCardDeserializer.FromContent(unknownKind).Single().Kind.ShouldBe(ContactKind.Individual);
    }

    [Test]
    public void FromContent_V3_WithOptionalFields_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:V3\nGEO:1;2\nCATEGORIES:c1,c2\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.ShouldBe(vCardVersion.v3);
        card.Geo!.Value.Latitude.ShouldBe(1f);
        card.Categories.Count.ShouldBe(2);
    }

    [Test]
    public void FromContent_V2_WithGeoAndCategories_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:V2\nGEO:3;4\nCATEGORIES:one,two\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.ShouldBe(vCardVersion.v2);
        card.Geo!.Value.Longitude.ShouldBe(4f);
        card.Categories.Count.ShouldBe(2);
    }
}
