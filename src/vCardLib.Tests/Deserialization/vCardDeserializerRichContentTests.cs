using System;
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
        card.Categories.ShouldContain("alpha");
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

    [Test]
    public void FromContent_V4_ScalarProfileFields_Parses()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nN:Doe;Jane;;;\nFN:Jane Doe\nUID:urn:uuid:abc\nTITLE:Director\nTZ:Europe/London\nBDAY:19850315\nREV:20240601T101530Z\nANNIVERSARY:20100704\nORG:Acme Corp;R&D\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name.ShouldNotBeNull();
        card.Name!.Value.FamilyName.ShouldBe("Doe");
        card.Name.Value.GivenName.ShouldBe("Jane");
        card.Uid.ShouldBe("urn:uuid:abc");
        card.Title.ShouldBe("Director");
        card.Timezone.ShouldBe("Europe/London");
        card.BirthDay.ShouldNotBeNull();
        card.BirthDay!.Value.Year.ShouldBe(1985);
        card.Revision.ShouldNotBeNull();
        card.Anniversary.ShouldNotBeNull();
        card.Organization.ShouldNotBeNull();
        card.Organization!.Value.Name.ShouldBe("Acme Corp");
    }

    [Test]
    public void FromContent_V3_WithUrl_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:Jane\nURL:http://example.org\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Url.ShouldNotBeNull();
        card.Url!.Value.Value.ShouldBe("http://example.org");
    }

    [Test]
    public void FromContent_V4_WithUrl_ThrowsInvalidCastException()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Jane\nURL:http://example.org\nEND:VCARD";

        Should.Throw<InvalidCastException>(() => vCardDeserializer.FromContent(content).Single());
    }

    [Test]
    public void FromContent_V4_WithLanguage_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Lang\nLANG:en-GB\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Language.ShouldNotBeNull();
        card.Language!.Value.Locale.ShouldBe("en-GB");
    }

    [Test]
    public void FromContent_V4_MultiplePhonesEmailsAndAddresses_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Multi\nTEL:+111\nTEL:+222\nEMAIL:one@example.org\nEMAIL:two@example.org\nADR:;;1 Main;A;S;1;US\nADR:;;2 Oak;B;T;2;CA\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.PhoneNumbers.Count.ShouldBe(2);
        card.PhoneNumbers.Select(p => p.Number).ShouldContain("+111");
        card.PhoneNumbers.Select(p => p.Number).ShouldContain("+222");
        card.EmailAddresses.Count.ShouldBe(2);
        card.EmailAddresses.Select(e => e.Value).ShouldContain("one@example.org");
        card.Addresses.Count.ShouldBe(2);
        card.Addresses.Select(a => a.StreetAddress).ShouldContain("1 Main");
        card.Addresses.Select(a => a.StreetAddress).ShouldContain("2 Oak");
    }

    [Test]
    public void FromContent_V4_MultiplePhotos_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Photos\nPHOTO:https://example.org/a.png\nPHOTO:https://example.org/b.png\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Photos.Count.ShouldBe(2);
        card.Photos.Select(p => p.Data).ShouldContain("https://example.org/a.png");
        card.Photos.Select(p => p.Data).ShouldContain("https://example.org/b.png");
    }

    [Test]
    public void FromContent_V4_WithMembers_Parses()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Group\nKIND:group\nMEMBER:mailto:one@example.org\nMEMBER:mailto:two@example.org\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Members.Count.ShouldBe(2);
        card.Members.ShouldContain("mailto:one@example.org");
        card.Members.ShouldContain("mailto:two@example.org");
    }

    [Test]
    public void FromContent_V3_WithNickname_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:John Doe\nNICKNAME:Johnny\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.NickName.ShouldBe("Johnny");
    }

    [Test]
    public void FromContent_V2_WithNickname_ReturnsNull()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:John Doe\nNICKNAME:Johnny\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.NickName.ShouldBeNull();
    }

    [Test]
    public void FromContent_V4_WithNameAndNote_ParsesBoth()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nN:Smith;Sam;;;\nFN:Sam Smith\nNOTE:Important client\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name.ShouldNotBeNull();
        card.Name!.Value.FamilyName.ShouldBe("Smith");
        card.Note.ShouldBe("Important client");
    }

    [Test]
    public void FromContent_V4_LogoLineStoredAsCustomField()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Logo\nLOGO:https://example.org/logo.png\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Logo.ShouldBeNull();
        card.CustomFields.Count.ShouldBe(1);
        card.CustomFields[0].Key.ShouldBe("LOGO:https");
        card.CustomFields[0].Value.ShouldBe("//example.org/logo.png");
    }

    [Test]
    public void FromContent_V4_AgentAndMailer_ReturnNull()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:V4\nAGENT:Rep\nMAILER:Client\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Agent.ShouldBeNull();
        card.Mailer.ShouldBeNull();
    }

    [Test]
    public void FromContent_V3_WithAgentMailerAndProdId_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:V3\nAGENT:Rep\nMAILER:Thunderbird\nPRODID:-//Example//vCard//EN\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Agent.ShouldBe("Rep");
        card.Mailer.ShouldBe("Thunderbird");
    }

    [Test]
    public void FromContent_V2_WithMailer_Parses()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:V2\nMAILER:PicoMail\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Mailer.ShouldBe("PicoMail");
    }

    [Test]
    public void FromContent_MultipleCustomFields_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Custom\nX-FOO:bar\nX-BAZ:qux\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.CustomFields.Count.ShouldBe(2);
        card.CustomFields.ShouldContain(kv => kv.Key == "X-FOO" && kv.Value == "bar");
        card.CustomFields.ShouldContain(kv => kv.Key == "X-BAZ" && kv.Value == "qux");
    }
}
