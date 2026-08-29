using System;
using System.Linq;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization;

/// <summary>
/// Field-level integration tests for <see cref="vCardDeserializer.FromContent"/>.
/// </summary>
[TestFixture]
public class vCardDeserializerFieldTests
{
    [Test]
    public void FromContent_WhenV4WithGeoKindGenderAndCategories_ParsesFields()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Rich\nGEO:12.5,-45.25\nKIND:group\nGENDER:O;non-binary\nCATEGORIES:alpha,beta\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.Must().Be(vCardVersion.v4);
        card.FormattedName.Must().Be("Rich");
        card.Geo!.Value.Latitude.Must().Be(12.5f);
        card.Geo.Value.Longitude.Must().Be(-45.25f);
        card.Kind.Must().Be(ContactKind.Group);
        card.Gender.Must().NotBeNull();
        card.Gender!.Value.Sex.Must().Be(BiologicalSex.Other);
        card.Gender.Value.GenderIdentity.Must().Be("non-binary");
        card.Categories.Count.Must().Be(2);
        card.Categories.Must().Contain("alpha");
    }

    [Test]
    public void FromContent_WhenV4WithTelEmailPhotoAdrAndCustom_ParsesFields()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Contact\nTEL:+15551234567\nEMAIL:me@example.org\nPHOTO:https://example.org/p.png\nADR:;;100 Main;City;ST;00000;US\nX-APP-ID:12345\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.PhoneNumbers.Count.Must().Be(1);
        card.EmailAddresses.Count.Must().Be(1);
        card.Photos.Count.Must().Be(1);
        card.Addresses.Count.Must().Be(1);
        card.CustomFields.Count.Must().Be(1);
        card.CustomFields[0].Key.Must().Be("X-APP-ID");
    }

    [Test]
    public void FromContent_WhenV4WithKnownAndUnknownKind_ParsesExpectedKind()
    {
        var org = "BEGIN:VCARD\nVERSION:4.0\nFN:Org\nKIND:org\nEND:VCARD";
        vCardDeserializer.FromContent(org).Single().Kind.Must().Be(ContactKind.Organization);

        var unknownKind = "BEGIN:VCARD\nVERSION:4.0\nFN:Def\nKIND:unknown-value\nEND:VCARD";
        vCardDeserializer.FromContent(unknownKind).Single().Kind.Must().Be(ContactKind.Individual);
    }

    [Test]
    public void FromContent_WhenV3WithGeoAndCategories_ParsesFields()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:V3\nGEO:1;2\nCATEGORIES:c1,c2\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.Must().Be(vCardVersion.v3);
        card.Geo!.Value.Latitude.Must().Be(1f);
        card.Categories.Count.Must().Be(2);
    }

    [Test]
    public void FromContent_WhenV2WithGeoAndCategories_ParsesFields()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:V2\nGEO:3;4\nCATEGORIES:one,two\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();
        card.Version.Must().Be(vCardVersion.v2);
        card.Geo!.Value.Longitude.Must().Be(4f);
        card.Categories.Count.Must().Be(2);
    }

    [Test]
    public void FromContent_WhenV4WithScalarProfileFields_ParsesFields()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nN:Doe;Jane;;;\nFN:Jane Doe\nUID:urn:uuid:abc\nTITLE:Director\nTZ:Europe/London\nBDAY:19850315\nREV:20240601T101530Z\nANNIVERSARY:20100704\nORG:Acme Corp;R&D\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name.Must().NotBeNull();
        card.Name!.Value.FamilyName.Must().Be("Doe");
        card.Name.Value.GivenName.Must().Be("Jane");
        card.Uid.Must().Be("urn:uuid:abc");
        card.Title.Must().Be("Director");
        card.Timezone.Must().Be("Europe/London");
        card.BirthDay.Must().NotBeNull();
        card.BirthDay!.Value.Year.Must().Be(1985);
        card.Revision.Must().NotBeNull();
        card.Anniversary.Must().NotBeNull();
        card.Organization.Must().NotBeNull();
        card.Organization!.Value.Name.Must().Be("Acme Corp");
    }

    [Test]
    public void FromContent_WhenV4WithLanguage_ParsesField()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Lang\nLANG:en-GB\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Language.Must().NotBeNull();
        card.Language!.Value.Locale.Must().Be("en-GB");
    }

    [Test]
    public void FromContent_WhenV3WithUrl_ParsesField()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:Jane\nURL:http://example.org\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Url.Must().NotBeNull();
        card.Url!.Value.Value.Must().Be("http://example.org");
    }

    [Test]
    public void FromContent_WhenV4WithUrl_ThrowsInvalidCastException()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Jane\nURL:http://example.org\nEND:VCARD";

        Ensure.Throws<InvalidCastException>(() => vCardDeserializer.FromContent(content).Single());
    }

    [Test]
    public void FromContent_WhenV4WithMultiplePhonesEmailsAndAddresses_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Multi\nTEL:+111\nTEL:+222\nEMAIL:one@example.org\nEMAIL:two@example.org\nADR:;;1 Main;A;S;1;US\nADR:;;2 Oak;B;T;2;CA\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.PhoneNumbers.Count.Must().Be(2);
        card.PhoneNumbers.Select(p => p.Number).Must().Contain("+111");
        card.PhoneNumbers.Select(p => p.Number).Must().Contain("+222");
        card.EmailAddresses.Count.Must().Be(2);
        card.EmailAddresses.Select(e => e.Value).Must().Contain("one@example.org");
        card.Addresses.Count.Must().Be(2);
        card.Addresses.Select(a => a.StreetAddress).Must().Contain("1 Main");
        card.Addresses.Select(a => a.StreetAddress).Must().Contain("2 Oak");
    }

    [Test]
    public void FromContent_WhenV4WithMultiplePhotos_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Photos\nPHOTO:https://example.org/a.png\nPHOTO:https://example.org/b.png\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Photos.Count.Must().Be(2);
        card.Photos.Select(p => p.Data).Must().Contain("https://example.org/a.png");
        card.Photos.Select(p => p.Data).Must().Contain("https://example.org/b.png");
    }

    [Test]
    public void FromContent_WhenV4WithMembers_ParsesAll()
    {
        var content =
            "BEGIN:VCARD\nVERSION:4.0\nFN:Group\nKIND:group\nMEMBER:mailto:one@example.org\nMEMBER:mailto:two@example.org\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Members.Count.Must().Be(2);
        card.Members.Must().Contain("mailto:one@example.org");
        card.Members.Must().Contain("mailto:two@example.org");
    }

    [Test]
    public void FromContent_WhenV3WithNickname_ParsesField()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:John Doe\nNICKNAME:Johnny\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.NickName.Must().Be("Johnny");
    }

    [Test]
    public void FromContent_WhenV2WithNickname_ReturnsNull()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:John Doe\nNICKNAME:Johnny\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.NickName.Must().BeNull();
    }

    [Test]
    public void FromContent_WhenV4WithNameAndNote_ParsesBoth()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nN:Smith;Sam;;;\nFN:Sam Smith\nNOTE:Important client\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name.Must().NotBeNull();
        card.Name!.Value.FamilyName.Must().Be("Smith");
        card.Note.Must().Be("Important client");
    }

    [Test]
    public void FromContent_WhenV4WithLogoLine_StoresCustomField()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Logo\nLOGO:https://example.org/logo.png\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Logo.Must().BeNull();
        card.CustomFields.Count.Must().Be(1);
        card.CustomFields[0].Key.Must().Be("LOGO:https");
        card.CustomFields[0].Value.Must().Be("//example.org/logo.png");
    }

    [Test]
    public void FromContent_WhenV4WithAgentAndMailer_ReturnsNull()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:V4\nAGENT:Rep\nMAILER:Client\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Agent.Must().BeNull();
        card.Mailer.Must().BeNull();
    }

    [Test]
    public void FromContent_WhenV3WithAgentMailerAndProdId_ParsesFields()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:V3\nAGENT:Rep\nMAILER:Thunderbird\nPRODID:-//Example//vCard//EN\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Agent.Must().Be("Rep");
        card.Mailer.Must().Be("Thunderbird");
    }

    [Test]
    public void FromContent_WhenV2WithMailer_ParsesField()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:V2\nMAILER:PicoMail\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Mailer.Must().Be("PicoMail");
    }

    [Test]
    public void FromContent_WhenMultipleCustomFields_ParsesAll()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Custom\nX-FOO:bar\nX-BAZ:qux\nEND:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.CustomFields.Count.Must().Be(2);
        card.CustomFields.Must().Contain(kv => kv.Key == "X-FOO" && kv.Value == "bar");
        card.CustomFields.Must().Contain(kv => kv.Key == "X-BAZ" && kv.Value == "qux");
    }
}
