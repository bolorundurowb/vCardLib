using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.VersionSerializers;

namespace vCardLib.Serialization;

// ReSharper disable once InconsistentNaming
public static class vCardSerializer
{
    private static readonly Dictionary<string, IFieldSerializer> FieldSerializers;

    static vCardSerializer()
    {
        var serializers = new List<IFieldSerializer>
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
            new KeyFieldSerializer(),
            new KindSerializer(),
            new LabelFieldSerializer(),
            new LanguageFieldSerializer(),
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
            new UidFieldSerializer(),
            new UrlFieldSerializer(),
        };
        FieldSerializers = serializers.ToDictionary(x => x.FieldKey, y => y);
    }

    public static string Serialize(vCard card, vCardVersion? overrideVersion = null)
    {
        var version = overrideVersion ?? card.Version;

        if (version is vCardVersion.v2) 
            return (new V2Serializer(FieldSerializers)).Serialize(card);

        throw new ArgumentException("Unknown version", nameof(version));
    }
}
