using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.VersionSerializers;

namespace vCardLib.Tests.Serialization.VersionSerializers;

[TestFixture]
public class VersionSerializerTests
{
    private static Dictionary<string, IFieldSerializer> CreateFieldSerializers() =>
        new List<IFieldSerializer>
        {
            new CustomFieldSerializer(),
            new AddressFieldSerializer(),
            new AgentFieldSerializer(),
            new AnniversaryFieldSerializer(),
            new BirthdayFieldSerializer(),
            new CategoriesFieldSerializer(),
            new EmailAddressFieldSerializer(),
            new FormattedNameSerializer(),
            new GenderFieldSerializer(),
            new GeoFieldSerializer(),
            new KeyFieldSerializer(),
            new KindSerializer(),
            new LabelFieldSerializer(),
            new LanguageFieldSerializer(),
            new LogoFieldSerializer(),
            new MailerFieldSerializer(),
            new MemberFieldSerializer(),
            new NameFieldSerializer(),
            new NicknameFieldSerializer(),
            new NoteFieldSerializer(),
            new OrganizationFieldSerializer(),
            new PhotoFieldSerializer(),
            new ProdIdFieldSerializer(),
            new RevisionFieldSerializer(),
            new TelephoneNumberFieldSerializer(),
            new TitleFieldSerializer(),
            new TimezoneFieldSerializer(),
            new UidFieldSerializer(),
            new UrlFieldSerializer(),
        }.ToDictionary(x => x.FieldKey, y => y);

    [TestCase(typeof(V2Serializer), "2.1")]
    [TestCase(typeof(V3Serializer), "3.0")]
    [TestCase(typeof(V4Serializer), "4.0")]
    public void Serialize_MinimalCard_EmitsBeginVersionAndEnd(System.Type serializerType, string expectedVersion)
    {
        var card = new vCard(expectedVersion switch
        {
            "2.1" => vCardVersion.v2,
            "3.0" => vCardVersion.v3,
            _ => vCardVersion.v4
        })
        {
            FormattedName = "Jane Doe"
        };

        var serializers = CreateFieldSerializers();
        var result = serializerType.Name switch
        {
            nameof(V2Serializer) => new V2Serializer(serializers).Serialize(card),
            nameof(V3Serializer) => new V3Serializer(serializers).Serialize(card),
            _ => new V4Serializer(serializers).Serialize(card)
        };

        result.ShouldContain("BEGIN:VCARD");
        result.ShouldContain($"VERSION:{expectedVersion}");
        result.ShouldContain("FN:Jane Doe");
        result.ShouldContain("END:VCARD");
    }

    [Test]
    public void V2Serializer_OmitsNickname_WhenNickNameSet()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "Jane Doe",
            NickName = "Janey"
        };

        var result = new V2Serializer(CreateFieldSerializers()).Serialize(card);

        result.ShouldNotContain("NICKNAME");
    }

    [Test]
    public void V3Serializer_IncludesNickname_WhenNickNameSet()
    {
        var card = new vCard(vCardVersion.v3)
        {
            FormattedName = "Jane Doe",
            NickName = "Janey"
        };

        var result = new V3Serializer(CreateFieldSerializers()).Serialize(card);

        result.ShouldContain("NICKNAME:Janey");
    }

    [Test]
    public void V4Serializer_IncludesGeoInV4Format()
    {
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Geo Person",
            Geo = new Geo(37.386013f, -122.08293f)
        };

        var result = new V4Serializer(CreateFieldSerializers()).Serialize(card);

        result.ShouldContain("GEO:geo:37.386013,-122.08293");
    }

    [Test]
    public void V4Serializer_EmitsOnePhotoLinePerPhoto()
    {
        var card = new vCard(vCardVersion.v4)
        {
            FormattedName = "Photo Person",
            Photos = new List<Photo>
            {
                new("http://example.com/a.jpg"),
                new("http://example.com/b.jpg")
            }
        };

        var result = new V4Serializer(CreateFieldSerializers()).Serialize(card);

        result.Split("PHOTO:").Length.ShouldBe(3);
        result.ShouldContain("http://example.com/a.jpg");
        result.ShouldContain("http://example.com/b.jpg");
    }

    [TestCase(typeof(V2Serializer))]
    [TestCase(typeof(V3Serializer))]
    [TestCase(typeof(V4Serializer))]
    public void Serialize_UsesStrictCrlfLineEndings(System.Type serializerType)
    {
        var card = new vCard(serializerType.Name switch
        {
            nameof(V2Serializer) => vCardVersion.v2,
            nameof(V3Serializer) => vCardVersion.v3,
            _ => vCardVersion.v4
        })
        {
            FormattedName = "CRLF Test",
            Note = "Short"
        };

        var serializers = CreateFieldSerializers();
        var result = serializerType.Name switch
        {
            nameof(V2Serializer) => new V2Serializer(serializers).Serialize(card),
            nameof(V3Serializer) => new V3Serializer(serializers).Serialize(card),
            _ => new V4Serializer(serializers).Serialize(card)
        };

        for (var i = 0; i < result.Length; i++)
        {
            if (result[i] == '\n' && (i == 0 || result[i - 1] != '\r'))
                Assert.Fail("Version serializer output must use CRLF only.");
        }

        result.ShouldEndWith("\r\n");
    }

    [Test]
    public void V4Serializer_RoundTripsFormattedName()
    {
        var card = new vCard(vCardVersion.v4) { FormattedName = "Round Trip" };

        var wire = new V4Serializer(CreateFieldSerializers()).Serialize(card);
        var roundTrip = vCardDeserializer.FromContent(wire).Single();

        roundTrip.FormattedName.ShouldBe("Round Trip");
        roundTrip.Version.ShouldBe(vCardVersion.v4);
    }
}
